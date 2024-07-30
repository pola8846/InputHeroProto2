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
    public float checkTime;
    private bool autoReset;
    private bool unscaledTime;

    //일시정지
    private bool isPaused;
    private float pauseTime;

    //경과 시간 카운트
    private float elapsedTime;
    private float lastUpdatedTime;

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
    /// 타이머를 일시정지합니다.
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
    /// 일시정지된 타이머를 다시 시작합니다.
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
    /// 시간 강제 갱신
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
            result = timeGap * GetConvertedTimeRate(timeRate);
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
