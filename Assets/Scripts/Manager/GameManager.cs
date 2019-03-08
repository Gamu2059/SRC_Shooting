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
	[SerializeField, Tooltip( "起動した瞬間にいたシーンから始めるかどうか" )]
	private bool m_IsStartFromBeginningScene;

	[SerializeField, Tooltip( "PreLaunchシーンから最初に遷移するシーン" )]
	private BaseSceneManager.E_SCENE m_StartScene;

	/// <summary>
	/// GameManagerでサイクルを管理するマネージャのリスト。
	/// </summary>
	private List<IControllableGameCycle> m_Managers;


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

	public override void OnInitialize()
	{
		m_Managers = new List<IControllableGameCycle>();
		m_Managers.Add( gameObject.AddComponent<BaseSceneManager>() );
		m_Managers.Add( gameObject.AddComponent<PlayerCharaManager>() );
		m_Managers.Add( gameObject.AddComponent<EnemyCharaManager>() );
		m_Managers.Add( gameObject.AddComponent<BulletManager>() );
		m_Managers.Add( gameObject.AddComponent<CollisionManager>() );

		m_Managers.ForEach( ( m ) => m.OnInitialize() );
	}

	public override void OnFinalize()
	{
		m_Managers.ForEach( ( m ) => m.OnFinalize() );

		m_Managers.Clear();
		m_Managers = null;
	}

	public override void OnStart()
	{
		m_Managers.ForEach( ( m ) => m.OnStart() );

		if( m_IsStartFromBeginningScene )
		{
			BaseSceneManager.Instance.LoadBeginScene();
		}
		else
		{
			BaseSceneManager.Instance.LoadScene( m_StartScene );
		}
	}

	public override void OnUpdate()
	{
		m_Managers.ForEach( ( m ) => m.OnUpdate() );
	}

	public override void OnLateUpdate()
	{
		m_Managers.ForEach( ( m ) => m.OnLateUpdate() );
	}
}
