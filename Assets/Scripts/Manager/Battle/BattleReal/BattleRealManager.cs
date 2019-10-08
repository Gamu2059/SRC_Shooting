using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バトルのリアルモードの処理を管理する。
/// </summary>
[Serializable]
public class BattleRealManager : ControllableObject
{
    #region Field

    private BattleRealParamSet m_ParamSet;

    private StateMachine<E_BATTLE_REAL_STATE> m_StateMachine;

    public BattleRealInputManager InputManager { get; private set; }
    public BattleRealTimerManager RealTimerManager { get; private set; }
    public BattleRealEventManager EventManager { get; private set; }
    public BattleRealPlayerManager PlayerManager { get; private set; }
    public BattleRealEnemyManager EnemyManager { get; private set; }
    public BattleRealBulletManager BulletManager { get; private set; }
    public BattleRealItemManager ItemManager { get; private set; }

    #endregion

    public static BattleRealManager Instance => BattleManager.Instance.RealManager;

    public BattleRealManager(BattleRealParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_REAL_STATE>();

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.START)
        {
            OnStart = StartOnStart,
            OnUpdate = UpdateOnStart,
            OnLateUpdate = LateUpdateOnStart,
            OnFixedUpdate = FixedUpdateOnStart,
            OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME)
        {
            OnStart = StartOnBeforeBeginGame,
            OnUpdate = UpdateOnBeforeBeginGame,
            OnLateUpdate = LateUpdateOnBeforeBeginGame,
            OnFixedUpdate = FixedUpdateOnBeforeBeginGame,
            OnEnd = EndOnBeforeBeginGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME_PERFORMANCE)
        {
            OnStart = StartOnBeforeBeginGamePerformance,
            OnUpdate = UpdateOnBeforeBeginGamePerformance,
            OnLateUpdate = LateUpdateOnBeforeBeginGamePerformance,
            OnFixedUpdate = FixedUpdateOnBeforeBeginGamePerformance,
            OnEnd = EndOnBeforeBeginGamePerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEGIN_GAME)
        {
            OnStart = StartOnBeginGame,
            OnUpdate = UpdateOnBeginGame,
            OnLateUpdate = LateUpdateOnBeginGame,
            OnFixedUpdate = FixedUpdateOnBeginGame,
            OnEnd = EndOnBeginGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME)
        {
            OnStart = StartOnGame,
            OnUpdate = UpdateOnGame,
            OnLateUpdate = LateUpdateOnGame,
            OnFixedUpdate = FixedUpdateOnGame,
            OnEnd = EndOnGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.DEAD)
        {
            OnStart = StartOnDead,
            OnUpdate = UpdateOnDead,
            OnLateUpdate = LateUpdateOnDead,
            OnFixedUpdate = FixedUpdateOnDead,
            OnEnd = EndOnDead,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BOSS_BATTLE_PERFORMANCE)
        {
            OnStart = StartOnBeforeBossBattlePerformance,
            OnUpdate = UpdateOnBeforeBossBattlePerformance,
            OnLateUpdate = LateUpdateOnBeforeBossBattlePerformance,
            OnFixedUpdate = FixedUpdateOnBeforeBossBattlePerformance,
            OnEnd = EndOnBeforeBossBattlePerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.STAY_HACKING)
        {
            OnStart = StartOnStayHacking,
            OnUpdate = UpdateOnStayHacking,
            OnLateUpdate = LateUpdateOnStayHacking,
            OnFixedUpdate = FixedUpdateOnStayHacking,
            OnEnd = EndOnStayHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_GAME_CLEAR_PERFORMANCE)
        {
            OnStart = StartOnBeforeGameClearPerformance,
            OnUpdate = UpdateOnBeforeGameClearPerformance,
            OnLateUpdate = LateUpdateOnBeforeGameClearPerformance,
            OnFixedUpdate = FixedUpdateOnBeforeGameClearPerformance,
            OnEnd = EndOnBeforeGameClearPerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME_CLEAR)
        {
            OnStart = StartOnGameClear,
            OnUpdate = UpdateOnGameClear,
            OnLateUpdate = LateUpdateOnGameClear,
            OnFixedUpdate = FixedUpdateOnGameClear,
            OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME_OVER)
        {
            OnStart = StartOnGameOver,
            OnUpdate = UpdateOnGameOver,
            OnLateUpdate = LateUpdateOnGameOver,
            OnFixedUpdate = FixedUpdateOnGameOver,
            OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.END)
        {
            OnStart = StartOnEnd,
            OnUpdate = UpdateOnEnd,
            OnLateUpdate = LateUpdateOnEnd,
            OnFixedUpdate = FixedUpdateOnEnd,
            OnEnd = EndOnEnd,
        });

        InputManager = new BattleRealInputManager();
        RealTimerManager = new BattleRealTimerManager();
        EventManager = new BattleRealEventManager(m_ParamSet.EventTriggerParamSet);
        PlayerManager = new BattleRealPlayerManager(m_ParamSet.PlayerManagerParamSet);
        EnemyManager = new BattleRealEnemyManager(m_ParamSet.EnemyManagerParamSet);
        BulletManager = new BattleRealBulletManager(m_ParamSet.BulletManagerParamSet);
        ItemManager = new BattleRealItemManager();

        InputManager.OnInitialize();
        RealTimerManager.OnInitialize();
        EventManager.OnInitialize();
        PlayerManager.OnInitialize();
        EnemyManager.OnInitialize();
        BulletManager.OnInitialize();
        ItemManager.OnInitialize();

        m_StateMachine.Goto(E_BATTLE_REAL_STATE.START);
    }

    public override void OnFinalize()
    {
        ItemManager.OnFinalize();
        BulletManager.OnFinalize();
        EnemyManager.OnFinalize();
        PlayerManager.OnFinalize();
        EventManager.OnFinalize();
        RealTimerManager.OnFinalize();
        InputManager.OnFinalize();

        m_StateMachine.OnFinalize();

        base.OnFinalize();
    }

    public void OnEnableObject()
    {
        //TimerManager.OnEnableObject();
        //EventManager.OnEnableObject();
        //PlayerManager.OnEnableObject();
        //EnemyManager.OnEnableObject();
        //BulletManager.OnEnableObject();
        //ItemManager.OnEnableObject();
    }

    public void OnDisableObject()
    {
        //ItemManager.OnDisableObject();
        //BulletManager.OnDisableObject();
        //EnemyManager.OnDisableObject();
        //PlayerManager.OnDisableObject();
        //EventManager.OnDisableObject();
        //TimerManager.OnDisableObject();
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
        RealTimerManager.OnStart();
        EventManager.OnStart();
        PlayerManager.OnStart();
        EnemyManager.OnStart();
        BulletManager.OnStart();
        ItemManager.OnStart();

        m_StateMachine.Goto(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME);
    }

    private void UpdateOnStart()
    {
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnStart()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnStart()
    {
        RealTimerManager.OnFixedUpdate();
    }

    private void EndOnStart()
    {

    }

    #endregion

    #region Before Begin Game State

    private void StartOnBeforeBeginGame()
    {
        m_StateMachine.Goto(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME_PERFORMANCE);
    }

    private void UpdateOnBeforeBeginGame()
    {
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnBeforeBeginGame()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnBeforeBeginGame()
    {
        RealTimerManager.OnFixedUpdate();
    }

    private void EndOnBeforeBeginGame()
    {

    }

    #endregion

    #region Before Begin Game Performance State

    private void StartOnBeforeBeginGamePerformance()
    {
        m_StateMachine.Goto(E_BATTLE_REAL_STATE.BEGIN_GAME);
    }

    private void UpdateOnBeforeBeginGamePerformance()
    {
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnBeforeBeginGamePerformance()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnBeforeBeginGamePerformance()
    {
        RealTimerManager.OnFixedUpdate();
    }

    private void EndOnBeforeBeginGamePerformance()
    {

    }

    #endregion

    #region Begin Game State

    private void StartOnBeginGame()
    {
        m_StateMachine.Goto(E_BATTLE_REAL_STATE.GAME);
    }

    private void UpdateOnBeginGame()
    {
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnBeginGame()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnBeginGame()
    {
        RealTimerManager.OnFixedUpdate();
    }

    private void EndOnBeginGame()
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
        RealTimerManager.OnUpdate();
        PlayerManager.OnUpdate();

        BulletManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        RealTimerManager.OnLateUpdate();
        PlayerManager.OnLateUpdate();

        BulletManager.OnLateUpdate();
    }

    private void FixedUpdateOnGame()
    {
        RealTimerManager.OnFixedUpdate();
        PlayerManager.OnFixedUpdate();

        BulletManager.OnFixedUpdate();
    }

    private void EndOnGame()
    {
        InputManager.RemoveInput();
    }

    #endregion

    #region Dead State

    private void StartOnDead()
    {

    }

    private void UpdateOnDead()
    {
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnDead()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnDead()
    {
        RealTimerManager.OnFixedUpdate();
    }

    private void EndOnDead()
    {

    }

    #endregion

    #region Before Boss Battle Performance State

    private void StartOnBeforeBossBattlePerformance()
    {

    }

    private void UpdateOnBeforeBossBattlePerformance()
    {
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnBeforeBossBattlePerformance()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnBeforeBossBattlePerformance()
    {
        RealTimerManager.OnFixedUpdate();
    }

    private void EndOnBeforeBossBattlePerformance()
    {

    }

    #endregion

    #region Stay Hacking State

    private void StartOnStayHacking()
    {

    }

    private void UpdateOnStayHacking()
    {
    }

    private void LateUpdateOnStayHacking()
    {

    }

    private void FixedUpdateOnStayHacking()
    {

    }

    private void EndOnStayHacking()
    {

    }

    #endregion

    #region Before Game Clear Performance State

    private void StartOnBeforeGameClearPerformance()
    {

    }

    private void UpdateOnBeforeGameClearPerformance()
    {
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnBeforeGameClearPerformance()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnBeforeGameClearPerformance()
    {
        RealTimerManager.OnFixedUpdate();
    }

    private void EndOnBeforeGameClearPerformance()
    {

    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {

    }

    private void UpdateOnGameClear()
    {
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnGameClear()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameClear()
    {
        RealTimerManager.OnFixedUpdate();
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
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnGameOver()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameOver()
    {
        RealTimerManager.OnFixedUpdate();
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
        RealTimerManager.OnUpdate();
    }

    private void LateUpdateOnEnd()
    {
        RealTimerManager.OnLateUpdate();
    }

    private void FixedUpdateOnEnd()
    {
        RealTimerManager.OnFixedUpdate();
    }

    private void EndOnEnd()
    {

    }

    #endregion

    public void RequestChangeState(E_BATTLE_REAL_STATE state)
    {
        if (m_StateMachine == null)
        {
            return;
        }

        m_StateMachine.Goto(state);
    }
}
