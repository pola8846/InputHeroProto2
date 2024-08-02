public class TickCounter
{
    /// <summary>
    /// ���� �θ� Ƚ��
    /// </summary>
    private int tickCount;

    /// <summary>
    /// �˻��� Ƚ��
    /// </summary>
    private int tickTargetNum;

    /// <summary>
    /// ������ �Ѱ��� �� �ڵ����� ������ ������
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

        if (tickCount >= tickTargetNum)
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
