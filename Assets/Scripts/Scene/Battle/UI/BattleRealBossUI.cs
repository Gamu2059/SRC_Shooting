#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// バトルリアルのボスUIを統括するクラス。
/// </summary>
public class BattleRealBossUI : ControllableMonoBehavior
{
    #region Define

    private enum E_STATE
    {
        HIDE,
        BEGIN,
        UPDATE,
    }

    private class StateCycle : StateCycleBase<BattleRealBossUI, E_STATE> { }

    private class InnerState : State<E_STATE, BattleRealBossUI>
    {
        public InnerState(E_STATE state, BattleRealBossUI target) : base(state, target) { }
        public InnerState(E_STATE state, BattleRealBossUI target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field Inspector

    [SerializeField]
    private Text m_NameText;

    [SerializeField]
    private IconGaugeIndicator m_HpGauge;

    [SerializeField]
    private IconGaugeIndicator m_DownGauge;

    [SerializeField]
    private IconCountIndicator m_HackingNum;

    [SerializeField]
    private AnimationCurve m_BeginGaugeRateCurve;

    [SerializeField]
    private float m_BeginHackDuration;

    #endregion

    #region Field

    public BattleRealBossController ReferencedBoss { get; private set; }
    private StateMachine<E_STATE, BattleRealBossUI> m_StateMachine;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_STATE, BattleRealBossUI>();
        m_StateMachine.AddState(new InnerState(E_STATE.HIDE, this, new HideState()));
        m_StateMachine.AddState(new InnerState(E_STATE.BEGIN, this, new BeginState()));
        m_StateMachine.AddState(new InnerState(E_STATE.UPDATE, this, new UpdateState()));

        m_HpGauge.OnInitialize();
        m_DownGauge.OnInitialize();
        m_HackingNum.OnInitialize();

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

        m_HackingNum.OnFinalize();
        m_DownGauge.OnFinalize();
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
        private int m_HackIconCount;

        public override void OnStart()
        {
            base.OnStart();

            var boss = Target.ReferencedBoss;
            Target.m_NameText.text = boss.BossName;
            Target.m_HpGauge.Boss = boss;
            Target.m_DownGauge.Boss = boss;
            Target.m_HackingNum.Boss = boss;

            Target.m_HpGauge.SetValue(0);
            Target.m_DownGauge.SetValue(0);
            Target.m_HackingNum.ShowCount(-1);
            Target.gameObject.SetActive(true);

            m_Duration = Target.m_BeginGaugeRateCurve.Duration();
            if (m_Duration <= 0)
            {
                Target.RequestChangeState(E_STATE.UPDATE);
                return;
            }

            var hackInterval = Target.m_BeginHackDuration;
            var hackNum = boss.HackingCompleteNum;
            if (hackNum > 0 && (m_Duration / hackNum) < hackInterval)
            {
                hackInterval = m_Duration / hackNum;
            }

            var timer = Timer.CreateIntervalTimer(E_TIMER_TYPE.SCALED_TIMER, hackInterval);
            timer.SetIntervalCallBack(() =>
            {
                Target.m_HackingNum.ShowCount(m_HackIconCount);
                m_HackIconCount++;
                if (m_HackIconCount >= hackNum)
                {
                    timer.DestroyTimer();
                }
            });
            BattleRealTimerManager.Instance.RegistTimer(timer);

            m_TimeCount = 0;
            m_HackIconCount = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var rate = Target.m_BeginGaugeRateCurve.Evaluate(m_TimeCount);
            var boss = Target.ReferencedBoss;
            var hpRate = boss.NowHp / boss.MaxHp;
            var downRate = boss.NowDownHp / boss.MaxDownHp;

            Target.m_HpGauge.SetValue(rate * hpRate);
            Target.m_DownGauge.SetValue(rate * downRate);

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
            Target.m_DownGauge.OnUpdate();
            Target.m_HackingNum.OnUpdate();
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
    public void EnableBossUI(BattleRealBossController boss)
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
