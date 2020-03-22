#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761のダウン時の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/INF-C-761/Down", fileName = "down.inf_c_761.asset", order = 4)]
public class InfC761DownBehavior : BattleRealEnemyBehaviorUnit
{
    #region Define

    private enum E_STATE
    {
        WAIT,
        MOVE_TO_RIGHT,
        MOVE_TO_LEFT
    }

    private class StateCycle : StateCycleBase<InfC761DownBehavior, E_STATE> { }

    private class InnerState : State<E_STATE, InfC761DownBehavior>
    {
        public InnerState(E_STATE state, InfC761DownBehavior target) : base(state, target) { }
        public InnerState(E_STATE state, InfC761DownBehavior target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field Inspector Readonly!

    [SerializeField]
    private float m_Amplitude;
    public float Amplitude => m_Amplitude;

    [SerializeField]
    private float m_NextMoveWaitTime;
    public float NextMoveWaitTime => m_NextMoveWaitTime;

    [SerializeField]
    private float m_MoveDuration;
    public float MoveDuration => m_MoveDuration;

    [SerializeField]
    private AnimationCurve m_NormalizedRate;
    public AnimationCurve NormalizedRate => m_NormalizedRate;

    #endregion

    #region Field

    private StateMachine<E_STATE, InfC761DownBehavior> m_StateMachine;

    private E_STATE m_NextState;
    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;
    private float m_Duration;
    private float m_TimeCount;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        m_StateMachine = new StateMachine<E_STATE, InfC761DownBehavior>();
        m_StateMachine.AddState(new InnerState(E_STATE.WAIT, this, new WaitState()));
        m_StateMachine.AddState(new InnerState(E_STATE.MOVE_TO_LEFT, this, new MoveToLeftState()));
        m_StateMachine.AddState(new InnerState(E_STATE.MOVE_TO_RIGHT, this, new MoveToRightState()));

        m_TimeCount = 0;
        m_Duration = NextMoveWaitTime;
        m_MoveStartPos = Enemy.transform.position;
        m_MoveEndPos = m_MoveStartPos;
        m_NextState = E_STATE.MOVE_TO_LEFT;

        RequestChangeState(E_STATE.WAIT);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        m_StateMachine.OnUpdate();
    }

    protected override void OnEnd()
    {
        m_StateMachine.OnFinalize();
        m_StateMachine = null;
        base.OnEnd();
    }

    #endregion

    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }
    
    private Vector3 GetMovePosition()
    {
        var rate = NormalizedRate;
        var duration = rate.keys[rate.keys.Length - 1].time;
        var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
        return Vector3.Lerp(m_MoveStartPos, m_MoveEndPos, t);
    }

    private class WaitState : StateCycle
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.m_TimeCount += Time.deltaTime;
            if (Target.m_TimeCount >= Target.m_Duration)
            {
                Target.RequestChangeState(Target.m_NextState);
            }
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.m_TimeCount = 0;
            Target.m_Duration = Target.MoveDuration;
        }
    }

    private class MoveState : StateCycle
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.m_TimeCount += Time.deltaTime;
            Target.Enemy.transform.position = Target.GetMovePosition();
            if (Target.m_TimeCount >= Target.m_Duration)
            {
                Target.RequestChangeState(E_STATE.WAIT);
            }
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.m_TimeCount = 0;
            Target.m_Duration = Target.NextMoveWaitTime;
            Target.m_MoveStartPos = Target.Enemy.transform.position;

            var amplitude = Target.Amplitude;
            Target.m_MoveEndPos = Target.m_MoveStartPos + Vector3.left * amplitude;
            Target.m_MoveEndPos.z += UnityEngine.Random.Range(-amplitude / 2, amplitude / 2);
        }
    }

    private class MoveToLeftState : MoveState
    {
        public override void OnEnd()
        {
            base.OnEnd();
            Target.m_NextState = E_STATE.MOVE_TO_RIGHT;
        }
    }

    private class MoveToRightState : MoveState
    {
        public override void OnEnd()
        {
            base.OnEnd();
            Target.m_NextState = E_STATE.MOVE_TO_LEFT;
        }
    }
}
