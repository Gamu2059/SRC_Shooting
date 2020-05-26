using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Doozy.Engine;

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

    [SerializeField, Tooltip("緊急で用意した難易度設定パラメータ 本番使用はしません")]
    private E_DIFFICULTY m_Difficulty = E_DIFFICULTY.NORMAL;

    [SerializeField, Tooltip("緊急で用意したチャプターデータ 本番使用はしません")]
    private E_CHAPTER m_Chapter = E_CHAPTER.CHAPTER_0;

    [Header("ParamSet")]

    [SerializeField, Tooltip("本番使用はしません")]
    private BattleParamSet m_ParamSet = default;
    public BattleParamSet ParamSet => m_ParamSet;

    [Header("Hacking In Out")]

    [SerializeField]
    private HackInOutController m_HackInController;

    [SerializeField]
    private HackInOutController m_HackOutController;

    [Header("DoozyUI GameEvent Key")]

    [SerializeField]
    private string m_OpenGameMenuKey = "Open InGameMenu";

    #endregion

    #region Field

    private StateMachine<E_BATTLE_STATE, BattleManager> m_StateMachine;
    private BattleRealManager m_RealManager;
    private BattleHackingManager m_HackingManager;
    public bool IsReadyBeforeShow { get; private set; }

    /// <summary>
    /// ゲームメニューを開いているかどうか。<br>
    /// メニューはゲームステートを超越したステートであるため、専用のフラグで管理する
    /// </summary>
    private bool m_IsOpenGameMenu;

    #endregion

    #region Open Callback

    public Action<E_BATTLE_STATE> ChangeStateAction { get; set; }

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        AudioManager.Instance.ResetAisac();

        var paramSet = m_ParamSet;
        if (!DataManager.Instance.IsSelectedGame)
        {
            DataManager.Instance.GameMode = E_GAME_MODE.CHAPTER;
            DataManager.Instance.Chapter = m_Chapter;
            DataManager.Instance.Difficulty = m_Difficulty;
        }
        else
        {
            paramSet = DataManager.Instance.GetCurrentBattleParamSet();
        }

        DataManager.Instance.OnChapterStart();

        m_StateMachine = new StateMachine<E_BATTLE_STATE, BattleManager>();

        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.START, this, new StartState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.REAL_MODE, this, new RealModeState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.HACKING_MODE, this, new HackingModeState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.TO_REAL, this, new ToRealState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.TO_HACKING, this, new ToHackingState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_STATE.END, this, new EndState()));

        m_RealManager = BattleRealManager.Builder(this, paramSet.BattleRealParamSet);
        m_HackingManager = BattleHackingManager.Builder(this, paramSet.BattleHackingParamSet);

        m_HackInController.OnInitialize();
        m_HackOutController.OnInitialize();

        m_IsOpenGameMenu = false;
    }

    public override void OnFinalize()
    {
        m_HackOutController.OnFinalize();
        m_HackInController.OnFinalize();

        m_HackingManager.OnFinalize();
        m_RealManager.OnFinalize();

        AudioManager.Instance.ResetAisac();

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        if (RewiredInputManager.Instance != null)
        {
            if (RewiredInputManager.Instance.OpenMenu == E_REWIRED_INPUT_STATE.DOWN)
            {
                OpenGameMenu();
            }
        }

        if (m_IsOpenGameMenu)
        {
            return;
        }

        m_StateMachine.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (m_IsOpenGameMenu)
        {
            return;
        }

        m_StateMachine.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (m_IsOpenGameMenu)
        {
            return;
        }

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
        //RewiredInputManager.Instance.ChangeToInGameInput();
    }

    public void OnBeforeHide()
    {
        //RewiredInputManager.Instance.ChangeToUIInput();
        Time.timeScale = 1;
    }

    public void OnAfterHide()
    {
        AudioManager.Instance.StopAllBgm();
        AudioManager.Instance.StopAllSe();
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

    public void ExitGame()
    {
        BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
    }

    /// <summary>
    /// ゲームメニューを開く
    /// </summary>
    public void OpenGameMenu()
    {
        if (m_IsOpenGameMenu)
        {
            return;
        }

        m_IsOpenGameMenu = true;
        GameEventMessage.SendEvent(m_OpenGameMenuKey);
        AudioManager.Instance.Play(E_COMMON_SOUND.INGAME_MENU_OPEN);
        //RewiredInputManager.Instance.ChangeToUIInput();
    }

    /// <summary>
    /// ゲームメニューを閉じる
    /// </summary>
    public void CloseGameMenu()
    {
        //RewiredInputManager.Instance.ChangeToInGameInput();
        m_IsOpenGameMenu = false;
        AudioManager.Instance.Play(E_COMMON_SOUND.INGAME_MENU_CLOSE);
    }
}
