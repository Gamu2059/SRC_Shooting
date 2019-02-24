using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の振る舞いの制御を行う。
/// </summary>
public class EnemyCharaManager : GlobalSingletonMonoBehavior<EnemyCharaManager>
{

	[SerializeField]
	private EnemyController[] m_EnemyControllers;

	public EnemyController[] GetControllers()
	{
		return m_EnemyControllers;
	}

	private void Update()
	{
		if( m_EnemyControllers != null )
		{
			foreach( var enemy in m_EnemyControllers )
			{
				enemy.OnUpdate();
			}
		}
	}
}
