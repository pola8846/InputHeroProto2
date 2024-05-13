using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickCounter
{
    /// <summary>
    /// 현재 부른 횟수
    /// </summary>
    private int tickCount;

    /// <summary>
    /// 검사할 횟수
    /// </summary>
    private int tickTargetNum;

    /// <summary>
    /// 상한을 넘겼을 때 자동으로 리셋할 것인지
    /// </summary>
    private bool autoReset;

    public TickCounter(int targetNum, bool autoReset = true)
    {
        tickTargetNum = targetNum;
        this.autoReset = autoReset;
    }

    public void Reset()
    {
        tickCount = 0;
    }

    public bool Check()
    {
        bool result = false;
        tickCount++;

        if (tickCount>=tickTargetNum)
        {
            result = true;
            if (autoReset)
            {
                Reset();
            }
        }

        return result;
    }
}
