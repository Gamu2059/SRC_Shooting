﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

/// <summary>
/// バトル画面のマネージャーを管理する上位マネージャ。
/// メイン画面とコマンド画面の切り替えを主に管理する。
/// </summary>
public class BattleManager : SingletonMonoBehavior<BattleManager>
{
    #region Field Inspector

    [SerializeField]
    private BattleParamSet m_BattleParamSet;

    [SerializeField]
    private BattleRealStageManager m_BattleRealStageManager;
    public BattleRealStageManager BattleRealStageManager => m_BattleRealStageManager;

    [SerializeField]
    private BattleRealUiManager m_BattleRealUiManager;
    public BattleRealUiManager BattleRealUiManager => m_BattleRealUiManager;

    [SerializeField]
    private BattleHackingStageManager m_BattleHackingStageManager;
    public BattleHackingStageManager BattleHackingStageManager => m_BattleHackingStageManager;

    [SerializeField]
    private BattleHackingUiManager m_BattleHackingUiManager;
    public BattleHackingUiManager BattleHackingUiManager => m_BattleHackingUiManager;

    [SerializeField]
    private VideoPlayer m_VideoPlayer;

    [SerializeField]
    private bool m_IsStartHackingMode;

    [SerializeField]
    public bool m_PlayerNotDead;

    #endregion

    #region Field

    private StateMachine<E_BATTLE_STATE> m_StateMachine;

    public BattleRealManager RealManager { get; private set; }
    public BattleHackingManager HackingManager { get; private set; }

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_STATE>();

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.START)
        {
            OnStart = StartOnStart,
            OnUpdate = UpdateOnStart,
            OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.REAL_MODE)
        {
            OnStart = StartOnRealMode,
            OnUpdate = UpdateOnRealMode,
            OnEnd = EndOnRealMode,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.HACKING_MODE)
        {
            OnStart = StartOnHackingMode,
            OnUpdate = UpdateOnHackingMode,
            OnEnd = EndOnHackingMode,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.TRANSITION_TO_REAL)
        {
            OnStart = StartOnTransitionToReal,
            OnUpdate = UpdateOnTransitionToReal,
            OnEnd = EndOnTransitionToReal,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.TRANSITION_TO_HACKING)
        {
            OnStart = StartOnTransitionToHacking,
            OnUpdate = UpdateOnTransitionToHacking,
            OnEnd = EndOnTransitionToHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.GAME_CLEAR)
        {
            OnStart = StartOnGameClear,
            OnUpdate = UpdateOnGameClear,
            OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.GAME_OVER)
        {
            OnStart = StartOnGameOver,
            OnUpdate = UpdateOnGameOver,
            OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.END)
        {
            OnStart = StartOnEnd,
            OnUpdate = UpdateOnEnd,
            OnEnd = EndOnEnd,
        });

        m_BattleRealStageManager.OnInitialize();

        RealManager = new BattleRealManager(m_BattleParamSet.BattleRealParamSet);
        HackingManager = new BattleHackingManager(m_BattleParamSet.BattleHackingParamSet);

        RealManager.OnInitialize();
        HackingManager.OnInitialize();

        RequestChangeState(E_BATTLE_STATE.START);
    }

    public override void OnFinalize()
    {
        HackingManager.OnFinalize();
        RealManager.OnFinalize();

        m_BattleRealStageManager.OnFinalize();

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
        RealManager.OnStart();
        HackingManager.OnStart();

        m_BattleRealStageManager.gameObject.SetActive(true);
        m_BattleHackingStageManager.gameObject.SetActive(false);
        m_VideoPlayer.gameObject.SetActive(false);

        var audio = AudioManager.Instance;
        audio.SetPrimaryBgmVolume(0);
        audio.SetSecondaryBgmVolume(0);
        audio.PlayPrimaryBgm(m_BattleParamSet.RealModeBgm);
        audio.PlaySecondaryBgm(m_BattleParamSet.HackingModeBgm);

        RequestChangeState(E_BATTLE_STATE.REAL_MODE);
    }

    private void UpdateOnStart()
    {

    }

    private void EndOnStart()
    {

    }

    #endregion

    #region Real Mode State

    private void StartOnRealMode()
    {
        var audio = AudioManager.Instance;
        audio.SetPrimaryBgmVolume(1);
        audio.SetSecondaryBgmVolume(0);

        m_BattleRealUiManager.SetAlpha(1);
        m_BattleHackingUiManager.SetAlpha(0);
    }

    private void UpdateOnRealMode()
    {
        RealManager.OnUpdate();

        if (m_IsStartHackingMode)
        {
            m_IsStartHackingMode = false;
            RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_HACKING);
        }
    }

    private void EndOnRealMode()
    {

    }

    #endregion

    #region Hacking Mode State

    private void StartOnHackingMode()
    {
        var audio = AudioManager.Instance;
        audio.SetPrimaryBgmVolume(0);
        audio.SetSecondaryBgmVolume(1);

        m_BattleHackingUiManager.SetAlpha(1);
        m_BattleRealUiManager.SetAlpha(0);
    }

    private void UpdateOnHackingMode()
    {
        HackingManager.OnUpdate();
    }

    private void EndOnHackingMode()
    {

    }

    #endregion

    #region Transition To Hacking State

    private void StartOnTransitionToHacking()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.STAY_HACKING);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.BEGIN_GAME);

        m_BattleHackingStageManager.gameObject.SetActive(true);

        m_VideoPlayer.clip = m_BattleParamSet.TransitionToHackingMovie;
        m_VideoPlayer.Play();
        m_VideoPlayer.gameObject.SetActive(true);

        AudioManager.Instance.PlaySe(m_BattleParamSet.TransitionToHackingSe);
    }

    private void UpdateOnTransitionToHacking()
    {
        if (m_VideoPlayer.isPlaying)
        {
            var movieTime = m_BattleParamSet.TransitionToHackingMovie.length;
            var normalizedTime = (float)(m_VideoPlayer.time / movieTime);

            var fadeOutValue = m_BattleParamSet.FadeOutParam.Evaluate(normalizedTime);
            var fadeInValue = m_BattleParamSet.FadeInParam.Evaluate(normalizedTime);

            m_BattleRealUiManager.SetAlpha(fadeOutValue);
            m_BattleHackingUiManager.SetAlpha(fadeInValue);

            var audio = AudioManager.Instance;
            audio.SetPrimaryBgmVolume(fadeOutValue);
            audio.SetSecondaryBgmVolume(fadeInValue);
        }
        else
        {
            RequestChangeState(E_BATTLE_STATE.HACKING_MODE);
        }
    }

    private void EndOnTransitionToHacking()
    {
        m_BattleRealStageManager.gameObject.SetActive(false);
        m_VideoPlayer.gameObject.SetActive(false);
        m_VideoPlayer.Stop();
    }

    #endregion

    #region Transition To Real State

    private void StartOnTransitionToReal()
    {
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);

        m_BattleRealStageManager.gameObject.SetActive(true);

        m_VideoPlayer.clip = m_BattleParamSet.TransitionToRealMovie;
        m_VideoPlayer.Play();
        m_VideoPlayer.gameObject.SetActive(true);

        AudioManager.Instance.PlaySe(m_BattleParamSet.TransitionToRealSe);
    }

    private void UpdateOnTransitionToReal()
    {
        if (m_VideoPlayer.isPlaying)
        {
            var movieTime = m_BattleParamSet.TransitionToRealMovie.length;
            var normalizedTime = (float)(m_VideoPlayer.time / movieTime);

            var fadeOutValue = m_BattleParamSet.FadeOutParam.Evaluate(normalizedTime);
            var fadeInValue = m_BattleParamSet.FadeInParam.Evaluate(normalizedTime);

            m_BattleHackingUiManager.SetAlpha(fadeOutValue);
            m_BattleRealUiManager.SetAlpha(fadeInValue);

            var audio = AudioManager.Instance;
            audio.SetSecondaryBgmVolume(fadeOutValue);
            audio.SetPrimaryBgmVolume(fadeInValue);
        }
        else
        {
            RequestChangeState(E_BATTLE_STATE.REAL_MODE);
        }
    }

    private void EndOnTransitionToReal()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.GAME);

        m_BattleHackingStageManager.gameObject.SetActive(false);
        m_VideoPlayer.gameObject.SetActive(false);
        m_VideoPlayer.Stop();
    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {

    }

    private void UpdateOnGameClear()
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

    private void EndOnEnd()
    {

    }

    #endregion

    public void RequestChangeState(E_BATTLE_STATE state)
    {
        if (m_StateMachine == null)
        {
            return;
        }

        m_StateMachine.Goto(state);
    }


    /// <summary>
    /// ゲームを開始する。
    /// </summary>
    public void GameStart()
    {

    }

    /// <summary>
    /// ゲームオーバーにする。
    /// </summary>
	public void GameOver()
    {
        //DetachBattleMainInputAction();
        //BattleMainUiManager.Instance.ShowGameOver();
        //BattleMainAudioManager.Instance.StopAllBGM();
        //var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        //{
        //    BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
        //});
        //TimerManager.Instance.RegistTimer(timer);
    }

    /// <summary>
    /// ゲームクリアにする。
    /// </summary>
	public void GameClear()
    {
        //DetachBattleMainInputAction();

        //BattleMainUiManager.Instance.ShowGameClear();
        //var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        //{
        //    BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
        //});
        //TimerManager.Instance.RegistTimer(timer);
    }
}
