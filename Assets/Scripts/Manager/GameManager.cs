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

        m_TimerManager.OnInitialize();
        m_TransitionManager.OnInitialize();
        m_SceneManager.OnInitialize();
	}

	public override void OnFinalize()
	{
        m_SceneManager.OnFinalize();
        m_TransitionManager.OnFinalize();
        m_TimerManager.OnFinalize();

        base.OnFinalize();
	}

	public override void OnStart()
	{
        base.OnStart();

        m_TimerManager.OnStart();
        m_TransitionManager.OnStart();
        m_SceneManager.OnStart();

		BaseSceneManager.Instance.LoadOnGameStart();
	}

	public override void OnUpdate()
	{
        base.OnUpdate();

        m_TimerManager.OnUpdate();
        m_TransitionManager.OnUpdate();
        m_SceneManager.OnUpdate();

        DOTween.ManualUpdate(Time.deltaTime, Time.unscaledDeltaTime);
	}

	public override void OnLateUpdate()
	{
        base.OnLateUpdate();

        m_TimerManager.OnLateUpdate();
        m_TransitionManager.OnLateUpdate();
        m_SceneManager.OnLateUpdate();
	}

	public override void OnFixedUpdate()
	{
        base.OnFixedUpdate();

        m_TimerManager.OnFixedUpdate();
        m_TransitionManager.OnFixedUpdate();
        m_SceneManager.OnFixedUpdate();
	}
}
