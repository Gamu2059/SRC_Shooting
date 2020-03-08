using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バトルのハッキングモードの処理を管理する。
/// </summary>
[Serializable]
public partial class BattleHackingManager : ControllableObject, IStateCallback<E_BATTLE_HACKING_STATE>
{
    #region Define

    private class StateCycle : StateCycleBase<BattleHackingManager, E_BATTLE_HACKING_STATE> { }

    private class InnerState : State<E_BATTLE_HACKING_STATE, BattleHackingManager>
    {
        public InnerState(E_BATTLE_HACKING_STATE state, BattleHackingManager target) : base(state, target) { }
        public InnerState(E_BATTLE_HACKING_STATE state, BattleHackingManager target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    private const float GAME_OVER_DURATION = 1f;

    public static BattleHackingManager Instance { get; private set; }

    #region Field

    private BattleHackingParamSet m_ParamSet;

    private BattleHackingLevelParamSet m_LevelParamSet;

    private BattleManager m_BattleManager;
    private StateMachine<E_BATTLE_HACKING_STATE, BattleHackingManager> m_StateMachine;

    private Action<E_BATTLE_HACKING_STATE> m_OnChangeState;

    public BattleHackingInputManager InputManager { get; private set; }
    public BattleHackingTimerManager HackingTimerManager { get; private set; }
    public BattleHackingPlayerManager PlayerManager { get; private set; }
    public BattleHackingEnemyManager EnemyManager { get; private set; }
    public BattleHackingBulletManager BulletManager { get; private set; }
    public BattleHackingEffectManager EffectManager { get; private set; }
    public BattleHackingCollisionManager CollisionManager { get; private set; }
    public BattleHackingCameraManager CameraManager { get; private set; }

    public bool IsHackingSuccess { get; private set; }

    private bool m_IsDeadPlayer;
    private bool m_IsDeadBoss;

    public float CurrentRemainTime { get; private set; }
    public float MaxRemainTime { get; private set; }
    public float CurrentRemainBonusTime { get; private set; }
    public float MaxRemainBonusTime { get; private set; }

    #endregion

    public BattleHackingManager(BattleManager battleManager, BattleHackingParamSet paramSet)
    {
        m_BattleManager = battleManager;
        m_ParamSet = paramSet;
        Instance = this;
    }

    #region Game Cycyle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_HACKING_STATE, BattleHackingManager>();

        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.TRANSITION_TO_REAL, this)
        {
            m_OnStart = StartOnTransitionToReal,
            m_OnUpdate = UpdateOnTransitionToReal,
            m_OnLateUpdate = LateUpdateOnTransitionToReal,
            m_OnFixedUpdate = FixedUpdateOnTransitionToReal,
            m_OnEnd = EndOnTransitionToReal,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.START, this)
        {
            m_OnStart = StartOnStart,
            m_OnUpdate = UpdateOnStart,
            m_OnLateUpdate = LateUpdateOnStart,
            m_OnFixedUpdate = FixedUpdateOnStart,
            m_OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.STAY_REAL, this)
        {
            m_OnStart = StartOnStayReal,
            m_OnUpdate = UpdateOnStayReal,
            m_OnLateUpdate = LateUpdateOnStayReal,
            m_OnFixedUpdate = FixedUpdateOnStayReal,
            m_OnEnd = EndOnStayReal,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.TRANSITION_TO_HACKING, this)
        {
            m_OnStart = StartOnTransitionToHacking,
            m_OnUpdate = UpdateOnTransitionToHacking,
            m_OnLateUpdate = LateUpdateOnTransitionToHacking,
            m_OnFixedUpdate = FixedUpdateOnTransitionToHacking,
            m_OnEnd = EndOnTransitionToHacking,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.GAME, this)
        {
            m_OnStart = StartOnGame,
            m_OnUpdate = UpdateOnGame,
            m_OnLateUpdate = LateUpdateOnGame,
            m_OnFixedUpdate = FixedUpdateOnGame,
            m_OnEnd = EndOnGame,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.GAME_CLEAR, this)
        {
            m_OnStart = StartOnGameClear,
            m_OnUpdate = UpdateOnGameClear,
            m_OnLateUpdate = LateUpdateOnGameClear,
            m_OnFixedUpdate = FixedUpdateOnGameClear,
            m_OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.GAME_OVER, this)
        {
            m_OnStart = StartOnGameOver,
            m_OnUpdate = UpdateOnGameOver,
            m_OnLateUpdate = LateUpdateOnGameOver,
            m_OnFixedUpdate = FixedUpdateOnGameOver,
            m_OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.END, this)
        {
            m_OnStart = StartOnEnd,
            m_OnUpdate = UpdateOnEnd,
            m_OnLateUpdate = LateUpdateOnEnd,
            m_OnFixedUpdate = FixedUpdateOnEnd,
            m_OnEnd = EndOnEnd,
        });

        InputManager = new BattleHackingInputManager();
        HackingTimerManager = new BattleHackingTimerManager();
        PlayerManager = new BattleHackingPlayerManager(m_ParamSet.PlayerManagerParamSet);
        EnemyManager = new BattleHackingEnemyManager(m_ParamSet.EnemyManagerParamSet);
        BulletManager = new BattleHackingBulletManager(m_ParamSet.BulletManagerParamSet);
        EffectManager = new BattleHackingEffectManager();
        CollisionManager = new BattleHackingCollisionManager(m_BattleManager.ParamSet.ColliderMaterial);
        CameraManager = new BattleHackingCameraManager();

        InputManager.OnInitialize();
        HackingTimerManager.OnInitialize();
        PlayerManager.OnInitialize();
        EnemyManager.OnInitialize();
        BulletManager.OnInitialize();
        EffectManager.OnInitialize();
        CollisionManager.OnInitialize();
        CameraManager.OnInitialize();

        CameraManager.RegisterCamera(m_BattleManager.BattleHackingBackCamera, E_CAMERA_TYPE.BACK_CAMERA);
        CameraManager.RegisterCamera(m_BattleManager.BattleHackingFrontCamera, E_CAMERA_TYPE.FRONT_CAMERA);

        RequestChangeState(E_BATTLE_HACKING_STATE.START);

        m_BattleManager.SetChangeStateCallback(OnChangeStateBattleManager);
    }

    public override void OnFinalize()
    {
        CameraManager.OnFinalize();
        CollisionManager.OnFinalize();
        EffectManager.OnFinalize();
        BulletManager.OnFinalize();
        EnemyManager.OnFinalize();
        PlayerManager.OnFinalize();
        HackingTimerManager.OnFinalize();
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

    #region Start State

    private void StartOnStart()
    {
        IsHackingSuccess = false;

        InputManager.OnStart();
        HackingTimerManager.OnStart();
        PlayerManager.OnStart();
        EnemyManager.OnStart();
        BulletManager.OnStart();
        EffectManager.OnStart();
        CollisionManager.OnStart();
        CameraManager.OnStart();

        SetHackingLevel(0);

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

    #region Transition To Real State

    private void StartOnTransitionToReal()
    {
        //var battleManager = BattleManager.Instance;
        //var player = PlayerManager.Player;
        //var centerPos = battleManager.BattleHackingStageManager.CalcViewportPosFromWorldPosition(player.transform, false);

        //// StageManagerは原点が中央にあるため、原点をずらす
        //centerPos += Vector2.one * 0.5f;
        //battleManager.BattleHackingUiManager.GridHoleEffect.PlayEffect(centerPos);
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
        PlayerManager.OnPutAway();
        EnemyManager.OnPutAway();
        BulletManager.OnPutAway();
        EffectManager.OnPutAway();

        m_BattleManager.BattleHackingUiManager.GridHoleEffect.StopEffect();
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

    #region Transition To Hacking State

    private void StartOnTransitionToHacking()
    {
        if (m_LevelParamSet == null)
        {
            Debug.LogError("Hacking Level Param Set is null!");
            RequestChangeState(E_BATTLE_HACKING_STATE.GAME_OVER);
            return;
        }

        m_IsDeadPlayer = false;
        m_IsDeadBoss = false;

        CurrentRemainTime = 20;
        MaxRemainTime = 20;
        // ボーナスタイムはアイテムの取得数に応じる
        CurrentRemainBonusTime = 10;
        MaxRemainBonusTime = 10;

        PlayerManager.OnPrepare(m_LevelParamSet);
        EnemyManager.OnPrepare(m_LevelParamSet);
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

    #region Game State

    private void StartOnGame()
    {
        InputManager.RegistInput();
    }

    private void UpdateOnGame()
    {
        // 消滅の更新
        EnemyManager.GotoPool();
        BulletManager.GotoPool();
        EffectManager.GotoPool();
        CollisionManager.DestroyDrawingColliderMeshes();

        InputManager.OnUpdate();
        HackingTimerManager.OnUpdate();
        PlayerManager.OnUpdate();
        EnemyManager.OnUpdate();
        BulletManager.OnUpdate();
        EffectManager.OnUpdate();
        CollisionManager.OnUpdate();
        CameraManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        HackingTimerManager.OnLateUpdate();
        PlayerManager.OnLateUpdate();
        EnemyManager.OnLateUpdate();
        BulletManager.OnLateUpdate();
        EffectManager.OnLateUpdate();
        CollisionManager.OnLateUpdate();
        CameraManager.OnLateUpdate();

        // 衝突フラグクリア
        PlayerManager.ClearColliderFlag();
        EnemyManager.ClearColliderFlag();
        BulletManager.ClearColliderFlag();

        // 衝突情報の更新
        PlayerManager.UpdateCollider();
        EnemyManager.UpdateCollider();
        BulletManager.UpdateCollider();

        // 衝突判定処理
        CollisionManager.CheckCollision();
        CollisionManager.DrawCollider();

        // 衝突処理
        PlayerManager.ProcessCollision();
        EnemyManager.ProcessCollision();
        BulletManager.ProcessCollision();

        if (CurrentRemainBonusTime <= 0)
        {
            CurrentRemainTime -= Time.deltaTime;
            if (CurrentRemainTime <= 0)
            {
                CurrentRemainTime = 0;
                RequestChangeState(E_BATTLE_HACKING_STATE.GAME_OVER);
            }
        }
        else
        {
            CurrentRemainBonusTime -= Time.deltaTime;
            if (CurrentRemainBonusTime <= 0)
            {
                CurrentRemainBonusTime = 0;
            }
        }

        // ゲームの終了をチェック
        CheckGameEnd();
    }

    private void FixedUpdateOnGame()
    {
        HackingTimerManager.OnFixedUpdate();
        PlayerManager.OnFixedUpdate();
        EnemyManager.OnFixedUpdate();
        BulletManager.OnFixedUpdate();
        EffectManager.OnFixedUpdate();
        CollisionManager.OnFixedUpdate();
        CameraManager.OnFixedUpdate();
    }

    private void EndOnGame()
    {
        InputManager.RemoveInput();
    }

    private void CheckGameEnd()
    {
        if (m_IsDeadPlayer && m_IsDeadBoss)
        {
            RequestChangeState(E_BATTLE_HACKING_STATE.GAME_CLEAR);
        }
        else
        {
            if (m_IsDeadPlayer)
            {
                RequestChangeState(E_BATTLE_HACKING_STATE.GAME_OVER);
            }
            if (m_IsDeadBoss)
            {
                RequestChangeState(E_BATTLE_HACKING_STATE.GAME_CLEAR);
            }
        }
    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {
        var destroyBulletTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 1f);
        destroyBulletTimer.SetTimeoutCallBack(() =>
        {
            destroyBulletTimer = null;

            var destroyBossTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 1f);
            destroyBossTimer.SetTimeoutCallBack(() =>
            {
                destroyBossTimer = null;
                m_BattleManager.RequestChangeState(E_BATTLE_STATE.TO_REAL);
            });
            TimerManager.Instance.RegistTimer(destroyBossTimer);

            CameraManager.Shake(m_ParamSet.DestroyBossShakeParam);
        });
        TimerManager.Instance.RegistTimer(destroyBulletTimer);

        IsHackingSuccess = true;


        BulletManager.DestroyAllEnemyBullet();
        CameraManager.Shake(m_ParamSet.DestroyBulletShakeParam);
    }

    private void UpdateOnGameClear()
    {
        BulletManager.GotoPool();

        EnemyManager.OnUpdate();
        EffectManager.OnUpdate();
        CameraManager.OnUpdate();
    }

    private void LateUpdateOnGameClear()
    {
        EnemyManager.OnLateUpdate();
        EffectManager.OnLateUpdate();
        CameraManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameClear()
    {
        EnemyManager.OnFixedUpdate();
        EffectManager.OnFixedUpdate();
        CameraManager.OnFixedUpdate();
    }

    private void EndOnGameClear()
    {

    }

    #endregion

    #region Game Over State

    private void StartOnGameOver()
    {
        IsHackingSuccess = false;

        var waitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 0.6f);
        waitTimer.SetTimeoutCallBack(() =>
        {
            waitTimer = null;
            m_BattleManager.RequestChangeState(E_BATTLE_STATE.TO_REAL);
        });
        TimerManager.Instance.RegistTimer(waitTimer);
    }

    private void UpdateOnGameOver()
    {
        EffectManager.OnUpdate();
        CameraManager.OnUpdate();
    }

    private void LateUpdateOnGameOver()
    {
        EffectManager.OnLateUpdate();
        CameraManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameOver()
    {
        EffectManager.OnFixedUpdate();
        CameraManager.OnFixedUpdate();
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

    private void OnChangeStateBattleManager(E_BATTLE_STATE nextState)
    {

    }

    /// <summary>
    /// BattleHackingManagerのステートを変更する。
    /// </summary>
    public void RequestChangeState(E_BATTLE_HACKING_STATE state)
    {
        if (m_StateMachine == null)
        {
            return;
        }

        m_StateMachine.Goto(state);
    }
    
    /// <summary>
     /// BattleHackingManagerのステート変更時に呼び出すコールバックを設定する。
     /// </summary>
    public void SetChangeStateCallback(Action<E_BATTLE_HACKING_STATE> callback)
    {
        m_OnChangeState += callback;
    }

    public void OnChangeState(E_BATTLE_HACKING_STATE state)
    {
        m_OnChangeState?.Invoke(state);
    }

    /// <summary>
    /// ハッキングのレベルを指定する。
    /// これは、ハッキングモードに遷移する前に指定しなければ意味をなさない。
    /// 指定したレベルが存在しなければ、一番最初にセットされているレベルを指定する。
    /// </summary>
    public void SetHackingLevel(string hackingLevelLabel)
    {
        if (m_ParamSet == null || m_ParamSet.LevelParamSets == null)
        {
            return;
        }

        var levels = m_ParamSet.LevelParamSets;
        for (int i = 0; i < levels.Length; i++)
        {
            var level = levels[i];
            if (hackingLevelLabel == level.GeneratorLabel)
            {
                m_LevelParamSet = level;
                return;
            }
        }

        if (levels.Length > 0)
        {
            m_LevelParamSet = levels[0];
        }
    }

    /// <summary>
    /// ハッキングのレベルを指定する。
    /// これは、ハッキングモードに遷移する前に指定しなければ意味をなさない。
    /// 指定したレベルが存在しなければ、一番最初にセットされているレベルを指定する。
    /// </summary>
    public void SetHackingLevel(int hackingLevelIndex)
    {
        if (m_ParamSet == null || m_ParamSet.LevelParamSets == null)
        {
            return;
        }

        var levels = m_ParamSet.LevelParamSets;
        if ((hackingLevelIndex < 0 || hackingLevelIndex >= levels.Length) && levels.Length > 0)
        {
            m_LevelParamSet = levels[0];
            return;
        }

        m_LevelParamSet = levels[hackingLevelIndex];
    }

    public void DeadPlayer()
    {
        m_IsDeadPlayer = true;
    }

    public void DeadBoss()
    {
        m_IsDeadBoss = true;
    }
}
