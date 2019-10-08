using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バトルのハッキングモードの処理を管理する。
/// </summary>
[Serializable]
public class BattleHackingManager : ControllableObject
{
    #region Field

    private BattleHackingParamSet m_ParamSet;

    private StateMachine<E_BATTLE_HACKING_STATE> m_StateMachine;

    public BattleHackingInputManager InputManager { get; private set; }
    public BattleHackingTimerManager HackingTimerManager { get; private set; }
    public BattleHackingPlayerManager PlayerManager { get; private set; }
    public BattleHackingEnemyManager EnemyManager { get; private set; }
    public BattleHackingBulletManager BulletManager { get; private set; }

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
        HackingTimerManager = new BattleHackingTimerManager();
        PlayerManager = new BattleHackingPlayerManager(m_ParamSet.PlayerManagerParamSet);
        EnemyManager = new BattleHackingEnemyManager();
        BulletManager = new BattleHackingBulletManager(m_ParamSet.BulletManagerParamSet);

        InputManager.OnInitialize();
        HackingTimerManager.OnInitialize();
        PlayerManager.OnInitialize();
        EnemyManager.OnInitialize();
        BulletManager.OnInitialize();

        RequestChangeState(E_BATTLE_HACKING_STATE.START);
    }

    public override void OnFinalize()
    {
        BulletManager.OnFinalize();
        EnemyManager.OnFinalize();
        PlayerManager.OnFinalize();
        HackingTimerManager.OnFinalize();
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
        HackingTimerManager.OnStart();
        PlayerManager.OnStart();
        EnemyManager.OnStart();
        BulletManager.OnStart();

        RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);
    }

    private void UpdateOnStart()
    {
        HackingTimerManager.OnUpdate();
    }

    private void LateUpdateOnStart()
    {
        HackingTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnStart()
    {
        HackingTimerManager.OnFixedUpdate();
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
        HackingTimerManager.OnUpdate();
        PlayerManager.OnUpdate();
        BulletManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        HackingTimerManager.OnLateUpdate();
        PlayerManager.OnLateUpdate();
        BulletManager.OnLateUpdate();
    }

    private void FixedUpdateOnGame()
    {
        HackingTimerManager.OnFixedUpdate();
        PlayerManager.OnFixedUpdate();
        BulletManager.OnFixedUpdate();
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
        HackingTimerManager.OnUpdate();
    }

    private void LateUpdateOnGameClear()
    {
        HackingTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameClear()
    {
        HackingTimerManager.OnFixedUpdate();
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
        HackingTimerManager.OnUpdate();
    }

    private void LateUpdateOnGameOver()
    {
        HackingTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameOver()
    {
        HackingTimerManager.OnFixedUpdate();
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
        HackingTimerManager.OnUpdate();
    }

    private void LateUpdateOnEndGame()
    {
        HackingTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnEndGame()
    {
        HackingTimerManager.OnFixedUpdate();
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
