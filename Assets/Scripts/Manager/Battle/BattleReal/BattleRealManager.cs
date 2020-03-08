using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バトルのリアルモードの処理を管理する。
/// </summary>
[Serializable]
public partial class BattleRealManager : ControllableObject
{
    #region Define

    private class BattleRealManagerState : State<E_BATTLE_REAL_STATE, BattleRealManager>
    {
        public BattleRealManagerState(E_BATTLE_REAL_STATE state) : base(state) { }
        public BattleRealManagerState(E_BATTLE_REAL_STATE state, StateCycleBase<BattleRealManager> cycle) : base(state, cycle) { }
    }

    #endregion

    public static BattleRealManager Instance { get; private set; }

    #region Field

    private BattleManager m_BattleManager;
    private BattleRealParamSet m_ParamSet;

    private StateMachine<E_BATTLE_REAL_STATE, BattleRealManager> m_StateMachine;

    public BattleRealInputManager InputManager { get; private set; }
    public BattleRealTimerManager RealTimerManager { get; private set; }
    public BattleRealEventManager EventManager { get; private set; }
    public BattleRealPlayerManager PlayerManager { get; private set; }
    public BattleRealEnemyGroupManager EnemyGroupManager { get; private set; }
    public BattleRealEnemyManager EnemyManager { get; private set; }
    public BattleRealBulletManager BulletManager { get; private set; }
    public BattleRealItemManager ItemManager { get; private set; }
    public BattleRealEffectManager EffectManager { get; private set; }
    public BattleRealCollisionManager CollisionManager { get; private set; }
    public BattleRealCameraManager CameraManager { get; private set; }

    private bool m_IsPlayerDead;

    public Action OnTransitionToHacking;
    public Action OnTransitionToReal;

    #endregion

    public BattleRealManager(BattleManager battleManager, BattleRealParamSet paramSet)
    {
        m_BattleManager = battleManager;
        m_ParamSet = paramSet;
        Instance = this;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_REAL_STATE, BattleRealManager>();

        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.START, new StartState(this)));
        //m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME)
        //{
        //    m_OnStart = StartOnBeforeBeginGame,
        //    m_OnUpdate = UpdateOnBeforeBeginGame,
        //    m_OnLateUpdate = LateUpdateOnBeforeBeginGame,
        //    m_OnFixedUpdate = FixedUpdateOnBeforeBeginGame,
        //    m_OnEnd = EndOnBeforeBeginGame,
        //});

        //m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME_PERFORMANCE)
        //{
        //    m_OnStart = StartOnBeforeBeginGamePerformance,
        //    m_OnUpdate = UpdateOnBeforeBeginGamePerformance,
        //    m_OnLateUpdate = LateUpdateOnBeforeBeginGamePerformance,
        //    m_OnFixedUpdate = FixedUpdateOnBeforeBeginGamePerformance,
        //    m_OnEnd = EndOnBeforeBeginGamePerformance,
        //});

        //m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.BEGIN_GAME)
        //{
        //    m_OnStart = StartOnBeginGame,
        //    m_OnUpdate = UpdateOnBeginGame,
        //    m_OnLateUpdate = LateUpdateOnBeginGame,
        //    m_OnFixedUpdate = FixedUpdateOnBeginGame,
        //    m_OnEnd = EndOnBeginGame,
        //});

        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.GAME, new GameState(this)));
        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.DEAD, new DeadState(this)));
        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.CHARGE_SHOT, new ChargeShotState(this)));

        //m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.BEFORE_BOSS_BATTLE_PERFORMANCE)
        //{
        //    m_OnStart = StartOnBeforeBossBattlePerformance,
        //    m_OnUpdate = UpdateOnBeforeBossBattlePerformance,
        //    m_OnLateUpdate = LateUpdateOnBeforeBossBattlePerformance,
        //    m_OnFixedUpdate = FixedUpdateOnBeforeBossBattlePerformance,
        //    m_OnEnd = EndOnBeforeBossBattlePerformance,
        //});

        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.TO_HACKING, new ToHackingState(this)));
        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.STAY_HACKING, new StayHackingState(this)));
        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.FROM_HACKING, new FromHackingState(this)));

        //m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.BEFORE_GAME_CLEAR_PERFORMANCE)
        //{
        //    m_OnStart = StartOnBeforeGameClearPerformance,
        //    m_OnUpdate = UpdateOnBeforeGameClearPerformance,
        //    m_OnLateUpdate = LateUpdateOnBeforeGameClearPerformance,
        //    m_OnFixedUpdate = FixedUpdateOnBeforeGameClearPerformance,
        //    m_OnEnd = EndOnBeforeGameClearPerformance,
        //});

        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.BEGIN_BOSS_BATTLE)
        {
            m_OnStart = StartOnBeginBossBattle,
            m_OnUpdate = UpdateOnBeginBossBattle,
            m_OnLateUpdate = LateUpdateOnBeginBossBattle,
            m_OnFixedUpdate = FixedUpdateOnBeginBossBattle,
            m_OnEnd = EndOnBeginBossBattle,
        });

        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.GAME_CLEAR, new GameClearState(this)));
        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.GAME_OVER, new GameOverState(this)));
        m_StateMachine.AddState(new BattleRealManagerState(E_BATTLE_REAL_STATE.END, new EndState(this)));

        InputManager = new BattleRealInputManager();
        RealTimerManager = new BattleRealTimerManager();
        EventManager = new BattleRealEventManager(this, m_ParamSet.EventTriggerParamSet);
        PlayerManager = new BattleRealPlayerManager(m_ParamSet.PlayerManagerParamSet);
        EnemyGroupManager = new BattleRealEnemyGroupManager(m_ParamSet.EnemyGroupManagerParamSet);
        EnemyManager = new BattleRealEnemyManager(m_ParamSet.EnemyManagerParamSet);
        BulletManager = new BattleRealBulletManager(m_ParamSet.BulletManagerParamSet);
        ItemManager = new BattleRealItemManager(m_ParamSet.ItemManagerParamSet);
        EffectManager = new BattleRealEffectManager();
        CollisionManager = new BattleRealCollisionManager(m_BattleManager.ParamSet.ColliderMaterial);
        CameraManager = new BattleRealCameraManager();

        InputManager.OnInitialize();
        RealTimerManager.OnInitialize();
        EventManager.OnInitialize();
        PlayerManager.OnInitialize();
        EnemyGroupManager.OnInitialize();
        EnemyManager.OnInitialize();
        BulletManager.OnInitialize();
        ItemManager.OnInitialize();
        EffectManager.OnInitialize();
        CollisionManager.OnInitialize();
        CameraManager.OnInitialize();

        m_StateMachine.Goto(E_BATTLE_REAL_STATE.START);
    }

    public override void OnFinalize()
    {
        CameraManager.OnFinalize();
        CollisionManager.OnFinalize();
        EffectManager.OnFinalize();
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

        if (InputManager.Menu == E_INPUT_STATE.DOWN)
        {
            m_BattleManager.ExitGame();
        }
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

    #region Begin Boss Battle State

    private void StartOnBeginBossBattle()
    {
        EnemyGroupManager.CreateBossGroup();
        m_BattleManager.BattleRealUiManager.SetEnableBossUI(true);
        RequestChangeState(E_BATTLE_REAL_STATE.GAME);
    }

    private void UpdateOnBeginBossBattle()
    {

    }

    private void LateUpdateOnBeginBossBattle()
    {

    }

    private void FixedUpdateOnBeginBossBattle()
    {

    }

    private void EndOnBeginBossBattle()
    {

    }

    #endregion

    /// <summary>
    /// BattleRealManagerのステートを変更する。
    /// </summary>
    public void RequestChangeState(E_BATTLE_REAL_STATE state)
    {
        if (m_StateMachine == null)
        {
            return;
        }

        m_StateMachine.Goto(state);
    }

    /// <summary>
    /// プレイヤーキャラが死亡した時に呼び出す。
    /// </summary>
    public void DeadPlayer()
    {
        m_IsPlayerDead = true;
    }

    public void GameStart()
    {
        m_BattleManager.GameStart();
    }

    public void ToHacking()
    {
        m_BattleManager.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_HACKING);
    }

    public void BossBattleStart()
    {
        m_BattleManager.BossBattleStart();
    }

    public void GameClearWithoutHackingComplete()
    {
        m_BattleManager.GameClearWithoutHackingComplete();
    }

    public void GameClearWithHackingComplete()
    {
        m_BattleManager.GameClearWithHackingComplete();
    }

    public void GameOver()
    {
        m_BattleManager.GameOver();
    }

    public void End()
    {
        m_BattleManager.RequestChangeState(E_BATTLE_STATE.END);
    }
}
