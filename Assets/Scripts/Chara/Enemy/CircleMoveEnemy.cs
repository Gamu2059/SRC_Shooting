using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 円運動をしながら定期的に弾を撃ってくる敵。
/// </summary>
public class CircleMoveEnemy : EnemyController
{

	[SerializeField]
	private Vector3 m_BasePos;

	[SerializeField]
	private float m_Radius;

	[SerializeField]
	private float m_AngleSpeed;

	[SerializeField]
	private float m_ShotInterval;

	[SerializeField]
	private float m_ShotPosOffset;

	[SerializeField]
	private float m_BulletSpeed = 10;

	[SerializeField]
	private float m_DeadDistance = 500;

	private float m_NowRad;

	private float m_ShotTime;

	private void Awake()
	{
		m_ShotTime = 0;
	}

	public override void OnUpdate()
	{
		m_NowRad += m_AngleSpeed * Time.deltaTime;
		m_NowRad %= Mathf.PI * 2;

		Vector3 pos = new Vector3( Mathf.Cos( m_NowRad ), 0, Mathf.Sin( m_NowRad ) ) * m_Radius + m_BasePos;
		transform.position = pos;

		if( m_ShotTime < 0f )
		{
			m_ShotTime = m_ShotInterval;
			ShotBullet();
		}
		else
		{
			m_ShotTime -= Time.deltaTime;
		}
	}

	public override void ShotBullet( int bulletIndex = 0 )
	{
		Bullet bullet = GetPoolBullet( 0 );
		bullet.transform.position = transform.position;
		bullet.transform.eulerAngles = Vector3.up * m_NowRad * Mathf.Rad2Deg;
		bullet.transform.Translate( m_ShotPosOffset * Mathf.Cos( m_NowRad ), 0, m_ShotPosOffset * Mathf.Sin( m_NowRad ) );
		bullet.ShotBullet();
	}
}
