using UnityEngine;

/// <summary>
/// Ÿ�̸�. ���� Reset���κ��� ���� �̸� ����ߴ� �ð�(��) Ȥ�� ���ϴ� �ð�(��)�� �������� �˻��� �� ����
/// </summary>
public class TickTimer
{
    public float time;
    public float checkTime;
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
    /// ������ ���ķ� �ش� �ð���ŭ ��������
    /// </summary>
    /// <param name="time">�ʿ��� ��� �ð�(s)</param>
    /// <returns>time��ŭ ����Ͽ��°�?</returns>
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
    /// ������ ���� �ش� �ð���ŭ �������� �󸶳� ���Ҵ���
    /// </summary>
    /// <param name="time">�ʿ��� ��� �ð�(s)</param>
    /// <returns>���� �ð�(s)</returns>
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

    /// <summary>
    /// �ð��� ������ ������ �����
    /// </summary>
    /// <param name="time">������ �� �ð�</param>
    public void AddOffset(float time)
    {
        this.time -= time;
    }
}
