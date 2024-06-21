using System;
using System.Collections.Generic;

/// <summary>
/// 주로 FSM의 상태 변화를 나타내며 조건과 실행부로 이루어진 클래스
/// </summary>
public class Transition
{
    /// <summary>
    /// 조건
    /// </summary>
    public Func<bool> Condition { get; }
    /// <summary>
    /// 실행부
    /// </summary>
    public Action Action { get; }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="condition">조건</param>
    /// <param name="action">실행부</param>
    public Transition(Func<bool> condition, Action action)
    {
        Condition = condition;
        Action = action;
    }

    /// <summary>
    /// 받은 리스트를 위에서부터 검사하여 처음 true인 것을 실행 후 true 반환, 모두 false면 false 반환
    /// </summary>
    /// <param name="transitions">검사할 트랜지션</param>
    /// <returns>실행했는가?</returns>
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
