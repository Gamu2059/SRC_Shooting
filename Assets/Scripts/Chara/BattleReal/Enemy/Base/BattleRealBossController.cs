using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスを制御するクラス。
/// </summary>
public partial class BattleRealBossController : BattleRealEnemyBase
{
    #region Define

    private enum E_STATE
    {
        /// <summary>
        /// 移動したり攻撃したりするためのステート
        /// </summary>
        BEHAVIOR,

        /// <summary>
        /// ダウンした時の移動ステート
        /// </summary>
        DOWN_BEHAVIOR,

        /// <summary>
        /// 移動や攻撃等を変更するためのステート
        /// </summary>
        CHANGE_BEHAVIOR,

        /// <summary>
        /// ハッキングに成功した時に遷移するステート
        /// </summary>
        HACKING_SUCCESS,

        /// <summary>
        /// ハッキングに失敗した時に遷移するステート
        /// </summary>
        HACKING_FAILURE,

        /// <summary>
        /// シーケンスによる自動制御を受けるステート
        /// </summary>
        SEQUENCE,

        /// <summary>
        /// 撃破された時に遷移するステート
        /// </summary>
        DEAD,

        /// <summary>
        /// 救出された時に遷移するステート
        /// </summary>
        RESCUE,

        /// <summary>
        /// 全て終えた時に遷移するステート
        /// </summary>
        END,
    }

    private class StateCycle : StateCycleBase<BattleRealBossController, E_STATE> { }

    private class InnerState : State<E_STATE, BattleRealBossController>
    {
        public InnerState(E_STATE state, BattleRealBossController target) : base(state, target) { }
        public InnerState(E_STATE state, BattleRealBossController target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field

    private StateMachine<E_STATE, BattleRealBossController> m_StateMachine;
    private BattleRealBossParam m_BossParam;
    private BattleRealEnemyBehaviorUnit m_CurrentBehavior;
    private BattleRealEnemyBehaviorUnit m_CurrentDownBehavior;

    public float NowDownHp { get; private set; }
    public float MaxDownHp { get; private set; }
    public int HackingCompleteNum { get; private set; }
    public int HackingSuccessCount { get; private set; }

    #endregion

    #region Game Cycle

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();
        m_BossParam = Param as BattleRealBossParam;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_STATE, BattleRealBossController>();
        m_StateMachine.AddState(new InnerState(E_STATE.BEHAVIOR, this, new BehaviorState()));
        m_StateMachine.AddState(new InnerState(E_STATE.DOWN_BEHAVIOR, this, new DownBehaviorState()));
        m_StateMachine.AddState(new InnerState(E_STATE.CHANGE_BEHAVIOR, this, new ChangeBehaviorState()));
        m_StateMachine.AddState(new InnerState(E_STATE.HACKING_SUCCESS, this, new HackingSuccessState()));
        m_StateMachine.AddState(new InnerState(E_STATE.HACKING_FAILURE, this, new HackingFailureState()));
        m_StateMachine.AddState(new InnerState(E_STATE.SEQUENCE, this, new SequenceState()));
        m_StateMachine.AddState(new InnerState(E_STATE.DEAD, this, new DeadState()));
        m_StateMachine.AddState(new InnerState(E_STATE.RESCUE, this, new RescueState()));
        m_StateMachine.AddState(new InnerState(E_STATE.END, this, new EndState()));
    }

    public override void OnFinalize()
    {
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

    #region Suffer

    protected override void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var colliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (colliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            // 通常状態の時にエナジー弾を食らうとダウンダメージになる
            var currentState = m_StateMachine.CurrentState.Key;
            if (currentState == E_STATE.BEHAVIOR)
            {
                switch (sufferData.OpponentObject)
                {
                    case HackerBullet hackerBullet:
                        DamageDownHp(hackerBullet.GetNowDownDamage());
                        break;
                }
            }
        }
    }

    protected override void OnEnterSufferChara(HitSufferData<BattleRealCharaController> sufferData)
    {
        base.OnEnterSufferChara(sufferData);

        var sufferType = sufferData.SufferCollider.Transform.ColliderType;
        switch (sufferType)
        {
            case E_COLLIDER_TYPE.CRITICAL:
                break;
            case E_COLLIDER_TYPE.ENEMY_HACKING:
                var currentState = m_StateMachine.CurrentState.Key;
                if (currentState == E_STATE.DOWN_BEHAVIOR)
                {
                    var colliderType = sufferData.HitCollider.Transform.ColliderType;
                    if (colliderType == E_COLLIDER_TYPE.PLAYER_HACKING)
                    {
                        var index = Math.Min(m_HackingLevelParamSets.Length - 1, m_HackingSuccessCount);
                        HackingDataHolder.HackingLevelParamSet = m_HackingLevelParamSets[index];
                        BattleRealEnemyManager.Instance.RequestToHacking();
                    }
                }
                break;
        }
    }

    #endregion

    /// <summary>
    /// ボスのステートを変更する。
    /// </summary>
    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    /// <summary>
    /// このボスのダウンHPを回復する。回復量は0より大きくなければ処理されない。
    /// </summary>
    public void RecoverDownHp(float recover)
    {
        if (recover <= 0)
        {
            return;
        }

        NowDownHp = Mathf.Clamp(NowDownHp + recover, 0, MaxDownHp);
        OnRecoverDownHp();
    }

    protected virtual void OnRecoverDownHp()
    {

    }

    /// <summary>
    /// このボスのダウンHPを削る。ダメージ量は0より大きくなければ処理されない。
    /// </summary>
    public void DamageDownHp(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        NowDownHp = Mathf.Clamp(NowDownHp - damage, 0, MaxDownHp);
        OnDamageDownHp();

        if (NowDownHp <= 0)
        {
            RequestChangeState(E_STATE.DOWN_BEHAVIOR);
        }
    }

    protected virtual void OnDamageDownHp()
    {

    }

    /// <summary>
    /// ハッキングモードから帰ってきた時の処理
    /// </summary>
    private void OnFromHacking()
    {
        var currentState = m_StateMachine.CurrentState.Key;
        if (currentState == E_STATE.DOWN_BEHAVIOR)
        {
            if (HackingDataHolder.IsHackingSuccess)
            {
                RequestChangeState(E_STATE.HACKING_SUCCESS);
            }
            else
            {
                RequestChangeState(E_STATE.HACKING_FAILURE);
            }
        }
    }

    protected sealed override void OnDead()
    {
        base.OnDead();
        RequestChangeState(E_STATE.DEAD);
    }
}
