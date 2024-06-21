/// <summary>
/// FSM에서 하나의 상태를 나타내는 기본  클래스
/// </summary>
public class State
{
    /// <summary>
    /// 모체 유닛
    /// </summary>
    protected Unit unit;

    /// <summary>
    /// 모체 상태머신
    /// </summary>
    protected StateMachine machine;

    public State(StateMachine machine)
    {
        unit = machine.Unit;
        this.machine = machine;
    }

    /// <summary>
    /// 상태 진입 시 실행될 함수
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// 해당 상태인 동안 매 프레임마다 반복 실행할 함수(Update 타이밍)
    /// </summary>
    public virtual void Execute() { }

    /// <summary>
    /// 상태 종료 시 실행될 함수
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// 상태 변경. 아무 곳에서도 변경하지 않으면 영원히 해당 상태에 갇힘
    /// </summary>
    /// <typeparam name="T">변경할 상태</typeparam>
    protected void ChangeState<T>() where T : State
    {
        machine.ChangeState<T>();
    }
}
