using System;
using System.Collections.Generic;

/// <summary>
/// FSM�� �����ϴ� Ŭ����
/// </summary>
public class StateMachine
{
    private Unit unit;
    public Unit Unit => unit;

    /// <summary>
    /// ���� ����
    /// </summary>
    private State currentState;

    /// <summary>
    /// ���� ĳ�̿�
    /// </summary>
    private Dictionary<Type, State> stateInstances = new Dictionary<Type, State>();

    public StateMachine(Unit unit)
    {
        this.unit = unit;
    }

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

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    public void Pause()
    {
        currentState?.Pause();
    }

    public void Resume()
    {
        currentState?.Resume();
    }
}
