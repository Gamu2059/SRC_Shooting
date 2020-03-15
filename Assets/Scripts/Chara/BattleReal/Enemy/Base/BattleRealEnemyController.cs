using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの雑魚敵を制御するクラス。
/// </summary>
public partial class BattleRealEnemyController : BattleRealEnemyBase
{
    #region Define

    private enum E_STATE
    {
        /// <summary>
        /// 移動したり攻撃したりするためのステート
        /// </summary>
        BEHAVIOR,

        /// <summary>
        /// シーケンスによる自動制御を受けるステート
        /// </summary>
        SEQUENCE,

        /// <summary>
        /// 撃破された瞬間だけ遷移するステート
        /// </summary>
        DEAD,
    }

    private class StateCycle : StateCycleBase<BattleRealEnemyController, E_STATE> { }

    private class InnerState : State<E_STATE, BattleRealEnemyController>
    {
        public InnerState(E_STATE state, BattleRealEnemyController target) : base(state, target) { }
        public InnerState(E_STATE state, BattleRealEnemyController target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field

    private StateMachine<E_STATE, BattleRealEnemyController> m_StateMachine;
    private BattleRealEnemyParam m_EnemyParam;
    private BattleRealEnemyBehaviorUnit m_EnemyBehavior;

    #endregion

    #region Game Cycle

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();
        m_EnemyParam = Param as BattleRealEnemyParam;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_STATE, BattleRealEnemyController>();
        m_StateMachine.AddState(new InnerState(E_STATE.BEHAVIOR, this, new BehaviorState()));
        m_StateMachine.AddState(new InnerState(E_STATE.SEQUENCE, this, new SequenceState()));
        m_StateMachine.AddState(new InnerState(E_STATE.DEAD, this, new DeadState()));

        if (m_EnemyParam != null && m_EnemyParam.OnInitializeEvents != null)
        {
            BattleRealEventManager.Instance.AddEvent(m_EnemyParam.OnInitializeEvents);
        }

        if (m_EnemyParam != null && m_EnemyParam.OnInitializeSequence)
        {
            RequestChangeState(E_STATE.SEQUENCE);
        }
        else
        {
            RequestChangeState(E_STATE.BEHAVIOR);
        }

        m_EnemyBehavior = null;
        if (m_EnemyParam != null && m_EnemyParam.Behavior != null)
        {
            m_EnemyBehavior = Instantiate(m_EnemyParam.Behavior);
            m_EnemyBehavior.SetEnemy(this);
        }
        m_EnemyBehavior?.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_EnemyBehavior?.OnFinalize();
        m_EnemyBehavior = null;
        m_StateMachine.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_StateMachine.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_StateMachine.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        m_StateMachine.OnFixedUpdate();
    }

    #endregion

    /// <summary>
    /// 敵のステートを変更する。
    /// </summary>
    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    protected sealed override void OnDead()
    {
        base.OnDead();
        RequestChangeState(E_STATE.DEAD);
    }
}
