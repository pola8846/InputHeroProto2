using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickTimer
{
    private float time;
    private float checkTime;
    private bool autoReset;

    public TickTimer(float checkTime = 1f, bool isTrigerInstant = false, bool autoReset = false)
    {
        this.checkTime = checkTime;
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
        time = Time.time;
    }

    /// <summary>
    /// 설정한 이후로 해당 시간만큼 지났는지
    /// </summary>
    /// <param name="time">필요한 경과 시간(s)</param>
    /// <returns>time만큼 경과하였는가?</returns>
    public bool Check(float time)
    {
        PerformanceManager.StartTimer("TickTimer.Check");
        bool result = this.time + time <= Time.time;
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
}
