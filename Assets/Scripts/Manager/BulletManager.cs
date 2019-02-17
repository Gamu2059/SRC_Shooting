using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全ての弾の制御を管理する。
/// </summary>
public class BulletManager : GlobalSingletonMonoBehavior<BulletManager>
{
	[SerializeField]
	private Transform m_BulletHolder;

	[SerializeField]
	private List<Bullet> m_Bullets;

	private List<Bullet> m_RemovingBullets;

	protected override void OnAwake()
	{
		base.OnAwake();
		m_Bullets = new List<Bullet>();
		m_RemovingBullets = new List<Bullet>();
	}

	private void Start()
	{
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

	public override void OnInit()
	{
	}

	public override void OnStart()
	{
	}

	public override void OnUpdate()
	{
		foreach( var bullet in m_Bullets )
		{
			if( bullet == null )
			{
				continue;
			}

			bullet.OnUpdate();
		}

		RemoveBullet();
	}

	public override void OnLateUpdate()
	{
		foreach( var bullet in m_Bullets )
		{
			if( bullet == null )
			{
				continue;
			}

			bullet.OnLateUpdate();
		}

		RemoveBullet();
	}

	/// <summary>
	/// 削除リストに入っている弾を制御下から外す。
	/// </summary>
	private void RemoveBullet()
	{
		int removeNum = m_RemovingBullets.Count;

		for( int i = 0; i < removeNum; i++ )
		{
			var bullet = m_RemovingBullets[0];
			m_Bullets.Remove( bullet );
			m_RemovingBullets.Remove( bullet );
		}
	}

	public void AddBullet( Bullet bullet )
	{
		if( bullet == null || m_Bullets.Contains( bullet ) )
		{
			return;
		}

		m_Bullets.Add( bullet );
	}

	public void CheckRemovingBullet( Bullet bullet )
	{
		if( bullet == null || !m_Bullets.Contains( bullet ) )
		{
			return;
		}

		m_RemovingBullets.Add( bullet );
	}

	public void SetBulletParent( Bullet bullet )
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
}
