using UnityEngine;

/// <summary>
/// Ÿ�̸�. ���� Reset���κ��� ���� �̸� ����ߴ� �ð�(��) Ȥ�� ���ϴ� �ð�(��)�� �������� �˻��� �� ����
/// </summary>
public class TickTimer
{
    /// <summary>
    /// ����� �ð�
    /// </summary>
    public float time;

    /// <summary>
    /// �˻��� �ð��� �⺻��
    /// </summary>
    public float checkTime;
    private bool autoReset;
    private bool unscaledTime;

    //�Ͻ�����
    private bool isPaused;
    private float pauseTime;

    /// <summary>
    /// ���� �ð�
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
    /// ������ ���ķ� �ش� �ð���ŭ ��������
    /// </summary>
    /// <param name="time">�ʿ��� ��� �ð�(s)</param>
    /// <returns>time��ŭ ����Ͽ��°�?</returns>
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
    /// ������ ���� �ش� �ð���ŭ �������� �󸶳� ���Ҵ���
    /// </summary>
    /// <param name="time">�ʿ��� ��� �ð�(s)</param>
    /// <returns>���� �ð�(s)</returns>
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
    /// �ð��� ������ ������ �����
    /// </summary>
    /// <param name="time">������ �� �ð�</param>
    public void AddOffset(float time)
    {
        this.time -= time;
    }

    /// <summary>
    /// Ÿ�̸Ӹ� �Ͻ������մϴ�.
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
    /// �Ͻ������� Ÿ�̸Ӹ� �ٽ� �����մϴ�.
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
