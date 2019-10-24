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
            OnStart = StartOnTransitionToReal,
            OnUpdate = UpdateOnTransitionToReal,
            OnLateUpdate = LateUpdateOnTransitionToReal,
            OnFixedUpdate = FixedUpdateOnTransitionToReal,
            OnEnd = EndOnTransitionToReal,
        });

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

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.TRANSITION_TO_HACKING)
        {
            OnStart = StartOnTransitionToHacking,
            OnUpdate = UpdateOnTransitionToHacking,
            OnLateUpdate = LateUpdateOnTransitionToHacking,
            OnFixedUpdate = FixedUpdateOnTransitionToHacking,
            OnEnd = EndOnTransitionToHacking,
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

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.END)
        {
            OnStart = StartOnEnd,
            OnUpdate = UpdateOnEnd,
            OnLateUpdate = LateUpdateOnEnd,
            OnFixedUpdate = FixedUpdateOnEnd,
            OnEnd = EndOnEnd,
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
        PlayerManager.ResetShotFlag();
    }

    private void UpdateOnGame()
    {
        InputManager.OnUpdate();
        HackingTimerManager.OnUpdate();
        PlayerManager.OnUpdate();
        BulletManager.OnUpdate();
        CollisionManager.OnUpdate();
    }

    private void LateUpdateOnGame()
    {
        HackingTimerManager.OnLateUpdate();
        PlayerManager.OnLateUpdate();
        BulletManager.OnLateUpdate();
        CollisionManager.OnLateUpdate();

        // 衝突フラグのクリア
        BulletManager.ClearColliderFlag();

        // 衝突情報の更新
        BulletManager.UpdateCollider();

        // 衝突判定処理
        CollisionManager.CheckCollision();
        CollisionManager.DrawCollider();

        // 衝突処理
        BulletManager.ProcessCollision();

        BulletManager.GotoPool();
    }

    private void FixedUpdateOnGame()
    {
        HackingTimerManager.OnFixedUpdate();
        PlayerManager.OnFixedUpdate();
        BulletManager.OnFixedUpdate();
        CollisionManager.OnFixedUpdate();
    }

    private void EndOnGame()
    {
        InputManager.RemoveInput();
        AudioManager.Instance.StopSe(AudioManager.E_SE_GROUP.PLAYER);
    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {
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
}
