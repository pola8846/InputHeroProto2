using UnityEngine;

/// <summary>
/// 타이머. 이전 Reset으로부터 지난 미리 등록했던 시간(초) 혹은 원하는 시간(초)이 지났는지 검사할 수 있음
/// </summary>
public class TickTimer
{
    /// <summary>
    /// 검사 기준 시간
    /// </summary>
    public float time;

    /// <summary>
    /// 검사할 시간의 기본값
    /// </summary>
    public float checkTime;//검사할 시간(초)
    private bool autoReset;//검사 이후 true 받으면 자동 리셋?
    private bool unscaledTime;//배속 영향 받는가?

    //일시정지
    private bool isPaused;//정지 중인가?
    private float pauseTime;//정지 걸었던 시간

    //경과 시간 카운트
    private float elapsedTime;//총 경과 시간
    private float lastUpdatedTime;//마지막으로 검사한 시간

    /// <summary>
    /// 현재 시간
    /// </summary>
    private float NowTime
    {
        get
        {
            if (unscaledTime)
            {
                return Time.unscaledTime;
            }
            else
            {
                return Time.time;
            }
        }
    }

    //생성자
    public TickTimer(float checkTime = 1f, bool isTrigerInstant = false, bool autoReset = false, bool unscaledTime = false)
    {
        this.checkTime = checkTime;
        this.unscaledTime = unscaledTime;
        if (isTrigerInstant)
        {
            time = float.MinValue;
            elapsedTime = float.MaxValue;
            lastUpdatedTime = time;
        }
        else
        {
            Reset();
        }
        this.autoReset = autoReset;
    }

    /// <summary>
    /// 초기화. 새로 타이머 파는 대신 이걸 사용해도 됨
    /// </summary>
    public void Reset()
    {
        time = NowTime;
        elapsedTime = 0f;
        lastUpdatedTime = time;
    }

    /// <summary>
    /// 설정한 이후로 해당 시간만큼 지났는지
    /// </summary>
    /// <param name="time">필요한 경과 시간(s)</param>
    /// <returns>time만큼 경과하였는가?</returns>
    public bool Check(float time, float timeRate = 1)
    {
        bool result;

        if (timeRate < 0)
        {
            timeRate = 1;
        }

        UpdateElapsedTime(timeRate);

        result = elapsedTime >= time;
        if (result && autoReset)
        {
            Reset();
        }
        return result;

        //일시정지 도중의 처리. 작업 우선도가 낮아져서 일단 주석 처리해둠
        /*
        if (isPaused)
        {
            UpdateElapsedTime(timeRate);
            //result = this.time + time <= pauseTime;
            result = elapsedTime >= time;
            if (result && autoReset)
            {
                Reset();
            }
            return result;
        }
        else
        {
            UpdateElapsedTime(timeRate);

            //result = this.time + time <= NowTime;
            result = elapsedTime >= time;
            if (result && autoReset)
            {
                Reset();
            }
            return result;
        }
        */
    }

    /// <summary>
    /// 기본 설정된 시간으로 검사
    /// </summary>
    /// <returns>설정한 시간만큼 지났는가?</returns>
    public bool Check()
    {
        return Check(checkTime);
    }

    /// <summary>
    /// 설정한 이후 해당 시간만큼 지나려면 얼마나 남았는지
    /// </summary>
    /// <param name="time">필요한 경과 시간(s)</param>
    /// <returns>남은 시간(s)</returns>
    public float GetRemain(float time)
    {
        if (isPaused)
        {
            return Mathf.Max(0, (this.time + time) - pauseTime);
        }
        else
        {
            return Mathf.Max(0, (this.time + time) - NowTime);
        }
    }

    /// <summary>
    /// 기본 설정된 시간으로 남은 시간 검사
    /// </summary>
    /// <returns>남은 시간</returns>
    public float GetRemain()
    {
        return GetRemain(checkTime);
    }

    /// <summary>
    /// 시간을 강제로 지나게 만들기
    /// </summary>
    /// <param name="time">지나게 할 시간</param>
    public void AddOffset(float time)
    {
        this.time -= time;
    }

    /// <summary>
    /// 타이머를 일시정지
    /// </summary>
    public void Pause()
    {
        if (!isPaused)
        {
            UpdateElapsedTime();
            pauseTime = NowTime - time;
            isPaused = true;
        }
    }

    /// <summary>
    /// 일시정지된 타이머를 다시 시작
    /// </summary>
    public void Resume()
    {
        if (isPaused)
        {
            UpdateElapsedTime();
            time = NowTime - pauseTime;
            isPaused = false;
        }
    }

    /// <summary>
    /// 시간 강제 갱신. 일시정지 혹은 시간 배율 변화 등이 일어날 때 사용
    /// </summary>
    /// <param name="timeRate">시간 변화 얼마나 적용할지 배율</param>
    public void UpdateElapsedTime(float timeRate = 1)
    {
        //일시정지 중엔 시간이 흐르지 않는다
        if (isPaused)
        {
            lastUpdatedTime = NowTime;
            return;
        }

        float result = 0;

        //지난 시간
        float timeGap = NowTime - lastUpdatedTime;

        if (!unscaledTime)
        {
            result = timeGap * GetConvertedTimeRate(timeRate) / Time.timeScale;
        }
        else
        {
            result = timeGap;
        }

        elapsedTime += result;
        lastUpdatedTime = NowTime;
    }


    /// <summary>
    /// 기존 timeScale 대신 1을 기준으로 Rate 비율만큼만 변한 시간 배율 반환
    /// </summary>
    /// <param name="timeRate">배속 적용할 비율</param>
    /// <returns>실제 적용될 배속 값</returns>
    public static float GetConvertedTimeRate(float timeRate)
    {
        float rateGap = Time.timeScale - 1;
        float rate = 1 + (rateGap * timeRate);
        return rate;
    }
}
