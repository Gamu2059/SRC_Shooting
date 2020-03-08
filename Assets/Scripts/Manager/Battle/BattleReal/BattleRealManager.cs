using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バトルのリアルモードの処理を管理する。
/// </summary>
[Serializable]
public partial class BattleRealManager : ControllableObject, IStateCallback<E_BATTLE_REAL_STATE>
{
    #region Define

    private class StateCycle : StateCycleBase<BattleRealManager, E_BATTLE_REAL_STATE> { }

    private class InnerState : State<E_BATTLE_REAL_STATE, BattleRealManager>
    {
        public InnerState(E_BATTLE_REAL_STATE state, BattleRealManager target) : base(state, target) { }
        public InnerState(E_BATTLE_REAL_STATE state, BattleRealManager target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    public static BattleRealManager Instance { get; private set; }

    #region Field

    private BattleManager m_BattleManager;
    private BattleRealParamSet m_ParamSet;

    private StateMachine<E_BATTLE_REAL_STATE, BattleRealManager> m_StateMachine;

    private Action<E_BATTLE_REAL_STATE> m_OnChangeState;

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

        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.START, this, new StartState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.GAME, this, new GameState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.DEAD, this, new DeadState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.CHARGE_SHOT, this, new ChargeShotState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.TO_HACKING, this, new ToHackingState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.STAY_HACKING, this, new StayHackingState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.FROM_HACKING, this, new FromHackingState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.BEGIN_BOSS_BATTLE, this)
        {
            m_OnStart = StartOnBeginBossBattle,
            m_OnUpdate = UpdateOnBeginBossBattle,
            m_OnLateUpdate = LateUpdateOnBeginBossBattle,
            m_OnFixedUpdate = FixedUpdateOnBeginBossBattle,
            m_OnEnd = EndOnBeginBossBattle,
        });
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.GAME_CLEAR, this, new GameClearState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.GAME_OVER, this, new GameOverState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.END, this, new EndState()));

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

        RequestChangeState(E_BATTLE_REAL_STATE.START);

        m_BattleManager.SetChangeStateCallback(OnChangeStateBattleManager);
    }

    public override void OnFinalize()
    {
        m_OnChangeState = null;

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

        Instance = null;

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

    private void OnChangeStateBattleManager(E_BATTLE_STATE nextState)
    {
        Debug.Log(nextState);
    }

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
    /// BattleRealManagerのステート変更時に呼び出すコールバックを設定する。
    /// </summary>
    public void SetChangeStateCallback(Action<E_BATTLE_REAL_STATE> callback)
    {
        m_OnChangeState += callback;
    }

    public void OnChangeState(E_BATTLE_REAL_STATE state)
    {
        m_OnChangeState?.Invoke(state);
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
        m_BattleManager.RequestChangeState(E_BATTLE_STATE.TO_HACKING);
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
