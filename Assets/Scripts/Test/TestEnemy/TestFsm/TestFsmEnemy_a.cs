using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class TestFsmEnemy_a : State
    {
        private TickTimer _timer;

        public TestFsmEnemy_a(StateMachine machine) : base(machine)
        {
            _timer = new TickTimer();
        }

        public override void Enter()
        {
            base.Enter();
            _timer.Reset();
            Debug.Log("TestFsmEnemy_a.Enter");
        }

        public override void Execute()
        {
            base.Execute();
            //Debug.Log("TestFsmEnemy_a.Execute");
            if (_timer != null && _timer.Check(1))
            {
                machine.ChangeState<TestFsmEnemy_delay>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("TestFsmEnemy_a.Exit");
        }
    }
