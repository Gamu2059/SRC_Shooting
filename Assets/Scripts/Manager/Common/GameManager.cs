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
    [Serializable]
    private class GameManagerParamSet
    {
        [SerializeField]
        private BattleConstantParam m_BattleConstantParam;
        public BattleConstantParam BattleConstantParam => m_BattleConstantParam;

        [SerializeField]
        private BattleAchievementParamSet m_BattleAchievementParamSet;
        public BattleAchievementParamSet BattleAchievementParamSet => m_BattleAchievementParamSet;

        [SerializeField]
        private AdxAssetParam m_AdxAssetParam;
        public AdxAssetParam AdxAssetParam => m_AdxAssetParam;
    }

    [SerializeField]
    private GameManagerParamSet m_GameManagerParamSet;

    [SerializeField]
    private BattleParamSetHolder m_BattleParamSetHolder;
    public BattleParamSetHolder BattleParamSetHolder => m_BattleParamSetHolder;

    #region Manager

    [SerializeField]
    private BaseSceneManager m_SceneManager;

    [SerializeField]
    private TransitionManager m_TransitionManager;

    public PlayerRecordManager PlayerRecordManager { get; private set; }

    #endregion

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

        AudioManager.Instance.SetAdxParam(m_GameManagerParamSet.AdxAssetParam);
        PlayerRecordManager = new PlayerRecordManager();

        TimerManager.Builder();
        AudioManager.Instance.OnInitialize();
        m_TransitionManager.OnInitialize();
        m_SceneManager.OnInitialize();
        DataManager.Builder(m_GameManagerParamSet.BattleConstantParam, m_GameManagerParamSet.BattleAchievementParamSet);
        PlayerRecordManager.OnInitialize();
	}

	public override void OnFinalize()
	{
        PlayerRecordManager.OnFinalize();
        DataManager.Instance.OnFinalize();
        m_SceneManager.OnFinalize();
        m_TransitionManager.OnFinalize();
        AudioManager.Instance.OnFinalize();
        TimerManager.Instance.OnFinalize();

        base.OnFinalize();
	}

	public override void OnStart()
	{
        base.OnStart();

        TimerManager.Instance.OnStart();
        AudioManager.Instance.OnStart();
        m_TransitionManager.OnStart();
        m_SceneManager.OnStart();

		BaseSceneManager.Instance.LoadOnGameStart();
	}

	public override void OnUpdate()
	{
        base.OnUpdate();

        TimerManager.Instance.OnUpdate();
        AudioManager.Instance.OnUpdate();
        m_TransitionManager.OnUpdate();
        m_SceneManager.OnUpdate();

        DOTween.ManualUpdate(Time.deltaTime, Time.unscaledDeltaTime);
	}

	public override void OnLateUpdate()
	{
        base.OnLateUpdate();

        TimerManager.Instance.OnLateUpdate();
        AudioManager.Instance.OnLateUpdate();
        m_TransitionManager.OnLateUpdate();
        m_SceneManager.OnLateUpdate();
	}

	public override void OnFixedUpdate()
	{
        base.OnFixedUpdate();

        TimerManager.Instance.OnFixedUpdate();
        AudioManager.Instance.OnFixedUpdate();
        m_TransitionManager.OnFixedUpdate();
        m_SceneManager.OnFixedUpdate();
	}

    #endregion
}
