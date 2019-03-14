﻿using System.Collections;
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
	private List<BulletController> m_StandbyBullets;

	/// <summary>
	/// UPDATE状態の弾を保持するリスト。
	/// </summary>
	[SerializeField]
	private List<BulletController> m_UpdateBullets;

	/// <summary>
	/// POOL状態の弾を保持するリスト。
	/// </summary>
	[SerializeField]
	private List<BulletController> m_PoolBullets;

	/// <summary>
	/// UPDATE状態に遷移する弾のリスト。
	/// </summary>
	private List<BulletController> m_GotoUpdateBullets;

	/// <summary>
	/// POOL状態に遷移する弾のリスト。
	/// </summary>
	private List<BulletController> m_GotoPoolBullets;

	/// <summary>
	/// STANDBY状態の弾を保持するリストを取得する。
	/// </summary>
	public List<BulletController> GetStandbyBullets()
	{
		return m_StandbyBullets;
	}

	/// <summary>
	/// UPDATE状態の弾を保持するリストを取得する。
	/// </summary>
	public List<BulletController> GetUpdateBullets()
	{
		return m_UpdateBullets;
	}

	/// <summary>
	/// POOL状態の弾を保持するリストを取得する。
	/// </summary>
	public List<BulletController> GetPoolBullets()
	{
		return m_PoolBullets;
	}



	public override void OnInitialize()
	{
		m_StandbyBullets = new List<BulletController>();
		m_UpdateBullets = new List<BulletController>();
		m_PoolBullets = new List<BulletController>();
		m_GotoUpdateBullets = new List<BulletController>();
		m_GotoPoolBullets = new List<BulletController>();
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
			m_GotoUpdateBullets.Add( bullet );
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
			int idx = count - i - 1;
			var bullet = m_GotoUpdateBullets[idx];
			m_GotoUpdateBullets.RemoveAt( idx );
			m_StandbyBullets.Remove( bullet );
			m_UpdateBullets.Add( bullet );
			bullet.SetBulletCycle( BulletController.E_BULLET_CYCLE.UPDATE );
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
			int idx = count - i - 1;
			var bullet = m_GotoPoolBullets[idx];
			bullet.SetBulletCycle( BulletController.E_BULLET_CYCLE.POOLED );
			m_GotoPoolBullets.RemoveAt( idx );
			m_UpdateBullets.Remove( bullet );
			m_PoolBullets.Add( bullet );
		}

		m_GotoPoolBullets.Clear();
	}

	/// <summary>
	/// 弾をSTANDBY状態にして制御下に入れる。
	/// </summary>
	public void CheckStandbyBullet( BulletController bullet )
	{
		if( bullet == null || !m_PoolBullets.Contains( bullet ) )
		{
			Debug.LogError( "指定された弾を追加できませんでした。" );
			return;
		}

		m_PoolBullets.Remove( bullet );
		m_StandbyBullets.Add( bullet );
		bullet.gameObject.SetActive( true );
		bullet.SetBulletCycle( BulletController.E_BULLET_CYCLE.STANDBY_UPDATE );
	}

	/// <summary>
	/// 指定した弾を制御から外すためにチェックする。
	/// </summary>
	public void CheckPoolBullet( BulletController bullet )
	{
		if( bullet == null || m_GotoPoolBullets.Contains( bullet ) )
		{
			Debug.LogError( "指定した弾を削除できませんでした。" );
			return;
		}

		bullet.SetBulletCycle( BulletController.E_BULLET_CYCLE.STANDBY_POOL );
		m_GotoPoolBullets.Add( bullet );
		bullet.gameObject.SetActive( false );
	}

	/// <summary>
	/// 指定した弾をBulletHolderの直下に入れる。
	/// </summary>
	private void SetBulletParent( BulletController bullet )
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
	public BulletController GetPoolingBullet( BulletController bulletPrefab )
	{
		if( bulletPrefab == null )
		{
			return null;
		}

		string bulletId = bulletPrefab.GetBulletGroupId();
		BulletController bullet = null;

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
			m_PoolBullets.Add( bullet );
		}

		return bullet;
	}
}