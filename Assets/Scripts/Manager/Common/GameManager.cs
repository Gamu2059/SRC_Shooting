#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

/// <summary>
/// 全てのグローバルなマネージャを管理するマネージャ。
/// </summary>
public class GameManager : GlobalSingletonMonoBehavior<GameManager>
{
    private TimerManager m_TimerManager;
    public TimerManager TimerManager => m_TimerManager;

    [SerializeField]
    private BaseSceneManager m_SceneManager;

    [SerializeField]
    private TransitionManager m_TransitionManager;

    [SerializeField]
    private AudioManager m_AudioManager;
    public AudioManager AudioManager => m_AudioManager;

    public DataManager DataManager { get; private set; }

    private PlayerData m_PlayerData;
    public PlayerData PlayerData => m_PlayerData;

    private PlayerRecordManager m_PlayerRecordManager;
    public PlayerRecordManager PlayerRecordManager => m_PlayerRecordManager;

	protected override void OnAwake()
	{
		base.OnAwake();
	}

	protected override void OnDestroyed()
	{
		base.OnDestroyed();
	}

	private void Start()
	{
		OnInitialize();
		OnStart();
	}

	private void Update()
	{
		OnUpdate();
	}

	private void LateUpdate()
	{
		OnLateUpdate();
	}

	private void FixedUpdate()
	{
		OnFixedUpdate();
	}

    public override void OnInitialize()
	{
        base.OnInitialize();

        m_TimerManager = new TimerManager();
        DataManager = new DataManager();
        m_PlayerRecordManager = new PlayerRecordManager();
        m_PlayerData = new PlayerData();

        m_TimerManager.OnInitialize();
        m_AudioManager.OnInitialize();
        m_TransitionManager.OnInitialize();
        m_SceneManager.OnInitialize();

        DataManager.OnInitialize();

        m_PlayerRecordManager.OnInitialize();

	}

	public override void OnFinalize()
	{
        m_PlayerRecordManager.OnFinalize();

        DataManager.OnFinalize();

        m_SceneManager.OnFinalize();
        m_TransitionManager.OnFinalize();
        m_AudioManager.OnFinalize();
        m_TimerManager.OnFinalize();

        base.OnFinalize();
	}

	public override void OnStart()
	{
        base.OnStart();

        m_TimerManager.OnStart();
        m_AudioManager.OnStart();
        m_TransitionManager.OnStart();
        m_SceneManager.OnStart();

		BaseSceneManager.Instance.LoadOnGameStart();
	}

	public override void OnUpdate()
	{
        base.OnUpdate();

        m_TimerManager.OnUpdate();
        m_AudioManager.OnUpdate();
        m_TransitionManager.OnUpdate();
        m_SceneManager.OnUpdate();

        DOTween.ManualUpdate(Time.deltaTime, Time.unscaledDeltaTime);
	}

	public override void OnLateUpdate()
	{
        base.OnLateUpdate();

        m_TimerManager.OnLateUpdate();
        m_AudioManager.OnLateUpdate();
        m_TransitionManager.OnLateUpdate();
        m_SceneManager.OnLateUpdate();
	}

	public override void OnFixedUpdate()
	{
        base.OnFixedUpdate();

        m_TimerManager.OnFixedUpdate();
        m_AudioManager.OnFixedUpdate();
        m_TransitionManager.OnFixedUpdate();
        m_SceneManager.OnFixedUpdate();
	}
}
