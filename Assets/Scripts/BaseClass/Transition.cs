using System;
using System.Collections.Generic;

/// <summary>
/// �ַ� FSM�� ���� ��ȭ�� ��Ÿ���� ���ǰ� ����η� �̷���� Ŭ����
/// </summary>
public class Transition
{
    /// <summary>
    /// ����
    /// </summary>
    public Func<bool> Condition { get; }
    /// <summary>
    /// �����
    /// </summary>
    public Action Action { get; }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="condition">����</param>
    /// <param name="action">�����</param>
    public Transition(Func<bool> condition, Action action)
    {
        Condition = condition;
        Action = action;
    }

    /// <summary>
    /// ���� ����Ʈ�� ���������� �˻��Ͽ� ó�� true�� ���� ���� �� true ��ȯ, ��� false�� false ��ȯ
    /// </summary>
    /// <param name="transitions">�˻��� Ʈ������</param>
    /// <returns>�����ߴ°�?</returns>
    public static bool EvaluateTransitions(List<Transition> transitions)
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            if (transitions[i].Condition())
            {
                transitions[i].Action();
                return true;
            }
        }

        return false;
    }
}
