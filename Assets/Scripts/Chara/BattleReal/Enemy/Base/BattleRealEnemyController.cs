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
        /// 生成直後に遷移するステート
        /// </summary>
        START,

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

    private E_ENEMY_BEHAVIOR_TYPE m_BehaviorType;
    private BattleRealEnemyBehaviorUnit m_Behavior;
    private BattleRealEnemyBehaviorGroup m_BehaviorGroup;

    private BattleRealEnemyBehaviorController m_BehaviorController;

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
        m_StateMachine.AddState(new InnerState(E_STATE.START, this, new StartState()));
        m_StateMachine.AddState(new InnerState(E_STATE.BEHAVIOR, this, new BehaviorState()));
        m_StateMachine.AddState(new InnerState(E_STATE.SEQUENCE, this, new SequenceState()));
        m_StateMachine.AddState(new InnerState(E_STATE.DEAD, this, new DeadState()));

        m_Behavior = null;
        m_BehaviorGroup = null;
        if (m_EnemyParam != null)
        {
            m_BehaviorType = m_EnemyParam.BehaviorType;
            if (m_BehaviorType == E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT)
            {
                if (m_EnemyParam.Behavior != null)
                {
                    m_Behavior = Instantiate(m_EnemyParam.Behavior);
                }
            }
            else
            {
                m_BehaviorGroup = m_EnemyParam.BehaviorGroup;
            }
        }

        m_BehaviorController = new BattleRealEnemyBehaviorController(this);
        m_BehaviorController.OnInitialize();

        RequestChangeState(E_STATE.START);
    }

    public override void OnFinalize()
    {
        m_BehaviorController.OnFinalize();
        m_BehaviorController = null;

        m_BehaviorGroup = null;
        m_Behavior = null;
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
