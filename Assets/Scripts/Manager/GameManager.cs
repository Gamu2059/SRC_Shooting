using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 全てのグローバルなマネージャを管理するマネージャ。
/// </summary>
public class GameManager : GlobalSingletonMonoBehavior<GameManager>
{
	/// <summary>
	/// GameManagerでサイクルを管理するマネージャのリスト。
	/// </summary>
	[SerializeField]
	private List<ControllableMonoBehavior> m_Managers;


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
		m_Managers.ForEach( ( m ) => m.OnInitialize() );
	}

	public override void OnFinalize()
	{
		m_Managers.ForEach( ( m ) => m.OnFinalize() );
	}

	public override void OnStart()
	{
		m_Managers.ForEach( ( m ) => m.OnStart() );

		BaseSceneManager.Instance.LoadOnGameStart();
	}

	public override void OnUpdate()
	{
		m_Managers.ForEach( ( m ) => m.OnUpdate() );
	}

	public override void OnLateUpdate()
	{
		m_Managers.ForEach( ( m ) => m.OnLateUpdate() );
	}

	public override void OnFixedUpdate()
	{
		m_Managers.ForEach( ( m ) => m.OnFixedUpdate() );
	}
}
