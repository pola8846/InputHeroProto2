/// <summary>
/// FSM에서 하나의 상태를 나타내는 기본  클래스
/// </summary>
public class State
{
    protected Unit unit;
    protected StateMachine machine;

    public State(StateMachine machine)
    {
        unit = machine.Unit;
        this.machine = machine;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
