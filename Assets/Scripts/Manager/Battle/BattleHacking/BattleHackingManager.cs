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

    private BattleHackingLevelParamSet m_LevelParamSet;

    private StateMachine<E_BATTLE_HACKING_STATE> m_StateMachine;

    public BattleHackingInputManager InputManager { get; private set; }
    public BattleHackingTimerManager HackingTimerManager { get; private set; }
    public BattleHackingPlayerManager PlayerManager { get; private set; }
    public BattleHackingEnemyManager EnemyManager { get; private set; }
    public BattleHackingBulletManager BulletManager { get; private set; }
    public BattleHackingCollisionManager CollisionManager { get; private set; }

    public bool IsHackingSuccess { get; private set; }

    private bool m_IsDeadPlayer;
    private bool m_IsDeadBoss;

    public float CurrentRemainTime { get; private set; }
    public float MaxRemainTime { get; private set; }
    public float CurrentRemainBonusTime { get; private set; }
    public float MaxRemainBonusTime { get; private set; }

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

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.TRANSITION_TO_REAL)
        {
            m_OnStart = StartOnTransitionToReal,
            m_OnUpdate = UpdateOnTransitionToReal,
            m_OnLateUpdate = LateUpdateOnTransitionToReal,
            m_OnFixedUpdate = FixedUpdateOnTransitionToReal,
            m_OnEnd = EndOnTransitionToReal,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.START)
        {
            m_OnStart = StartOnStart,
            m_OnUpdate = UpdateOnStart,
            m_OnLateUpdate = LateUpdateOnStart,
            m_OnFixedUpdate = FixedUpdateOnStart,
            m_OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.STAY_REAL)
        {
            m_OnStart = StartOnStayReal,
            m_OnUpdate = UpdateOnStayReal,
            m_OnLateUpdate = LateUpdateOnStayReal,
            m_OnFixedUpdate = FixedUpdateOnStayReal,
            m_OnEnd = EndOnStayReal,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.TRANSITION_TO_HACKING)
        {
            m_OnStart = StartOnTransitionToHacking,
            m_OnUpdate = UpdateOnTransitionToHacking,
            m_OnLateUpdate = LateUpdateOnTransitionToHacking,
            m_OnFixedUpdate = FixedUpdateOnTransitionToHacking,
            m_OnEnd = EndOnTransitionToHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.GAME)
        {
            m_OnStart = StartOnGame,
            m_OnUpdate = UpdateOnGame,
            m_OnLateUpdate = LateUpdateOnGame,
            m_OnFixedUpdate = FixedUpdateOnGame,
            m_OnEnd = EndOnGame,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.GAME_CLEAR)
        {
            m_OnStart = StartOnGameClear,
            m_OnUpdate = UpdateOnGameClear,
            m_OnLateUpdate = LateUpdateOnGameClear,
            m_OnFixedUpdate = FixedUpdateOnGameClear,
            m_OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.GAME_OVER)
        {
            m_OnStart = StartOnGameOver,
            m_OnUpdate = UpdateOnGameOver,
            m_OnLateUpdate = LateUpdateOnGameOver,
            m_OnFixedUpdate = FixedUpdateOnGameOver,
            m_OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.END)
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
        CollisionManager = new BattleHackingCollisionManager();

        InputManager.OnInitialize();
        HackingTimerManager.OnInitialize();
        PlayerManager.OnInitialize();
        EnemyManager.OnInitialize();
        BulletManager.OnInitialize();
        CollisionManager.OnInitialize();

        RequestChangeState(E_BATTLE_HACKING_STATE.START);
    }

    public override void OnFinalize()
    {
        CollisionManager.OnFinalize();
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
        IsHackingSuccess = false;

        InputManager.OnStart();
        HackingTimerManager.OnStart();
        PlayerManager.OnStart();
        EnemyManager.OnStart();
        BulletManager.OnStart();
        CollisionManager.OnStart();

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
        CollisionManager.DestroyDrawingColliderMeshes();

        InputManager.OnUpdate();
        HackingTimerManager.OnUpdate();
        PlayerManager.OnUpdate();
        EnemyManager.OnUpdate();
        BulletManager.OnUpdate();
        CollisionManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        HackingTimerManager.OnLateUpdate();
        PlayerManager.OnLateUpdate();
        EnemyManager.OnLateUpdate();
        BulletManager.OnLateUpdate();
        CollisionManager.OnLateUpdate();

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

        // ゲームの終了をチェック
        CheckGameEnd();
    }

    private void FixedUpdateOnGame()
    {
        HackingTimerManager.OnFixedUpdate();
        PlayerManager.OnFixedUpdate();
        EnemyManager.OnFixedUpdate();
        BulletManager.OnFixedUpdate();
        CollisionManager.OnFixedUpdate();

        if (CurrentRemainBonusTime <= 0)
        {
            CurrentRemainTime -= Time.fixedDeltaTime;
            if (CurrentRemainTime <= 0)
            {
                CurrentRemainTime = 0;
                RequestChangeState(E_BATTLE_HACKING_STATE.GAME_OVER);
            }
        }
        else
        {
            CurrentRemainBonusTime -= Time.fixedDeltaTime;
            if (CurrentRemainBonusTime <= 0)
            {
                CurrentRemainBonusTime = 0;
            }
        }
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
        IsHackingSuccess = true;
        BattleManager.Instance.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_REAL);
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
        IsHackingSuccess = false;
        BattleManager.Instance.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_REAL);
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

    public void RequestChangeState(E_BATTLE_HACKING_STATE state)
    {
        if (m_StateMachine == null)
        {
            return;
        }

        m_StateMachine.Goto(state);
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
