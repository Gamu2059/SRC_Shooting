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

	protected override void OnAwake()
	{
		base.OnAwake();
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

			if( !bullet.IsStarted )
			{
				bullet.OnStart();
				bullet.IsStarted = true;
			}
			else
			{
				bullet.OnUpdate();
			}
		}
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
	}

	public void AddBullet( Bullet bullet )
	{
		if( bullet == null || m_Bullets.Contains( bullet ) )
		{
			return;
		}

		bullet.IsStarted = false;
		m_Bullets.Add( bullet );
	}

	public void RemoveBullet( Bullet bullet )
	{
		if( bullet == null || !m_Bullets.Contains( bullet ) )
		{
			return;
		}

		m_Bullets.Remove( bullet );
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
