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
    public BattleRealCameraManager CameraManager { get; private set; }

    private bool m_IsPlayerDead;

    public Action OnTransitionToHacking;
    public Action OnTransitionToReal;

    #endregion

    public static BattleRealManager Instance {
        get {
            if (BattleManager.Instance == null)
            {
                return null;
            }
            return BattleManager.Instance.RealManager;
        }
    }

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
            m_OnStart = StartOnStart,
            m_OnUpdate = UpdateOnStart,
            m_OnLateUpdate = LateUpdateOnStart,
            m_OnFixedUpdate = FixedUpdateOnStart,
            m_OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME)
        {
            m_OnStart = StartOnBeforeBeginGame,
            m_OnUpdate = UpdateOnBeforeBeginGame,
            m_OnLateUpdate = LateUpdateOnBeforeBeginGame,
            m_OnFixedUpdate = FixedUpdateOnBeforeBeginGame,
            m_OnEnd = EndOnBeforeBeginGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BEGIN_GAME_PERFORMANCE)
        {
            m_OnStart = StartOnBeforeBeginGamePerformance,
            m_OnUpdate = UpdateOnBeforeBeginGamePerformance,
            m_OnLateUpdate = LateUpdateOnBeforeBeginGamePerformance,
            m_OnFixedUpdate = FixedUpdateOnBeforeBeginGamePerformance,
            m_OnEnd = EndOnBeforeBeginGamePerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEGIN_GAME)
        {
            m_OnStart = StartOnBeginGame,
            m_OnUpdate = UpdateOnBeginGame,
            m_OnLateUpdate = LateUpdateOnBeginGame,
            m_OnFixedUpdate = FixedUpdateOnBeginGame,
            m_OnEnd = EndOnBeginGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME)
        {
            m_OnStart = StartOnGame,
            m_OnUpdate = UpdateOnGame,
            m_OnLateUpdate = LateUpdateOnGame,
            m_OnFixedUpdate = FixedUpdateOnGame,
            m_OnEnd = EndOnGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.DEAD)
        {
            m_OnStart = StartOnDead,
            m_OnUpdate = UpdateOnDead,
            m_OnLateUpdate = LateUpdateOnDead,
            m_OnFixedUpdate = FixedUpdateOnDead,
            m_OnEnd = EndOnDead,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_BOSS_BATTLE_PERFORMANCE)
        {
            m_OnStart = StartOnBeforeBossBattlePerformance,
            m_OnUpdate = UpdateOnBeforeBossBattlePerformance,
            m_OnLateUpdate = LateUpdateOnBeforeBossBattlePerformance,
            m_OnFixedUpdate = FixedUpdateOnBeforeBossBattlePerformance,
            m_OnEnd = EndOnBeforeBossBattlePerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.TRANSITION_TO_HACKING)
        {
            m_OnStart = StartOnTransitionToHacking,
            m_OnUpdate = UpdateOnTransitionToHacking,
            m_OnLateUpdate = LateUpdateOnTransitionToHacking,
            m_OnFixedUpdate = FixedUpdateOnTransitionToHacking,
            m_OnEnd = EndOnTransitionToHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.STAY_HACKING)
        {
            m_OnStart = StartOnStayHacking,
            m_OnUpdate = UpdateOnStayHacking,
            m_OnLateUpdate = LateUpdateOnStayHacking,
            m_OnFixedUpdate = FixedUpdateOnStayHacking,
            m_OnEnd = EndOnStayHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.TRANSITION_TO_REAL)
        {
            m_OnStart = StartOnTransitionToReal,
            m_OnUpdate = UpdateOnTransitionToReal,
            m_OnLateUpdate = LateUpdateOnTransitionToReal,
            m_OnFixedUpdate = FixedUpdateOnTransitionToReal,
            m_OnEnd = EndOnTransitionToReal,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.BEFORE_GAME_CLEAR_PERFORMANCE)
        {
            m_OnStart = StartOnBeforeGameClearPerformance,
            m_OnUpdate = UpdateOnBeforeGameClearPerformance,
            m_OnLateUpdate = LateUpdateOnBeforeGameClearPerformance,
            m_OnFixedUpdate = FixedUpdateOnBeforeGameClearPerformance,
            m_OnEnd = EndOnBeforeGameClearPerformance,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME_CLEAR)
        {
            m_OnStart = StartOnGameClear,
            m_OnUpdate = UpdateOnGameClear,
            m_OnLateUpdate = LateUpdateOnGameClear,
            m_OnFixedUpdate = FixedUpdateOnGameClear,
            m_OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.GAME_OVER)
        {
            m_OnStart = StartOnGameOver,
            m_OnUpdate = UpdateOnGameOver,
            m_OnLateUpdate = LateUpdateOnGameOver,
            m_OnFixedUpdate = FixedUpdateOnGameOver,
            m_OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new State<E_BATTLE_REAL_STATE>(E_BATTLE_REAL_STATE.END)
        {
            m_OnStart = StartOnEnd,
            m_OnUpdate = UpdateOnEnd,
            m_OnLateUpdate = LateUpdateOnEnd,
            m_OnFixedUpdate = FixedUpdateOnEnd,
            m_OnEnd = EndOnEnd,
        });

        InputManager = new BattleRealInputManager();
        RealTimerManager = new BattleRealTimerManager();
        EventManager = new BattleRealEventManager(m_ParamSet.EventTriggerParamSet);
        PlayerManager = new BattleRealPlayerManager(m_ParamSet.PlayerManagerParamSet);
        EnemyGroupManager = new BattleRealEnemyGroupManager(m_ParamSet.EnemyGroupManagerParamSet);
        EnemyManager = new BattleRealEnemyManager(m_ParamSet.EnemyManagerParamSet);
        BulletManager = new BattleRealBulletManager(m_ParamSet.BulletManagerParamSet);
        ItemManager = new BattleRealItemManager(m_ParamSet.ItemManagerParamSet);
        CollisionManager = new BattleRealCollisionManager();
        CameraManager = new BattleRealCameraManager();

        InputManager.OnInitialize();
        RealTimerManager.OnInitialize();
        EventManager.OnInitialize();
        PlayerManager.OnInitialize();
        EnemyGroupManager.OnInitialize();
        EnemyManager.OnInitialize();
        BulletManager.OnInitialize();
        ItemManager.OnInitialize();
        CollisionManager.OnInitialize();
        CameraManager.OnInitialize();

        m_StateMachine.Goto(E_BATTLE_REAL_STATE.START);
    }

    public override void OnFinalize()
    {
        CameraManager.OnFinalize();
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
        CameraManager.OnStart();

        CameraManager.RegisterCamera(BattleManager.Instance.BattleRealBackCamera, E_CAMERA_TYPE.BACK_CAMERA);
        CameraManager.RegisterCamera(BattleManager.Instance.BattleRealFrontCamera, E_CAMERA_TYPE.FRONT_CAMERA);

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
        m_IsPlayerDead = false;
        InputManager.RegistInput();
    }

    private void UpdateOnGame()
    {
        // 消滅の更新
        EnemyGroupManager.GotoPool();
        EnemyManager.GotoPool();
        BulletManager.GotoPool();
        ItemManager.GotoPool();
        CollisionManager.DestroyDrawingColliderMeshes();

        InputManager.OnUpdate();
        RealTimerManager.OnUpdate();
        EventManager.OnUpdate();
        PlayerManager.OnUpdate();
        EnemyGroupManager.OnUpdate();
        EnemyManager.OnUpdate();
        BulletManager.OnUpdate();
        ItemManager.OnUpdate();
        CameraManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        RealTimerManager.OnLateUpdate();
        EventManager.OnLateUpdate();
        PlayerManager.OnLateUpdate();
        EnemyGroupManager.OnLateUpdate();
        EnemyManager.OnLateUpdate();
        BulletManager.OnLateUpdate();
        ItemManager.OnLateUpdate();
        CameraManager.OnLateUpdate();

        // 衝突フラグクリア
        PlayerManager.ClearColliderFlag();
        EnemyManager.ClearColliderFlag();
        BulletManager.ClearColliderFlag();
        ItemManager.ClearColliderFlag();

        // 衝突情報の更新
        PlayerManager.UpdateCollider();
        EnemyManager.UpdateCollider();
        BulletManager.UpdateCollider();
        ItemManager.UpdateCollider();

        // 衝突判定処理
        CollisionManager.CheckCollision();
        CollisionManager.DrawCollider();

        // 衝突処理
        PlayerManager.ProcessCollision();
        EnemyManager.ProcessCollision();
        BulletManager.ProcessCollision();
        ItemManager.ProcessCollision();

        CheckDeadPlayer();
    }

    private void CheckDeadPlayer()
    {
        if (m_IsPlayerDead)
        {
            RequestChangeState(E_BATTLE_REAL_STATE.DEAD);
        }
    }

    private void FixedUpdateOnGame()
    {
        RealTimerManager.OnFixedUpdate();
        EventManager.OnFixedUpdate();
        PlayerManager.OnFixedUpdate();
        EnemyGroupManager.OnFixedUpdate();
        EnemyManager.OnFixedUpdate();
        BulletManager.OnFixedUpdate();
        ItemManager.OnFixedUpdate();
        CameraManager.OnFixedUpdate();
    }

    private void EndOnGame()
    {
        InputManager.RemoveInput();
    }

    #endregion

    #region Dead State

    private void StartOnDead()
    {
        PlayerManager.SetPlayerActive(false);
        var pData = GameManager.Instance.PlayerData;
        if (pData.m_Last < 1)
        {
            BattleManager.Instance.GameOver();
        }
        else
        {
            pData.DecreaseLast();
            var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 2);
            timer.SetTimeoutCallBack(() =>
            {
                timer = null;
                RequestChangeState(E_BATTLE_REAL_STATE.GAME);
            });
            TimerManager.Instance.RegistTimer(timer);
            Time.timeScale = 0.1f;
        }
    }

    private void UpdateOnDead()
    {
        // 消滅の更新
        EnemyGroupManager.GotoPool();
        EnemyManager.GotoPool();
        BulletManager.GotoPool();
        ItemManager.GotoPool();
        CollisionManager.DestroyDrawingColliderMeshes();

        //InputManager.OnUpdate();
        RealTimerManager.OnUpdate();
        EventManager.OnUpdate();
        //PlayerManager.OnUpdate();
        EnemyGroupManager.OnUpdate();
        EnemyManager.OnUpdate();
        BulletManager.OnUpdate();
        ItemManager.OnUpdate();
        CameraManager.OnUpdate();
    }

    private void LateUpdateOnDead()
    {
        RealTimerManager.OnLateUpdate();
        EventManager.OnLateUpdate();
        //PlayerManager.OnLateUpdate();
        EnemyGroupManager.OnLateUpdate();
        EnemyManager.OnLateUpdate();
        BulletManager.OnLateUpdate();
        ItemManager.OnLateUpdate();
        CameraManager.OnLateUpdate();

        // 衝突フラグクリア
        PlayerManager.ClearColliderFlag();
        EnemyManager.ClearColliderFlag();
        BulletManager.ClearColliderFlag();
        ItemManager.ClearColliderFlag();

        // 衝突情報の更新
        PlayerManager.UpdateCollider();
        EnemyManager.UpdateCollider();
        BulletManager.UpdateCollider();
        ItemManager.UpdateCollider();
    }

    private void FixedUpdateOnDead()
    {
        RealTimerManager.OnFixedUpdate();
        EventManager.OnFixedUpdate();
        //PlayerManager.OnFixedUpdate();
        EnemyGroupManager.OnFixedUpdate();
        EnemyManager.OnFixedUpdate();
        BulletManager.OnFixedUpdate();
        ItemManager.OnFixedUpdate();
        CameraManager.OnFixedUpdate();
    }

    private void EndOnDead()
    {
        PlayerManager.InitPlayerPosition();
        PlayerManager.SetPlayerActive(true);
        PlayerManager.SetPlayerInvinsible();
        Time.timeScale = 1;
    }

    #endregion

    #region Before Boss Battle Performance State

    private void StartOnBeforeBossBattlePerformance()
    {
        EnemyGroupManager.CreateBossGroup();
        RequestChangeState(E_BATTLE_REAL_STATE.GAME);
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
        var bossBgmName = BattleManager.Instance.ParamSet.BgmParamSet.BossBgmName;
        AudioManager.Instance.PlayBgm(bossBgmName);
    }

    #endregion

    #region Transition To Hacking State

    private void StartOnTransitionToHacking()
    {
        CameraManager.PauseCamera(E_CAMERA_TYPE.BACK_CAMERA);
        CameraManager.PauseCamera(E_CAMERA_TYPE.FRONT_CAMERA);
        OnTransitionToHacking?.Invoke();
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
        CameraManager.ResumeCamera(E_CAMERA_TYPE.BACK_CAMERA);
        CameraManager.ResumeCamera(E_CAMERA_TYPE.FRONT_CAMERA);
        OnTransitionToReal?.Invoke();
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

    public void DeadPlayer()
    {
        m_IsPlayerDead = true;
    }
}
