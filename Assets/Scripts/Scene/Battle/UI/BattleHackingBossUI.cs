#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleHackingBossUI : ControllableMonoBehavior
{
    #region Define

    private enum E_STATE
    {
        HIDE,
        BEGIN,
        UPDATE,
    }

    private class StateCycle : StateCycleBase<BattleHackingBossUI, E_STATE> { }

    private class InnerState : State<E_STATE, BattleHackingBossUI>
    {
        public InnerState(E_STATE state, BattleHackingBossUI target) : base(state, target) { }
        public InnerState(E_STATE state, BattleHackingBossUI target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field Inspector

    [SerializeField]
    private GridGaugeIndicator m_HpGauge;

    [SerializeField]
    private AnimationCurve m_BeginGaugeRateCurve;

    #endregion

    #region Field

    public BattleHackingBoss ReferencedBoss { get; private set; }
    private StateMachine<E_STATE, BattleHackingBossUI> m_StateMachine;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_STATE, BattleHackingBossUI>();
        m_StateMachine.AddState(new InnerState(E_STATE.HIDE, this, new HideState()));
        m_StateMachine.AddState(new InnerState(E_STATE.BEGIN, this, new BeginState()));
        m_StateMachine.AddState(new InnerState(E_STATE.UPDATE, this, new UpdateState()));

        m_HpGauge.OnInitialize();

        RequestChangeState(E_STATE.HIDE);

        gameObject.SetActive(false);
    }

    public override void OnFinalize()
    {
        if (ReferencedBoss != null)
        {
            ReferencedBoss.FinalizeAction -= DisableBossUI;
            ReferencedBoss.HideAction -= DisableBossUI;
            ReferencedBoss = null;
        }

        m_HpGauge.OnFinalize();
        m_StateMachine.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_StateMachine.OnUpdate();
    }

    #endregion

    #region Hide State

    private class HideState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Begin State

    private class BeginState : StateCycle
    {
        private float m_Duration;
        private float m_TimeCount;

        public override void OnStart()
        {
            base.OnStart();

            var boss = Target.ReferencedBoss;
            Target.m_HpGauge.Boss = boss;

            Target.m_HpGauge.SetValue(0);
            Target.gameObject.SetActive(true);

            m_Duration = Target.m_BeginGaugeRateCurve.Duration();
            m_TimeCount = 0;
            if (m_Duration <= 0)
            {
                Target.RequestChangeState(E_STATE.UPDATE);
                return;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var rate = Target.m_BeginGaugeRateCurve.Evaluate(m_TimeCount);
            var boss = Target.ReferencedBoss;
            var hpRate = boss.NowHp / boss.MaxHp;

            Target.m_HpGauge.SetValue(rate * hpRate);

            if (m_TimeCount >= m_Duration)
            {
                Target.RequestChangeState(E_STATE.UPDATE);
            }

            m_TimeCount += Time.deltaTime;
        }
    }

    #endregion

    #region Update State

    private class UpdateState : StateCycle
    {
        public override void OnUpdate()
        {
            base.OnUpdate();

            Target.m_HpGauge.OnUpdate();
        }
    }

    #endregion

    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    /// <summary>
    /// ボスの参照を渡してUIを有効にする。
    /// </summary>
    public void EnableBossUI(BattleHackingBoss boss)
    {
        if (boss == null || ReferencedBoss != null)
        {
            return;
        }

        ReferencedBoss = boss;
        ReferencedBoss.HideAction += DisableBossUI;
        ReferencedBoss.FinalizeAction += DisableBossUI;

        RequestChangeState(E_STATE.BEGIN);
    }

    /// <summary>
    /// ボスの参照を切ってUIを無効にする。
    /// </summary>
    public void DisableBossUI()
    {
        if (ReferencedBoss != null)
        {
            ReferencedBoss.FinalizeAction -= DisableBossUI;
            ReferencedBoss.HideAction -= DisableBossUI;
            ReferencedBoss = null;
        }

        RequestChangeState(E_STATE.HIDE);
    }
}
