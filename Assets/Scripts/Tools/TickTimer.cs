using UnityEngine;

/// <summary>
/// 타이머. 이전 Reset으로부터 지난 미리 등록했던 시간(초) 혹은 원하는 시간(초)이 지났는지 검사할 수 있음
/// </summary>
public class TickTimer
{
    /// <summary>
    /// 경과한 시간
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
    }

    /// <summary>
    /// 설정한 이후로 해당 시간만큼 지났는지
    /// </summary>
    /// <param name="time">필요한 경과 시간(s)</param>
    /// <returns>time만큼 경과하였는가?</returns>
    public bool Check(float time)
    {
        bool result;
        if (isPaused)
        {
            result = this.time + time <= pauseTime;
            if (result && autoReset)
            {
                Reset();
            }
            return result;
        }
        else
        {
            result = this.time + time <= NowTime;
            if (result && autoReset)
            {
                Reset();
            }
            return result;
        }
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
            time = NowTime - pauseTime;
            isPaused = false;
        }
    }
}
