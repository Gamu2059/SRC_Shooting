using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルのハッキングモードの処理を管理する。
/// </summary>
public class BattleHackingManager : ControllableObject
{
    #region Field

    private BattleHackingParamSet m_ParamSet;

    private StateMachine<E_BATTLE_HACKING_STATE> m_StateMachine;

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

        m_StateMachine.AddState(new State<E_BATTLE_HACKING_STATE>(E_BATTLE_HACKING_STATE.BEGIN_GAME)
        {
            OnStart = StartOnBeginGame,
            OnUpdate = UpdateOnBeginGame,
            OnLateUpdate = LateUpdateOnBeginGame,
            OnFixedUpdate = FixedUpdateOnBeginGame,
            OnEnd = EndOnBeginGame,
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

        RequestChangeState(E_BATTLE_HACKING_STATE.START);
    }

    public override void OnFinalize()
    {
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

    #region Begin Game State

    private void StartOnBeginGame()
    {
        RequestChangeState(E_BATTLE_HACKING_STATE.GAME);
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
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 5f);
        timer.SetTimeoutCallBack(() =>
        {
            RequestChangeState(E_BATTLE_HACKING_STATE.GAME_CLEAR);
        });
        TimerManager.Instance.RegistTimer(timer);
    }

    private void UpdateOnGame()
    {
    }

    private void LateUpdateOnGame()
    {
    }

    private void FixedUpdateOnGame()
    {
    }

    private void EndOnGame()
    {
    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {
        RequestChangeState(E_BATTLE_HACKING_STATE.END_GAME);
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
        RequestChangeState(E_BATTLE_HACKING_STATE.END_GAME);
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

    #region End Game State

    private void StartOnEndGame()
    {
        BattleManager.Instance.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_REAL);
    }

    private void UpdateOnEndGame()
    {

    }

    private void LateUpdateOnEndGame()
    {

    }

    private void FixedUpdateOnEndGame()
    {

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
