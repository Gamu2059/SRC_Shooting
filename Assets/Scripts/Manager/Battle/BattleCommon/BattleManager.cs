using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

using E_AISAC_TYPE = AudioManager.E_AISAC_TYPE;

/// <summary>
/// バトル画面のマネージャーを管理する上位マネージャ。
/// メイン画面とコマンド画面の切り替えを主に管理する。
/// </summary>
public class BattleManager : SingletonMonoBehavior<BattleManager>
{
    #region Field Inspector

    [Header("ParamSet")]

    [SerializeField]
    private BattleParamSet m_ParamSet;
    public BattleParamSet ParamSet => m_ParamSet;

    [Header("StageManager")]

    [SerializeField]
    private BattleRealStageManager m_BattleRealStageManager;
    public BattleRealStageManager BattleRealStageManager => m_BattleRealStageManager;

    [SerializeField]
    private BattleHackingStageManager m_BattleHackingStageManager;
    public BattleHackingStageManager BattleHackingStageManager => m_BattleHackingStageManager;

    [Header("UiManager")]

    [SerializeField]
    private BattleRealUiManager m_BattleRealUiManager;
    public BattleRealUiManager BattleRealUiManager => m_BattleRealUiManager;

    [SerializeField]
    private BattleHackingUiManager m_BattleHackingUiManager;
    public BattleHackingUiManager BattleHackingUiManager => m_BattleHackingUiManager;

    [Header("PlayableManager")]

    [SerializeField]
    private BattleRealPlayableManager m_BattleRealPlayableManager;
    public BattleRealPlayableManager BattleRealPlayableManager => m_BattleRealPlayableManager;

    [Header("Camera")]

    [SerializeField]
    private BattleRealCameraController m_BattleRealFrontCamera;
    public BattleRealCameraController BattleRealFrontCamera => m_BattleRealFrontCamera;

    [SerializeField]
    private BattleRealCameraController m_BattleRealBackCamera;
    public BattleRealCameraController BattleRealBackCamera => m_BattleRealBackCamera;

    [Header("Video")]

    [SerializeField]
    private VideoPlayer m_VideoPlayer;

    [Header("Debug")]

    [SerializeField]
    private bool m_IsStartHackingMode;

    [SerializeField]
    public bool m_PlayerNotDead;

    [SerializeField]
    public bool m_IsDrawColliderArea;

    [SerializeField]
    public bool m_IsDrawOutSideColliderArea;

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
            m_OnStart = StartOnStart,
            m_OnUpdate = UpdateOnStart,
            m_OnLateUpdate = LateUpdateOnStart,
            m_OnFixedUpdate = FixedUpdateOnStart,
            m_OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.REAL_MODE)
        {
            m_OnStart = StartOnRealMode,
            m_OnUpdate = UpdateOnRealMode,
            m_OnLateUpdate = LateUpdateOnRealMode,
            m_OnFixedUpdate = FixedUpdateOnRealMode,
            m_OnEnd = EndOnRealMode,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.HACKING_MODE)
        {
            m_OnStart = StartOnHackingMode,
            m_OnUpdate = UpdateOnHackingMode,
            m_OnLateUpdate = LateUpdateOnHackingMode,
            m_OnFixedUpdate = FixedUpdateOnHackingMode,
            m_OnEnd = EndOnHackingMode,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.TRANSITION_TO_REAL)
        {
            m_OnStart = StartOnTransitionToReal,
            m_OnUpdate = UpdateOnTransitionToReal,
            m_OnLateUpdate = LateUpdateOnTransitionToReal,
            m_OnFixedUpdate = FixedUpdateOnTransitionToReal,
            m_OnEnd = EndOnTransitionToReal,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.TRANSITION_TO_HACKING)
        {
            m_OnStart = StartOnTransitionToHacking,
            m_OnUpdate = UpdateOnTransitionToHacking,
            m_OnLateUpdate = LateUpdateOnTransitionToHacking,
            m_OnFixedUpdate = FixedUpdateOnTransitionToHacking,
            m_OnEnd = EndOnTransitionToHacking,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.GAME_CLEAR)
        {
            m_OnStart = StartOnGameClear,
            m_OnUpdate = UpdateOnGameClear,
            m_OnLateUpdate = LateUpdateOnGameClear,
            m_OnFixedUpdate = FixedUpdateOnGameClear,
            m_OnEnd = EndOnGameClear,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.GAME_OVER)
        {
            m_OnStart = StartOnGameOver,
            m_OnUpdate = UpdateOnGameOver,
            m_OnLateUpdate = LateUpdateOnGameOver,
            m_OnFixedUpdate = FixedUpdateOnGameOver,
            m_OnEnd = EndOnGameOver,
        });

        m_StateMachine.AddState(new State<E_BATTLE_STATE>(E_BATTLE_STATE.END)
        {
            m_OnStart = StartOnEnd,
            m_OnUpdate = UpdateOnEnd,
            m_OnLateUpdate = LateUpdateOnEnd,
            m_OnFixedUpdate = FixedUpdateOnEnd,
            m_OnEnd = EndOnEnd,
        });

        m_BattleRealStageManager.OnInitialize();
        m_BattleHackingStageManager.OnInitialize();

        RealManager = new BattleRealManager(m_ParamSet.BattleRealParamSet);
        HackingManager = new BattleHackingManager(m_ParamSet.BattleHackingParamSet);

        RealManager.OnInitialize();
        HackingManager.OnInitialize();

        m_BattleRealPlayableManager.OnInitialize();

        m_BattleRealUiManager.OnInitialize();
        m_BattleHackingUiManager.OnInitialize();

        RequestChangeState(E_BATTLE_STATE.START);
    }

    public override void OnFinalize()
    {
        m_BattleHackingUiManager.OnFinalize();
        m_BattleRealUiManager.OnFinalize();

        m_BattleRealPlayableManager.OnFinalize();

        HackingManager.OnFinalize();
        RealManager.OnFinalize();

        m_BattleHackingStageManager.OnFinalize();
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

        m_BattleRealUiManager.SetEnableGameClear(false);
        m_BattleRealUiManager.SetEnableGameOver(false);

        RequestChangeState(E_BATTLE_STATE.REAL_MODE);

        var pData = GameManager.Instance.PlayerData;
        pData.ResetHackingSucceedCount();
        pData.ResetBestScore();
        pData.ResetPlayerLast();
    }

    private void UpdateOnStart()
    {
        RealManager.OnUpdate();
        HackingManager.OnUpdate();
    }

    private void LateUpdateOnStart()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();
    }

    private void FixedUpdateOnStart()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();
    }

    private void EndOnStart()
    {

    }

    #endregion

    #region Real Mode State

    private void StartOnRealMode()
    {
        var audio = AudioManager.Instance;
        audio.SetBgmAisac(E_AISAC_TYPE.BGM_FADE_CONTROL, 0);

        m_BattleRealUiManager.SetAlpha(1);
        m_BattleHackingUiManager.SetAlpha(0);
    }

    private void UpdateOnRealMode()
    {
        if (m_IsStartHackingMode)
        {
            m_IsStartHackingMode = false;
            RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_HACKING);
        }

        RealManager.OnUpdate();
        HackingManager.OnUpdate();
    }

    private void LateUpdateOnRealMode()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();
    }

    private void FixedUpdateOnRealMode()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();
    }

    private void EndOnRealMode()
    {

    }

    #endregion

    #region Hacking Mode State

    private void StartOnHackingMode()
    {
        var audio = AudioManager.Instance;
        audio.SetBgmAisac(E_AISAC_TYPE.BGM_FADE_CONTROL, 1);

        m_BattleHackingUiManager.SetAlpha(1);
        m_BattleRealUiManager.SetAlpha(0);
    }

    private void UpdateOnHackingMode()
    {
        RealManager.OnUpdate();
        HackingManager.OnUpdate();
    }

    private void LateUpdateOnHackingMode()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();
    }

    private void FixedUpdateOnHackingMode()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();
    }

    private void EndOnHackingMode()
    {

    }

    #endregion

    #region Transition To Hacking State

    private void StartOnTransitionToHacking()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.TRANSITION_TO_HACKING);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.TRANSITION_TO_HACKING);

        m_BattleHackingStageManager.gameObject.SetActive(true);

        m_VideoPlayer.clip = m_ParamSet.TransitionToHackingMovie;
        m_VideoPlayer.Play();
        m_VideoPlayer.gameObject.SetActive(true);

        var audio = AudioManager.Instance;
        audio.PlaySe(AudioManager.E_SE_GROUP.GLOBAL, m_ParamSet.TransitionToHackingSeName);
    }

    private void UpdateOnTransitionToHacking()
    {
        if (m_VideoPlayer.isPlaying)
        {
            var movieTime = m_ParamSet.TransitionToHackingMovie.length;
            var normalizedTime = (float)(m_VideoPlayer.time / movieTime);

            var fadeOutVideoValue = m_ParamSet.FadeOutVideoParam.Evaluate(normalizedTime);
            var fadeInVideoValue = m_ParamSet.FadeInVideoParam.Evaluate(normalizedTime);

            m_BattleRealUiManager.SetAlpha(fadeOutVideoValue);
            m_BattleHackingUiManager.SetAlpha(fadeInVideoValue);

            var audio = AudioManager.Instance;
            audio.SetBgmAisac(E_AISAC_TYPE.BGM_FADE_CONTROL, normalizedTime);
        }
        else
        {
            RequestChangeState(E_BATTLE_STATE.HACKING_MODE);
        }

        RealManager.OnUpdate();
        HackingManager.OnUpdate();
    }

    private void LateUpdateOnTransitionToHacking()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();
    }

    private void FixedUpdateOnTransitionToHacking()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();
    }

    private void EndOnTransitionToHacking()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.STAY_HACKING);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.GAME);

        m_BattleRealStageManager.gameObject.SetActive(false);
        m_VideoPlayer.gameObject.SetActive(false);
        m_VideoPlayer.Stop();
    }

    #endregion

    #region Transition To Real State

    private void StartOnTransitionToReal()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.TRANSITION_TO_REAL);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.TRANSITION_TO_REAL);

        m_BattleRealStageManager.gameObject.SetActive(true);

        m_VideoPlayer.clip = m_ParamSet.TransitionToRealMovie;
        m_VideoPlayer.Play();
        m_VideoPlayer.gameObject.SetActive(true);

        var audio = AudioManager.Instance;
        audio.PlaySe(AudioManager.E_SE_GROUP.GLOBAL, m_ParamSet.TransitionToRealSeName);
    }

    private void UpdateOnTransitionToReal()
    {
        if (m_VideoPlayer.isPlaying)
        {
            var movieTime = m_ParamSet.TransitionToRealMovie.length;
            var normalizedTime = (float)(m_VideoPlayer.time / movieTime);

            var fadeOutVideoValue = m_ParamSet.FadeOutVideoParam.Evaluate(normalizedTime);
            var fadeInVideoValue = m_ParamSet.FadeInVideoParam.Evaluate(normalizedTime);

            m_BattleHackingUiManager.SetAlpha(fadeOutVideoValue);
            m_BattleRealUiManager.SetAlpha(fadeInVideoValue);

            var audio = AudioManager.Instance;
            // ハッキングモードから戻す時は逆にしなければならない
            audio.SetBgmAisac(E_AISAC_TYPE.BGM_FADE_CONTROL, 1 - normalizedTime);
        }
        else
        {
            RequestChangeState(E_BATTLE_STATE.REAL_MODE);
        }

        RealManager.OnUpdate();
        HackingManager.OnUpdate();
    }

    private void LateUpdateOnTransitionToReal()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();
    }

    private void FixedUpdateOnTransitionToReal()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();
    }

    private void EndOnTransitionToReal()
    {
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.GAME);

        m_BattleHackingStageManager.gameObject.SetActive(false);
        m_VideoPlayer.gameObject.SetActive(false);
        m_VideoPlayer.Stop();
    }

    #endregion

    #region Game Clear State

    private void StartOnGameClear()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.GAME_OVER);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);

        m_BattleRealUiManager.SetEnableGameClear(true);

        AudioManager.Instance.StopBgm();
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        {
            RequestChangeState(E_BATTLE_STATE.END);
        });
        TimerManager.Instance.RegistTimer(timer);
    }

    private void UpdateOnGameClear()
    {
        RealManager.OnUpdate();
        HackingManager.OnUpdate();
    }

    private void LateUpdateOnGameClear()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameClear()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();
    }

    private void EndOnGameClear()
    {

    }

    #endregion

    #region Game Over State

    private void StartOnGameOver()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.GAME_OVER);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);

        m_BattleRealUiManager.SetEnableGameOver(true);

        AudioManager.Instance.StopBgm();
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        {
            RequestChangeState(E_BATTLE_STATE.END);
        });
        TimerManager.Instance.RegistTimer(timer);
    }

    private void UpdateOnGameOver()
    {
        RealManager.OnUpdate();
        HackingManager.OnUpdate();
    }

    private void LateUpdateOnGameOver()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();
    }

    private void FixedUpdateOnGameOver()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();
    }

    private void EndOnGameOver()
    {

    }

    #endregion

    #region End State

    private void StartOnEnd()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.END);
        HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.END);

        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        {
            BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
        });
        TimerManager.Instance.RegistTimer(timer);

        //Debug.Log("BackToTitle");
        GameManager.Instance.PlayerRecordManager.AddRecord(new PlayerRecord(BattleRealPlayerManager.Instance.GetCurrentScore().Value, 1, System.DateTime.Now));
        GameManager.Instance.PlayerRecordManager.ShowRecord();
    }

    private void UpdateOnEnd()
    {
        RealManager.OnUpdate();
        HackingManager.OnUpdate();
    }

    private void LateUpdateOnEnd()
    {
        RealManager.OnLateUpdate();
        HackingManager.OnLateUpdate();
    }

    private void FixedUpdateOnEnd()
    {
        RealManager.OnFixedUpdate();
        HackingManager.OnFixedUpdate();
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

    public void GotoBossEvent()
    {
        RealManager.RequestChangeState(E_BATTLE_REAL_STATE.BEFORE_BOSS_BATTLE_PERFORMANCE);
    }

    /// <summary>
    /// ゲームオーバーにする。
    /// </summary>
	public void GameOver()
    {
        RequestChangeState(E_BATTLE_STATE.GAME_OVER);
    }

    /// <summary>
    /// ゲームクリアにする。
    /// </summary>
	public void GameClear()
    {
        RequestChangeState(E_BATTLE_STATE.GAME_CLEAR);
    }
}
