/// <summary>
/// FSM���� �ϳ��� ���¸� ��Ÿ���� �⺻  Ŭ����
/// </summary>
public class State
{
    /// <summary>
    /// ��ü ����
    /// </summary>
    protected Unit unit;

    /// <summary>
    /// ��ü ���¸ӽ�
    /// </summary>
    protected StateMachine machine;

    public State(StateMachine machine)
    {
        unit = machine.Unit;
        this.machine = machine;
    }

    /// <summary>
    /// ���� ���� �� ����� �Լ�
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// �ش� ������ ���� �� �����Ӹ��� �ݺ� ������ �Լ�(Update Ÿ�̹�)
    /// </summary>
    public virtual void Execute() { }

    /// <summary>
    /// ���� ���� �� ����� �Լ�
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// ���� ����. �ƹ� �������� �������� ������ ������ �ش� ���¿� ����
    /// </summary>
    /// <typeparam name="T">������ ����</typeparam>
    protected void ChangeState<T>() where T : State
    {
        machine.ChangeState<T>();
    }
}
