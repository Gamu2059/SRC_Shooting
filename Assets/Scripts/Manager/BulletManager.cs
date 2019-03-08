using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 全ての弾の制御を管理する。
/// </summary>
public class BulletManager : SingletonMonoBehavior<BulletManager>
{
	/// <summary>
	/// 全ての弾を保持するトランスフォーム。
	/// </summary>
	[SerializeField]
	private Transform m_BulletHolder;

	/// <summary>
	/// STANDBY状態の弾を保持するリスト。
	/// </summary>
	[SerializeField]
	private LinkedList<Bullet> m_StandbyBullets;

	/// <summary>
	/// UPDATE状態の弾を保持するリスト。
	/// </summary>
	[SerializeField]
	private LinkedList<Bullet> m_UpdateBullets;

	/// <summary>
	/// POOL状態の弾を保持するリスト。
	/// </summary>
	[SerializeField]
	private LinkedList<Bullet> m_PoolBullets;

	/// <summary>
	/// UPDATE状態に遷移する弾のリスト。
	/// </summary>
	private LinkedList<Bullet> m_GotoUpdateBullets;

	/// <summary>
	/// POOL状態に遷移する弾のリスト。
	/// </summary>
	private LinkedList<Bullet> m_GotoPoolBullets;

	/// <summary>
	/// STANDBY状態の弾を保持するリストを取得する。
	/// </summary>
	public LinkedList<Bullet> GetStandbyBullets()
	{
		return m_StandbyBullets;
	}

	/// <summary>
	/// UPDATE状態の弾を保持するリストを取得する。
	/// </summary>
	public LinkedList<Bullet> GetUpdateBullets()
	{
		return m_UpdateBullets;
	}

	/// <summary>
	/// POOL状態の弾を保持するリストを取得する。
	/// </summary>
	public LinkedList<Bullet> GetPoolBullets()
	{
		return m_PoolBullets;
	}



	public override void OnInitialize()
	{
		m_StandbyBullets = new LinkedList<Bullet>();
		m_UpdateBullets = new LinkedList<Bullet>();
		m_PoolBullets = new LinkedList<Bullet>();
		m_GotoUpdateBullets = new LinkedList<Bullet>();
		m_GotoPoolBullets = new LinkedList<Bullet>();
	}

	public override void OnFinalize()
	{
		m_StandbyBullets.Clear();
		m_StandbyBullets = null;
		m_UpdateBullets.Clear();
		m_UpdateBullets = null;
		m_PoolBullets.Clear();
		m_PoolBullets = null;
	}

	public override void OnStart()
	{
	}

	public override void OnUpdate()
	{
		// Start処理
		foreach( var bullet in m_StandbyBullets )
		{
			if( bullet == null )
			{
				continue;
			}

			bullet.OnStart();
			m_GotoUpdateBullets.AddLast( bullet );
		}

		GotoUpdateFromStandby();

		// Update処理
		foreach( var bullet in m_UpdateBullets )
		{
			if( bullet == null )
			{
				continue;
			}

			bullet.OnUpdate();
		}
	}

	public override void OnLateUpdate()
	{
		// LateUpdate処理
		foreach( var bullet in m_UpdateBullets )
		{
			if( bullet == null )
			{
				continue;
			}

			bullet.OnLateUpdate();
		}

		GotoPoolFromUpdate();
	}



	/// <summary>
	/// UPDATE状態にする。
	/// </summary>
	private void GotoUpdateFromStandby()
	{
		int count = m_GotoUpdateBullets.Count();

		for( int i = 0; i < count; i++ )
		{
			var node = m_GotoUpdateBullets.First;
			var bullet = node.Value;
			m_StandbyBullets.Remove( bullet );
			m_UpdateBullets.AddLast( bullet );
			m_GotoUpdateBullets.RemoveFirst();
			bullet.SetBulletCycle( Bullet.E_BULLET_CYCLE.UPDATE );
		}

		m_GotoUpdateBullets.Clear();
	}

	/// <summary>
	/// POOL状態にする。
	/// </summary>
	private void GotoPoolFromUpdate()
	{
		int count = m_GotoPoolBullets.Count;

		for( int i = 0; i < count; i++ )
		{
			var node = m_GotoPoolBullets.First;
			var bullet = node.Value;
			m_UpdateBullets.Remove( node );
			m_GotoPoolBullets.RemoveFirst();
			bullet.SetBulletCycle( Bullet.E_BULLET_CYCLE.POOLED );
		}

		m_GotoPoolBullets.Clear();
	}

	/// <summary>
	/// 弾をSTANDBY状態にして制御下に入れる。
	/// </summary>
	public void CheckStandbyBullet( Bullet bullet )
	{
		if( bullet == null )
		{
			Debug.LogError( "指定された弾がnullのため、追加できませんでした。" );
			return;
		}

		m_StandbyBullets.AddLast( bullet );
		bullet.SetBulletCycle( Bullet.E_BULLET_CYCLE.STANDBY );
		bullet.gameObject.SetActive( true );
	}

	/// <summary>
	/// 指定した弾を制御から外すためにチェックする。
	/// </summary>
	public void CheckPoolBullet( Bullet bullet )
	{
		if( bullet == null )
		{
			Debug.LogError( "指定した弾がnullのため、削除できませんでした。" );
			return;
		}

		bullet.gameObject.SetActive( false );
		m_GotoPoolBullets.AddLast( bullet );
	}

	/// <summary>
	/// 指定した弾をBulletHolderの直下に入れる。
	/// </summary>
	private void SetBulletParent( Bullet bullet )
	{
		if( bullet == null )
		{
			return;
		}

		Transform t = m_BulletHolder;

		if( t == null )
		{
			t = transform;
		}

		bullet.transform.SetParent( t );
	}

	/// <summary>
	/// プールから弾を取得する。
	/// 足りなければ生成する。
	/// </summary>
	/// <param name="bulletPrefab">取得や生成の情報源となる弾のプレハブ</param>
	public Bullet GetPoolingBullet( Bullet bulletPrefab )
	{
		if( bulletPrefab == null )
		{
			return null;
		}

		string bulletId = bulletPrefab.GetBulletGroupId();
		Bullet bullet = null;

		foreach( var b in m_PoolBullets )
		{
			if( b != null && b.GetBulletGroupId() == bulletId )
			{
				bullet = b;
				break;
			}
		}

		if( bullet == null )
		{
			bullet = Instantiate( bulletPrefab );
			SetBulletParent( bullet );
			m_PoolBullets.AddLast( bullet );
		}

		return bullet;
	}
}
