using System;
using System.Collections.Generic;

/// <summary>
/// FSM�� �����ϴ� Ŭ����
/// </summary>
public class StateMachine
{
    /// <summary>
    /// ��ü ����
    /// </summary>
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

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="unit">��ü�� ��ϵ� ����</param>
    public StateMachine(Unit unit)
    {
        this.unit = unit;
    }

    public string GetCurrentState()
    {
        return currentState.ToString();
    }

    /// <summary>
    /// �ش� ���·� ��ȯ
    /// </summary>
    /// <typeparam name="T">��ȯ�� ������ Ÿ��</typeparam>
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
    /// �ش� ���·� ��ȯ
    /// </summary>
    /// <param name="state">��ȯ�� ���� ��ü</param>
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
    /// state�� update �̺�Ʈ ����
    /// </summary>
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    /// <summary>
    /// �Ͻ����� ������. ���� ��
    /// </summary>
    public void Pause()
    {
        currentState?.Pause();
    }

    /// <summary>
    /// �Ͻ����� ���� ������. ���� ��
    /// </summary>
    public void Resume()
    {
        currentState?.Resume();
    }
}
