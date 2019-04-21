using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerController : PlayerController
{
	[SerializeField, Range( 0f, 1f )]
	private float m_ShotInterval;

	private float shotDelay;

	[SerializeField]
	private Transform[] m_MainShotPosition;

	public override void OnUpdate()
	{
		base.OnUpdate();
		shotDelay += Time.deltaTime;
	}

	public override void ShotBullet()
	{
		/*
		if (shotDelay >= m_ShotInterval && IsReadyShotBullet)
		{
		    Bullet b = GetOriginalBullet(0);
		    for (int i = 0; i < m_MainShotPosition.Length; i++)
		    {
		        Bullet bullet = GetPoolBullet(0);
		        bullet.ShotBullet(this, m_MainShotPosition[i].position, m_MainShotPosition[i].eulerAngles, b.transform.localScale, bulletIndex, m_BulletParams[0], 2);
		    }
		    shotDelay = 0;
		    IsReadyShotBullet = false;
		}
		 */

	}

	public override void ShotBomb()
	{
		base.ShotBomb();
	}
}
