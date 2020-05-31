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

    #region Field

    private BattleHackingParamSet m_ParamSet;
    private StateMachine<E_BATTLE_HACKING_STATE, BattleHackingManager> m_StateMachine;

    private bool m_IsDeadPlayer;
    private bool m_IsDeadBoss;
    private bool m_IsTimeout;

    #endregion

    #region Open Callback

    public Action<E_BATTLE_HACKING_STATE> ChangeStateAction { get; set; }

    #endregion

    #region Closed Callback

    private Action<E_BATTLE_STATE> RequestChangeStateBattleManagerAction { get; set; }

    #endregion

    public static BattleHackingManager Builder(BattleManager battleManager, BattleHackingParamSet param)
    {
        var manager = new BattleHackingManager();
        manager.SetParam(param);
        manager.SetCallback(battleManager);
        manager.OnInitialize();
        return manager;
    }

    private void SetParam(BattleHackingParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    private void SetCallback(BattleManager manager)
    {
        RequestChangeStateBattleManagerAction += manager.RequestChangeState;
    }

    #region Game Cycyle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_HACKING_STATE, BattleHackingManager>();
        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.START, this, new StartState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.TO_REAL, this, new ToRealState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.STAY_REAL, this, new StayRealState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.FROM_REAL, this, new FromRealState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.GAME, this, new GameState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.GAME_CLEAR, this, new GameClearState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.GAME_OVER, this, new GameOverState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_HACKING_STATE.END, this, new EndState()));

        BattleHackingTimerManager.Builder();
        BattleHackingPlayerManager.Builder(this, m_ParamSet.PlayerManagerParamSet);
        BattleHackingEnemyManager.Builder(this, m_ParamSet.EnemyManagerParamSet);
        BattleHackingBulletManager.Builder(this, m_ParamSet.BulletManagerParamSet);
        BattleHackingEffectManager.Builder();
        BattleHackingCollisionManager.Builder();
        BattleHackingCameraManager.Instance.OnInitialize();
        BattleHackingUiManager.Instance.OnInitialize();
    }

    public override void OnFinalize()
    {
        RequestChangeStateBattleManagerAction = null;
        ChangeStateAction = null;

        BattleHackingUiManager.Instance.OnFinalize();
        BattleHackingCameraManager.Instance.OnFinalize();
        BattleHackingCollisionManager.Instance.OnFinalize();
        BattleHackingEffectManager.Instance.OnFinalize();
        BattleHackingBulletManager.Instance.OnFinalize();
        BattleHackingEnemyManager.Instance.OnFinalize();
        BattleHackingPlayerManager.Instance.OnFinalize();
        BattleHackingTimerManager.Instance.OnFinalize();
        m_StateMachine.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_StateMachine.OnUpdate();

        if (Input.GetKeyDown(KeyCode.K))
        {
            BattleHackingEnemyManager.Instance.KillAllBoss();
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

    /// <summary>
    /// BattleHackingManagerのステートを変更する。
    /// </summary>
    public void RequestChangeState(E_BATTLE_HACKING_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    /// <summary>
    /// プレイヤーが撃破された時に呼び出される。
    /// </summary>
    public void DeadPlayer()
    {
        m_IsDeadPlayer = true;
    }

    /// <summary>
    /// ボスが撃破された時に呼び出される。
    /// </summary>
    public void DeadBoss()
    {
        m_IsDeadBoss = true;
    }

    /// <summary>
    /// 制限時間に間に合わなかった時に呼び出される。
    /// </summary>
    public void Timeout()
    {
        m_IsTimeout = true;
    }
}
