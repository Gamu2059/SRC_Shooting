using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

/// <summary>
/// バトル画面のマネージャーを管理する上位マネージャ。
/// メイン画面とコマンド画面の切り替えを主に管理する。
/// </summary>
public partial class BattleManager : ControllableMonoBehavior, IStateCallback<E_BATTLE_STATE>
{
    #region Define

    private class StateCycle : StateCycleBase<BattleManager, E_BATTLE_STATE> { }

    private class InnerState : State<E_BATTLE_STATE, BattleManager>
    {
        public InnerState(E_BATTLE_STATE state, BattleManager target) : base(state, target) { }
        public InnerState(E_BATTLE_STATE state, BattleManager target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field Inspector

    [Header("ParamSet")]

    [SerializeField]
    private BattleParamSet m_ParamSet = default;
    public BattleParamSet ParamSet => m_ParamSet;

    [Header("Video")]

    [SerializeField]
    private VideoPlayer m_VideoPlayer = default;

    [Header("GameOver")]

    [SerializeField]
    private GameOverController m_GameOverController = default;

    [Header("Debug")]

    [SerializeField]
    private bool m_IsStartHackingMode = default;

    [SerializeField]
    public bool m_PlayerNotDead = default;

    [SerializeField]
    public bool m_IsDrawColliderArea = default;

    [SerializeField]
    public bool m_IsDrawOutSideColliderArea = default;

    #endregion

    #region Field

    private StateMachine<E_BATTLE_STATE, BattleManager> m_StateMachine;
    private Action<E_BATTLE_STATE> m_OnChangeState;
    private BattleRealManager m_RealManager;
    private BattleHackingManager m_HackingManager;
    public bool IsReadyBeforeShow { get; private set; }

    #endregion

    #region Open Callback

    public Action<E_BATTLE_STATE> ChangeStateAction { get; set; }

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_STATE, BattleManager>();

        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.START, this, new StartState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.REAL_MODE, this, new RealModeState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.HACKING_MODE, this, new HackingModeState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.TO_REAL, this, new ToRealState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.TO_HACKING, this, new ToHackingState()));

        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.END, this, new EndState()));

        m_RealManager = BattleRealManager.Builder(this, m_ParamSet.BattleRealParamSet);
        m_HackingManager = BattleHackingManager.Builder(this, m_ParamSet.BattleHackingParamSet);

        m_GameOverController.OnInitialize();
        m_GameOverController.EndAction += m_RealManager.End;

        CalcPerfectHackingSuccessNum();
    }

    public override void OnFinalize()
    {
        m_OnChangeState = null;

        m_GameOverController.OnFinalize();

        m_HackingManager.OnFinalize();
        m_RealManager.OnFinalize();

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_StateMachine.OnUpdate();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            m_IsDrawColliderArea = !m_IsDrawColliderArea;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DataManager.Instance.BattleData.AddEnergyCount(1);
        }
#endif
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

    public void OnBeforeShow()
    {
        IsReadyBeforeShow = false;
        RequestChangeState(E_BATTLE_STATE.START);
    }

    public void OnAfterShow()
    {

    }

    public void OnBeforeHide()
    {
        Time.timeScale = 1;
    }

    public void OnAfterHide()
    {

    }

    /// <summary>
    /// BattleManagerのステートを変更する。
    /// </summary>
    public void RequestChangeState(E_BATTLE_STATE state)
    {
        if (m_StateMachine == null)
        {
            return;
        }

        m_StateMachine.Goto(state);
    }

    /// <summary>
    /// このステージでは最短で何回でハッキング完了になるかを計上する。
    /// </summary>
    private void CalcPerfectHackingSuccessNum()
    {
        var generator = m_ParamSet.BattleRealParamSet.EnemyManagerParamSet.Generator;
        int sum = 0;

        foreach (var group in generator.Contents)
        {
            foreach (var enemy in group.GroupGenerateParamSet.IndividualGenerateParamSets)
            {
                if (enemy.EnemyGenerateParamSet is BattleRealBossGenerateParamSet bossParamSet)
                {
                    sum += bossParamSet.HackingCompleteNum;
                }
            }
        }

        DataManager.Instance.BattleData.SetPerfectHackingSuccessCount(sum);
    }

    public void ExitGame()
    {
        BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
    }
}
