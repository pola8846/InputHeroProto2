using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ð����� Ư�� ������ �����ϴ� �� �ݺ��ϴ� ����
/// </summary>
public class TimedState : State
{
    protected TickTimer timer;
    protected int counter;//������ Ƚ��
    public TimedState(StateMachine machine):base(machine)
    {
        timer = new(autoReset: true);
    }

    public override void Enter()
    {
        timer.Reset();
        counter = 0;
    }

    public override void Execute()
    {
        if (timer.Check())
        {
            counter++;
            Main();
            timer.Reset();
        }
    }
    
    protected virtual void Main()
    {

    }
}

