using UnityEngine;

/// <summary>
/// 타이머. 이전 Reset으로부터 지난 미리 등록했던 시간(초) 혹은 원하는 시간(초)이 지났는지 검사할 수 있음
/// </summary>
public class TickTimer
{
    public float time;
    private float checkTime;
    private bool autoReset;
    private bool unscaledTime;

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
        PerformanceManager.StartTimer("TickTimer.Check");
        bool result = this.time + time <= NowTime;
        if (result && autoReset)
        {
            Reset();
        }
        PerformanceManager.StopTimer("TickTimer.Check");
        return result;
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
        if (this.time + time <= NowTime)
        {
            return 0f;
        }
        else
        {
            return (this.time + time) - NowTime;
        }
    }

    public float GetRemain()
    {
        return GetRemain(this.checkTime);
    }
}
