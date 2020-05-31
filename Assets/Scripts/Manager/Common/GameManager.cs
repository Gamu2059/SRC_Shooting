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
    #region Unity Game Cycle

    protected override void OnAwake()
	{
		base.OnAwake();
	}

	protected override void OnDestroyed()
	{
        OnFinalize();
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

    #endregion

    #region Game Cycle

    public override void OnInitialize()
	{
        base.OnInitialize();

        SaveDataManager.Load();

        SystemRecordManager.Instance.OnInitialize();
        PlayerRecordManager.Instance.OnInitialize();
        DataManager.Instance.OnInitialize();

        TimerManager.Builder();

        RewiredInputManager.Instance.OnInitialize();
        AudioManager.Instance.OnInitialize();
        TransitionManager.Instance.OnInitialize();
        BaseSceneManager.Instance.OnInitialize();
	}

	public override void OnFinalize()
	{
        BaseSceneManager.Instance.OnFinalize();
        TransitionManager.Instance.OnFinalize();
        AudioManager.Instance.OnFinalize();
        RewiredInputManager.Instance.OnFinalize();

        TimerManager.Instance.OnFinalize();

        DataManager.Instance.OnFinalize();
        PlayerRecordManager.Instance.OnFinalize();
        SystemRecordManager.Instance.OnFinalize();

        SaveDataManager.Save();

        base.OnFinalize();
	}

	public override void OnStart()
	{
        base.OnStart();

        TimerManager.Instance.OnStart();
        RewiredInputManager.Instance.OnStart();
        AudioManager.Instance.OnStart();
        TransitionManager.Instance.OnStart();
        BaseSceneManager.Instance.OnStart();

		BaseSceneManager.Instance.LoadOnGameStart();
	}

	public override void OnUpdate()
	{
        base.OnUpdate();

        TimerManager.Instance.OnUpdate();
        RewiredInputManager.Instance.OnUpdate();
        AudioManager.Instance.OnUpdate();
        TransitionManager.Instance.OnUpdate();
        BaseSceneManager.Instance.OnUpdate();
	}

	public override void OnLateUpdate()
	{
        base.OnLateUpdate();

        TimerManager.Instance.OnLateUpdate();
        RewiredInputManager.Instance.OnLateUpdate();
        AudioManager.Instance.OnLateUpdate();
        TransitionManager.Instance.OnLateUpdate();
        BaseSceneManager.Instance.OnLateUpdate();
    }

	public override void OnFixedUpdate()
	{
        base.OnFixedUpdate();

        TimerManager.Instance.OnFixedUpdate();
        RewiredInputManager.Instance.OnFixedUpdate();
        AudioManager.Instance.OnFixedUpdate();
        TransitionManager.Instance.OnFixedUpdate();
        BaseSceneManager.Instance.OnFixedUpdate();
    }

    #endregion
}
