using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の振る舞いの制御を行う。
/// </summary>
public class EnemyCharaManager : SingletonMonoBehavior<EnemyCharaManager>
{
	/// <summary>
	/// ボスを含め、全ての敵オブジェクトを保持する。
	/// </summary>
	[SerializeField]
	private List<EnemyController> m_Controllers;

	/// <summary>
	/// ボスのオブジェクトのみを保持する。
	/// </summary>
	[SerializeField]
	private List<EnemyController> m_BossControllers;

	/// <summary>
	/// 全ての敵オブジェクトを取得する。
	/// </summary>
	public List<EnemyController> GetControllers()
	{
		return m_Controllers;
	}

	/// <summary>
	/// ステージ上の全てのボス敵のオブジェクトを取得する。
	/// </summary>
	public List<EnemyController> GetBossControllers()
	{
		return m_BossControllers;
	}

	protected override void OnAwake()
	{
		base.OnAwake();

		m_Controllers = new List<EnemyController>();
		m_BossControllers = new List<EnemyController>();
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

		// シーンが変わる時は中身を全て破棄する
		DestroyAllEnemy();
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

	/// <summary>
	/// 敵キャラを登録する。
	/// いずれこのメソッドは外部から参照できなくする予定です。
	/// </summary>
	public EnemyController RegistEnemy( EnemyController controller )
	{
		if( controller == null || m_Controllers.Contains( controller ) )
		{
			return null;
		}

		StageManager.Instance.AddMoveObjectHolder( controller.transform );
		m_Controllers.Add( controller );
		controller.OnInitialize();
		return controller;
	}

	/// <summary>
	/// 敵キャラのプレハブから敵キャラを新規作成する。
	/// </summary>
	public EnemyController CreateEnemy( EnemyController enemyPrefab )
	{
		if( enemyPrefab == null )
		{
			return null;
		}

		EnemyController controller = Instantiate( enemyPrefab );
		return RegistEnemy( controller );
	}

	/// <summary>
	/// 敵キャラを破棄する。
	/// </summary>
	public void DestroyEnemy( EnemyController controller )
	{
		if( controller == null || !m_Controllers.Contains( controller ) )
		{
			return;
		}

		controller.OnFinalize();
		m_Controllers.Remove( controller );
		Destroy( controller.gameObject );
	}

	/// <summary>
	/// 全ての敵キャラを破棄する。
	/// </summary>
	public void DestroyAllEnemy()
	{
		foreach( var controller in m_Controllers )
		{
			if( controller == null )
			{
				continue;
			}

			controller.OnFinalize();
			Destroy( controller.gameObject );
		}

		m_Controllers.Clear();
		m_BossControllers.Clear();
	}
}
