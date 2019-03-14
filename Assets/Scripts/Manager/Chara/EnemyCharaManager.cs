using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の振る舞いの制御を行う。
/// </summary>
public class EnemyCharaManager : SingletonMonoBehavior<EnemyCharaManager>
{

	[SerializeField]
	private List<EnemyController> m_Controllers;

	public List<EnemyController> GetControllers()
	{
		return m_Controllers;
	}

	protected override void OnAwake()
	{
		base.OnAwake();

		m_Controllers = new List<EnemyController>();
	}

	protected override void OnDestroyed()
	{
		base.OnDestroyed();
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnFinalize()
	{
		base.OnFinalize();
		m_Controllers.Clear();
	}

	public override void OnUpdate()
	{
		if( m_Controllers != null )
		{
			foreach( var enemy in m_Controllers )
			{
				enemy.OnUpdate();
			}
		}
	}

	public void RegistChara( EnemyController controller )
	{
		if( controller == null || m_Controllers.Contains( controller ) )
		{
			return;
		}

		controller.OnInitialize();
		m_Controllers.Add( controller );
	}

	public void DestroyChara( EnemyController controller )
	{
		if( controller == null || !m_Controllers.Contains( controller ) )
		{
			return;
		}

		controller.OnFinalize();
		m_Controllers.Remove( controller );
		Destroy( controller.gameObject );
	}
}
