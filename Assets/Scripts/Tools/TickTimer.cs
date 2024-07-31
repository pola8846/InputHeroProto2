using UnityEngine;

/// <summary>
/// Ÿ�̸�. ���� Reset���κ��� ���� �̸� ����ߴ� �ð�(��) Ȥ�� ���ϴ� �ð�(��)�� �������� �˻��� �� ����
/// </summary>
public class TickTimer
{
    /// <summary>
    /// �˻� ���� �ð�
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

    //��� �ð� ī��Ʈ
    private float elapsedTime;
    private float lastUpdatedTime;

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
    /// ������ ���ķ� �ش� �ð���ŭ ��������
    /// </summary>
    /// <param name="time">�ʿ��� ��� �ð�(s)</param>
    /// <returns>time��ŭ ����Ͽ��°�?</returns>
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
            UpdateElapsedTime();
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
            UpdateElapsedTime();
            time = NowTime - pauseTime;
            isPaused = false;
        }
    }

    /// <summary>
    /// �ð� ���� ����
    /// </summary>
    /// <param name="timeRate">�ð� ��ȭ �󸶳� �������� ����</param>
    public void UpdateElapsedTime(float timeRate = 1)
    {
        //�Ͻ����� �߿� �ð��� �帣�� �ʴ´�
        if (isPaused)
        {
            lastUpdatedTime = NowTime;
            return;
        }

        float result = 0;

        //���� �ð�
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
    /// ���� timeScale ��� 1�� �������� Rate ������ŭ�� ���� �ð� ���� ��ȯ
    /// </summary>
    /// <param name="timeRate">��� ������ ����</param>
    /// <returns>���� ����� ��� ��</returns>
    public static float GetConvertedTimeRate(float timeRate)
    {
        float rateGap = Time.timeScale - 1;
        float rate = 1 + (rateGap * timeRate);
        return rate;
    }
}
