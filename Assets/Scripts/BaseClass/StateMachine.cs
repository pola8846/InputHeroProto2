using System;
using System.Collections.Generic;

/// <summary>
/// FSM을 관리하는 클래스
/// </summary>
public class StateMachine
{
    /// <summary>
    /// 모체 유닛
    /// </summary>
    private Unit unit;
    public Unit Unit => unit;

    /// <summary>
    /// 현재 상태
    /// </summary>
    private State currentState;

    /// <summary>
    /// 상태 캐싱용
    /// </summary>
    private Dictionary<Type, State> stateInstances = new Dictionary<Type, State>();

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="unit">모체로 등록될 유닛</param>
    public StateMachine(Unit unit)
    {
        this.unit = unit;
    }

    public string GetCurrentState()
    {
        return currentState.ToString();
    }

    /// <summary>
    /// 해당 상태로 전환
    /// </summary>
    /// <typeparam name="T">전환할 상태의 타입</typeparam>
    public void ChangeState<T>() where T : State
    {
        Type type = typeof(T);
        if (!stateInstances.ContainsKey(type))
        {
            State state = Activator.CreateInstance(type, this) as State;
            stateInstances.Add(type, state);
        }
        currentState?.Exit();
        currentState = stateInstances[type];
        currentState.Enter();
    }

    /// <summary>
    /// 해당 상태로 전환
    /// </summary>
    /// <param name="state">전환할 상태 개체</param>
    public void ChangeState(State state)
    {
        Type type = state.GetType();
        if (!stateInstances.ContainsKey(type))
        {
            State s = new(this);
            stateInstances.Add(type, s);
        }
        currentState?.Exit();
        currentState = stateInstances[type];
        currentState.Enter();
    }

    /// <summary>
    /// state에 update 이벤트 전달
    /// </summary>
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    /// <summary>
    /// 일시정지 대응용. 제작 중
    /// </summary>
    public void Pause()
    {
        currentState?.Pause();
    }

    /// <summary>
    /// 일시정지 해제 대응용. 제작 중
    /// </summary>
    public void Resume()
    {
        currentState?.Resume();
    }
}
