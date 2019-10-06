using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルのハッキングモードの処理を管理する。
/// </summary>
public class BattleHackingManager : ControllableObject
{
    #region Field

    private BattleHackingParamSet m_ParamSet;

    private StateMachine<E_BATTLE_HACKING_STATE> m_StateMachine;

    public BattleHackingInputManager InputManager { get; private set; }

    public BattleHackingTimerManager HackingTimerManager { get; private set; }



    public BattleHackingPlayerManager PlayerManager { get; private set; }

    #endregion

    public static BattleHackingManager Instance => BattleManager.Instance.HackingManager;

    public BattleHackingManager(BattleHackingParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycyle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_HACKING_STATE>();

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.START)
        {
            OnStart = StartOnStart,
            OnUpdate = UpdateOnStart,
            OnLateUpdate = LateUpdateOnStart,
            OnFixedUpdate = FixedUpdateOnStart,
            OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.STAY_REAL)
        {
            OnStart = StartOnStayReal,
            OnUpdate = UpdateOnStayReal,
            OnLateUpdate = LateUpdateOnStayReal,
            OnFixedUpdate = FixedUpdateOnStayReal,
            OnEnd = EndOnStayReal,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.PREPARE_GAME)
        {
            OnStart = StartOnPrepareGame,
            OnUpdate = UpdateOnPrepareGame,
            OnLateUpdate = LateUpdateOnPrepareGame,
            OnFixedUpdate = FixedUpdateOnPrepareGame,
            OnEnd = EndOnPrepareGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.GAME)
        {
            OnStart = StartOnGame,
            OnUpdate = UpdateOnGame,
            OnLateUpdate = LateUpdateOnGame,
            OnFixedUpdate = FixedUpdateOnGame,
            OnEnd = EndOnGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.GAME_CLEAR)
        {
            OnStart = StartOnGameClear,
            OnUpdate = UpdateOnGameClear,
            OnLateUpdate = LateUpdateOnGameClear,
            OnFixedUpdate = FixedUpdateOnGameClear,
            OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.GAME_OVER)
        {
            OnStart = StartOnGameOver,
            OnUpdate = UpdateOnGameOver,
            OnLateUpdate = LateUpdateOnGameOver,
            OnFixedUpdate = FixedUpdateOnGameOver,
            OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.END_GAME)
        {
            OnStart = StartOnEndGame,
            OnUpdate = UpdateOnEndGame,
            OnLateUpdate = LateUpdateOnEndGame,
            OnFixedUpdate = FixedUpdateOnEndGame,
            OnEnd = EndOnEndGame,
        });

        InputManager = new BattleHackingInputManager();
        PlayerManager = new BattleHackingPlayerManager(m_ParamSet.PlayerManagerParamSet);

        InputManager.OnInitialize();
        PlayerManager.OnInitialize();

        RequestChangeState(E_BATTLE_HACKING_STATE.START);
    }

    public override void OnFinalize()
    {
        PlayerManager.OnFinalize();
        InputManager.OnFinalize();
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

    #region Start State

    private void StartOnStart()
    {
        InputManager.OnStart();
        PlayerManager.OnStart();

        RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);
    }

    private void UpdateOnStart()
    {

    }

    private void LateUpdateOnStart()
    {

    }

    private void FixedUpdateOnStart()
    {

    }

    private void EndOnStart()
    {

    }

    #endregion

    #region Stay Real State

    private void StartOnStayReal()
    {

    }

    private void UpdateOnStayReal()
    {

    }

    private void LateUpdateOnStayReal()
    {

    }

    private void FixedUpdateOnStayReal()
    {

    }

    private void EndOnStayReal()
    {

    }

    #endregion

    #region Prepare Game State

    private void StartOnPrepareGame()
    {
        PlayerManager.OnPrepare();
    }

    private void UpdateOnPrepareGame()
    {

    }

    private void LateUpdateOnPrepareGame()
    {

    }

    private void FixedUpdateOnPrepareGame()
    {

    }

    private void EndOnPrepareGame()
    {

    }

    #endregion

    #region Game State

    private void StartOnGame()
    {
        InputManager.RegisterInput();
    }

    private void UpdateOnGame()
    {
        InputManager.OnUpdate();
        PlayerManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        PlayerManager.OnLateUpdate();
    }

    private void FixedUpdateOnGame()
    {
        PlayerManager.OnFixedUpdate();
    }

    private void EndOnGame()
    {
        InputManager.RemoveInput();
    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {
        RequestChangeState(E_BATTLE_HACKING_STATE.END_GAME);
    }

    private void UpdateOnGameClear()
    {

    }

    private void LateUpdateOnGameClear()
    {

    }

    private void FixedUpdateOnGameClear()
    {

    }

    private void EndOnGameClear()
    {

    }

    #endregion

    #region Game Over State

    private void StartOnGameOver()
    {
        RequestChangeState(E_BATTLE_HACKING_STATE.END_GAME);
    }

    private void UpdateOnGameOver()
    {

    }

    private void LateUpdateOnGameOver()
    {

    }

    private void FixedUpdateOnGameOver()
    {

    }

    private void EndOnGameOver()
    {

    }

    #endregion

    #region End Game State

    private void StartOnEndGame()
    {
        PlayerManager.OnPutAway();
        BattleManager.Instance.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_REAL);
    }

    private void UpdateOnEndGame()
    {

    }

    private void LateUpdateOnEndGame()
    {

    }

    private void FixedUpdateOnEndGame()
    {

    }

    private void EndOnEndGame()
    {

    }

    #endregion

    public void RequestChangeState(E_BATTLE_HACKING_STATE state)
    {
        if (m_StateMachine == null)
        {
            return;
        }

        m_StateMachine.Goto(state);
    }
}
