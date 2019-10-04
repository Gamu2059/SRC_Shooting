using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バトル画面のマネージャーを管理する上位マネージャ。
/// メイン画面とコマンド画面の切り替えを主に管理する。
/// </summary>
public class BattleManager : SingletonMonoBehavior<BattleManager>
{
    public enum E_BATTLE_STATE
    {
        START,

        REAL_MODE,

        HACKING_MODE,

        TRANSITION_TO_REAL,

        TRANSITION_TO_HACKING,

        GAME_CLEAR,

        GAME_OVER,

        END,
    }

    #region Field Inspector

    [SerializeField]
    private BattleParamSet m_BattleParamSet;

    [SerializeField]
    private BattleRealStageManager m_BattleRealStageManager;
    public BattleRealStageManager BattleRealStageManager => m_BattleRealStageManager;

    [SerializeField]
    private bool m_IsStartHackingMode;

    [SerializeField]
    public bool m_PlayerNotDead;

    #endregion

    #region Field

    private StateMachine<E_BATTLE_STATE> m_StateMachine;

    public BattleRealManager RealManager { get; private set; }
    public BattleHackingManager HackingManager { get; private set; }

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_STATE>();

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.START)
        {
            OnStart = StartOnStart,
            OnUpdate = UpdateOnStart,
            OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.REAL_MODE)
        {
            OnStart = StartOnRealMode,
            OnUpdate = UpdateOnRealMode,
            OnEnd = EndOnRealMode,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.HACKING_MODE)
        {
            OnStart = StartOnHackingMode,
            OnUpdate = UpdateOnHackingMode,
            OnEnd = EndOnHackingMode,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.TRANSITION_TO_REAL)
        {
            OnStart = StartOnTransitionToReal,
            OnUpdate = UpdateOnTransitionToReal,
            OnEnd = EndOnTransitionToReal,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.TRANSITION_TO_HACKING)
        {
            OnStart = StartOnTransitionToHacking,
            OnUpdate = UpdateOnTransitionToHacking,
            OnEnd = EndOnTransitionToHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.GAME_CLEAR)
        {
            OnStart = StartOnGameClear,
            OnUpdate = UpdateOnGameClear,
            OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.GAME_OVER)
        {
            OnStart = StartOnGameOver,
            OnUpdate = UpdateOnGameOver,
            OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.END)
        {
            OnStart = StartOnEnd,
            OnUpdate = UpdateOnEnd,
            OnEnd = EndOnEnd,
        });

        m_BattleRealStageManager.OnInitialize();

        RealManager = new BattleRealManager(m_BattleParamSet.BattleRealParamSet);
        HackingManager = new BattleHackingManager(m_BattleParamSet.BattleHackingParamSet);

        RealManager.OnInitialize();
        HackingManager.OnInitialize();

        m_StateMachine.Goto(E_BATTLE_STATE.START);
    }

    public override void OnFinalize()
    {
        HackingManager.OnFinalize();
        RealManager.OnFinalize();

        m_BattleRealStageManager.OnFinalize();

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

    #region Start State

    private void StartOnStart()
    {
        RealManager.OnStart();
        HackingManager.OnStart();

        if (m_IsStartHackingMode)
        {
            m_StateMachine.Goto(E_BATTLE_STATE.TRANSITION_TO_HACKING);
        } else
        {
            m_StateMachine.Goto(E_BATTLE_STATE.REAL_MODE);
        }
    }

    private void UpdateOnStart()
    {

    }

    private void EndOnStart()
    {

    }

    #endregion

    #region Real Mode State

    private void StartOnRealMode()
    {

    }

    private void UpdateOnRealMode()
    {
        RealManager.OnUpdate();
    }

    private void EndOnRealMode()
    {

    }

    #endregion

    #region Hacking Mode State

    private void StartOnHackingMode()
    {

    }

    private void UpdateOnHackingMode()
    {

    }

    private void EndOnHackingMode()
    {

    }

    #endregion

    #region Transition To Hacking State

    private void StartOnTransitionToHacking()
    {

    }

    private void UpdateOnTransitionToHacking()
    {

    }

    private void EndOnTransitionToHacking()
    {

    }

    #endregion

    #region Transition To Real State

    private void StartOnTransitionToReal()
    {

    }

    private void UpdateOnTransitionToReal()
    {

    }

    private void EndOnTransitionToReal()
    {

    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {

    }

    private void UpdateOnGameClear()
    {

    }

    private void EndOnGameClear()
    {

    }

    #endregion

    #region Game Over State

    private void StartOnGameOver()
    {

    }

    private void UpdateOnGameOver()
    {

    }

    private void EndOnGameOver()
    {

    }

    #endregion

    #region End State

    private void StartOnEnd()
    {

    }

    private void UpdateOnEnd()
    {

    }

    private void EndOnEnd()
    {

    }

    #endregion

    /// <summary>
    /// ゲームを開始する。
    /// </summary>
    public void GameStart()
    {

    }

    /// <summary>
    /// ゲームオーバーにする。
    /// </summary>
	public void GameOver()
    {
        //DetachBattleMainInputAction();
        //BattleMainUiManager.Instance.ShowGameOver();
        //BattleMainAudioManager.Instance.StopAllBGM();
        //var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        //{
        //    BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
        //});
        //TimerManager.Instance.RegistTimer(timer);
    }

    /// <summary>
    /// ゲームクリアにする。
    /// </summary>
	public void GameClear()
    {
        //DetachBattleMainInputAction();

        //BattleMainUiManager.Instance.ShowGameClear();
        //var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        //{
        //    BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
        //});
        //TimerManager.Instance.RegistTimer(timer);
    }
}
