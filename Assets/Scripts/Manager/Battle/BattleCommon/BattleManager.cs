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

    [Header("StageManager")]

    [SerializeField]
    private BattleRealStageManager m_BattleRealStageManager = default;
    public BattleRealStageManager BattleRealStageManager => m_BattleRealStageManager;

    [SerializeField]
    private BattleHackingStageManager m_BattleHackingStageManager = default;
    public BattleHackingStageManager BattleHackingStageManager => m_BattleHackingStageManager;

    [Header("UiManager")]

    [SerializeField]
    private BattleRealUiManager m_BattleRealUiManager = default;
    public BattleRealUiManager BattleRealUiManager => m_BattleRealUiManager;

    [SerializeField]
    private BattleHackingUiManager m_BattleHackingUiManager = default;
    public BattleHackingUiManager BattleHackingUiManager => m_BattleHackingUiManager;

    [Header("Camera")]

    [SerializeField]
    private BattleRealCameraController m_BattleRealFrontCamera = default;
    public BattleRealCameraController BattleRealFrontCamera => m_BattleRealFrontCamera;

    [SerializeField]
    private BattleRealCameraController m_BattleRealBackCamera = default;
    public BattleRealCameraController BattleRealBackCamera => m_BattleRealBackCamera;

    [SerializeField]
    private BattleHackingCameraController m_BattleHackingFrontCamera = default;
    public BattleHackingCameraController BattleHackingFrontCamera => m_BattleHackingFrontCamera;

    [SerializeField]
    private BattleHackingCameraController m_BattleHackingBackCamera = default;
    public BattleHackingCameraController BattleHackingBackCamera => m_BattleHackingBackCamera;

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

    public BattleRealManager RealManager { get; private set; }

    public BattleHackingManager HackingManager { get; private set; }

    public bool IsReadyBeforeShow { get; private set; }

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

        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.GAME_CLEAR, this)
        {
            m_OnStart = StartOnGameClear,
            m_OnUpdate = UpdateOnGameClear,
            m_OnLateUpdate = LateUpdateOnGameClear,
            m_OnFixedUpdate = FixedUpdateOnGameClear,
            m_OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.GAME_OVER, this)
        {
            m_OnStart = StartOnGameOver,
            m_OnUpdate = UpdateOnGameOver,
            m_OnLateUpdate = LateUpdateOnGameOver,
            m_OnFixedUpdate = FixedUpdateOnGameOver,
            m_OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.END, this, new EndState()));

        BattleRealStageManager.OnInitialize();
        BattleHackingStageManager.OnInitialize();

        RealManager = new BattleRealManager(this, m_ParamSet.BattleRealParamSet);
        HackingManager = new BattleHackingManager(this, m_ParamSet.BattleHackingParamSet);

        RealManager.OnInitialize();
        HackingManager.OnInitialize();

        BattleRealUiManager.OnInitialize();
        BattleHackingUiManager.OnInitialize();

        m_GameOverController.OnInitialize();

        CalcPerfectHackingSuccessNum();
    }

    public override void OnFinalize()
    {
        m_OnChangeState = null;

        m_GameOverController.OnFinalize();

        BattleHackingUiManager.OnFinalize();
        BattleRealUiManager.OnFinalize();

        HackingManager.OnFinalize();
        RealManager.OnFinalize();

        BattleHackingStageManager.OnFinalize();
        BattleRealStageManager.OnFinalize();

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

    #region Game Clear State

    private void StartOnGameClear()
    {
        // ステージクリアした時しか記録しない
        var resultData = DataManager.Instance.BattleResultData;
        var battleData = DataManager.Instance.BattleData;
        resultData.ClacScore(battleData);

        GameManager.Instance.PlayerRecordManager.AddStoryModeRecord(new PlayerRecord("Nanashi", resultData.TotalScore, E_STAGE.NORMAL_1, DateTime.Now));

        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.GAME_CLEAR);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);
        AudioManager.Instance.StopAllBgm();
        AudioManager.Instance.StopAllSe();
        AudioManager.Instance.Play(m_ParamSet.GameClearSe);
    }

    private void UpdateOnGameClear()
    {
        RealManager.OnUpdate();
        HackingManager.OnUpdate();

        BattleRealUiManager.OnUpdate();
        BattleHackingUiManager.OnUpdate();
    }

    private void LateUpdateOnGameClear()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();

        BattleRealUiManager.OnLateUpdate();
        BattleHackingUiManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameClear()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();

        BattleRealUiManager.OnFixedUpdate();
        BattleHackingUiManager.OnFixedUpdate();
    }

    private void EndOnGameClear()
    {

    }

    #endregion

    #region Game Over State

    private void StartOnGameOver()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.GAME_OVER);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);

        AudioManager.Instance.StopAllBgm();
        AudioManager.Instance.Play(m_ParamSet.GameOverSe);
        m_GameOverController.PlayGameOver();
    }

    private void UpdateOnGameOver()
    {
        RealManager.OnUpdate();
        HackingManager.OnUpdate();

        BattleRealUiManager.OnUpdate();
        BattleHackingUiManager.OnUpdate();

        m_GameOverController.OnUpdate();
    }

    private void LateUpdateOnGameOver()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();

        BattleRealUiManager.OnLateUpdate();
        BattleHackingUiManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameOver()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();

        BattleRealUiManager.OnFixedUpdate();
        BattleHackingUiManager.OnFixedUpdate();
    }

    private void EndOnGameOver()
    {

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
    /// BattleManagerのステート変更時に呼び出すコールバックを設定する。
    /// </summary>
    public void SetChangeStateCallback(Action<E_BATTLE_STATE> callback)
    {
        m_OnChangeState += callback;
    }

    public void OnChangeState(E_BATTLE_STATE state)
    {
        m_OnChangeState?.Invoke(state);
    }

    /// <summary>
    /// このステージでは最短で何回でハッキング完了になるかを計上する。
    /// </summary>
    private void CalcPerfectHackingSuccessNum()
    {
        var generator = m_ParamSet.BattleRealParamSet.EnemyGroupManagerParamSet.Generator;
        int sum = 0;

        if (generator.BossParamSet == null)
        {
            return;
        }

        foreach (var boss in generator.BossParamSet.IndividualGenerateParamSets)
        {
            if (boss.EnemyGenerateParamSet is BattleRealBossGenerateParamSet bossParamSet)
            {
                sum += bossParamSet.HackingCompleteNum;
            }
        }

        DataManager.Instance.BattleData.SetPerfectHackingSuccessCount(sum);
    }

    /// <summary>
    /// ゲームを開始する。
    /// </summary>
    public void GameStart()
    {

    }

    public void BossBattleStart()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.BEGIN_BOSS_BATTLE);
    }

    /// <summary>
    /// ゲームオーバーにする。
    /// </summary>
	public void GameOver()
    {
        RequestChangeState(E_BATTLE_STATE.GAME_OVER);
    }

    /// <summary>
    /// 全てのインフェストラを救出することはないままゲームクリアにする。
    /// </summary>
	public void GameClearWithoutHackingComplete()
    {
        DataManager.Instance.BattleData.SetHackingComplete(false);
        RequestChangeState(E_BATTLE_STATE.GAME_CLEAR);
    }

    /// <summary>
    /// 全てのインフェストラを救出したことにしてゲームクリアにする。
    /// </summary>
    public void GameClearWithHackingComplete()
    {
        DataManager.Instance.BattleData.SetHackingComplete(true);
        RequestChangeState(E_BATTLE_STATE.GAME_CLEAR);
    }

    public void ExitGame()
    {
        BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
    }
}
