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
    public BattleRealEnemyGroupManager EnemyGroupManager { get; private set; }
    public BattleRealEnemyManager EnemyManager { get; private set; }
    public BattleRealBulletManager BulletManager { get; private set; }
    public BattleRealItemManager ItemManager { get; private set; }
    public BattleRealCollisionManager CollisionManager { get; private set; }

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

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.TRANSITION_TO_HACKING)
        {
            OnStart = StartOnTransitionToHacking,
            OnUpdate = UpdateOnTransitionToHacking,
            OnLateUpdate = LateUpdateOnTransitionToHacking,
            OnFixedUpdate = FixedUpdateOnTransitionToHacking,
            OnEnd = EndOnTransitionToHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.STAY_HACKING)
        {
            OnStart = StartOnStayHacking,
            OnUpdate = UpdateOnStayHacking,
            OnLateUpdate = LateUpdateOnStayHacking,
            OnFixedUpdate = FixedUpdateOnStayHacking,
            OnEnd = EndOnStayHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.TRANSITION_TO_REAL)
        {
            OnStart = StartOnTransitionToReal,
            OnUpdate = UpdateOnTransitionToReal,
            OnLateUpdate = LateUpdateOnTransitionToReal,
            OnFixedUpdate = FixedUpdateOnTransitionToReal,
            OnEnd = EndOnTransitionToReal,
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
        EnemyGroupManager = new BattleRealEnemyGroupManager(m_ParamSet.EnemyGroupManagerParamSet);
        EnemyManager = new BattleRealEnemyManager(m_ParamSet.EnemyManagerParamSet);
        BulletManager = new BattleRealBulletManager(m_ParamSet.BulletManagerParamSet);
        ItemManager = new BattleRealItemManager();
        CollisionManager = new BattleRealCollisionManager();

        InputManager.OnInitialize();
        RealTimerManager.OnInitialize();
        EventManager.OnInitialize();
        PlayerManager.OnInitialize();
        EnemyGroupManager.OnInitialize();
        EnemyManager.OnInitialize();
        BulletManager.OnInitialize();
        ItemManager.OnInitialize();
        CollisionManager.OnInitialize();

        m_StateMachine.Goto(E_BATTLE_REAL_STATE.START);
    }

    public override void OnFinalize()
    {
        CollisionManager.OnFinalize();
        ItemManager.OnFinalize();
        BulletManager.OnFinalize();
        EnemyManager.OnFinalize();
        EnemyGroupManager.OnFinalize();
        PlayerManager.OnFinalize();
        EventManager.OnFinalize();
        RealTimerManager.OnFinalize();
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

    public void OnRenderObject()
    {
        if (m_StateMachine == null || m_StateMachine.CurrentState == null)
        {
            return;
        }

        var state = m_StateMachine.CurrentState;
        switch (state.Key)
        {
            case E_BATTLE_REAL_STATE.GAME:
                RenderObjectOnGame();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Start State

    private void StartOnStart()
    {
        InputManager.OnStart();
        RealTimerManager.OnStart();
        EventManager.OnStart();
        PlayerManager.OnStart();
        EnemyGroupManager.OnStart();
        EnemyManager.OnStart();
        BulletManager.OnStart();
        ItemManager.OnStart();
        CollisionManager.OnStart();

        m_StateMachine.Goto(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME);
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

    #region Before Begin Game State

    private void StartOnBeforeBeginGame()
    {
        m_StateMachine.Goto(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME_PERFORMANCE);
    }

    private void UpdateOnBeforeBeginGame()
    {
    }

    private void LateUpdateOnBeforeBeginGame()
    {
    }

    private void FixedUpdateOnBeforeBeginGame()
    {
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
    }

    private void LateUpdateOnBeforeBeginGamePerformance()
    {
    }

    private void FixedUpdateOnBeforeBeginGamePerformance()
    {
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
    }

    private void LateUpdateOnBeginGame()
    {
    }

    private void FixedUpdateOnBeginGame()
    {
    }

    private void EndOnBeginGame()
    {

    }

    #endregion

    #region Game State

    private void StartOnGame()
    {
        InputManager.RegistInput();
    }

    private void UpdateOnGame()
    {
        InputManager.OnUpdate();
        RealTimerManager.OnUpdate();
        EventManager.OnUpdate();
        PlayerManager.OnUpdate();
        EnemyGroupManager.OnUpdate();
        EnemyManager.OnUpdate();
        BulletManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        RealTimerManager.OnLateUpdate();
        EventManager.OnLateUpdate();
        PlayerManager.OnLateUpdate();
        EnemyGroupManager.OnLateUpdate();
        EnemyManager.OnLateUpdate();
        BulletManager.OnLateUpdate();

        CollisionManager.UpdateCollider();
        CollisionManager.CheckCollision();

        // 消滅の更新
        EnemyGroupManager.GotoPool();
        EnemyManager.GotoPool();
        BulletManager.GotoPool();
    }

    private void FixedUpdateOnGame()
    {
        RealTimerManager.OnFixedUpdate();
        EventManager.OnFixedUpdate();
        PlayerManager.OnFixedUpdate();
        EnemyGroupManager.OnFixedUpdate();
        EnemyManager.OnFixedUpdate();
        BulletManager.OnFixedUpdate();
    }

    private void RenderObjectOnGame()
    {
        PlayerManager.OnRenderCollider();
        EnemyManager.OnRenderCollider();
        BulletManager.OnRenderCollider();
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
    }

    private void LateUpdateOnDead()
    {
    }

    private void FixedUpdateOnDead()
    {
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
    }

    private void LateUpdateOnBeforeBossBattlePerformance()
    {
    }

    private void FixedUpdateOnBeforeBossBattlePerformance()
    {
    }

    private void EndOnBeforeBossBattlePerformance()
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

    private void LateUpdateOnTransitionToHacking()
    {

    }

    private void FixedUpdateOnTransitionToHacking()
    {

    }

    private void EndOnTransitionToHacking()
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

    #region Transition To Real State

    private void StartOnTransitionToReal()
    {

    }

    private void UpdateOnTransitionToReal()
    {

    }

    private void LateUpdateOnTransitionToReal()
    {

    }

    private void FixedUpdateOnTransitionToReal()
    {

    }

    private void EndOnTransitionToReal()
    {

    }

    #endregion

    #region Before Game Clear Performance State

    private void StartOnBeforeGameClearPerformance()
    {

    }

    private void UpdateOnBeforeGameClearPerformance()
    {
    }

    private void LateUpdateOnBeforeGameClearPerformance()
    {
    }

    private void FixedUpdateOnBeforeGameClearPerformance()
    {
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

    #region End State

    private void StartOnEnd()
    {

    }

    private void UpdateOnEnd()
    {
    }

    private void LateUpdateOnEnd()
    {
    }

    private void FixedUpdateOnEnd()
    {
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
