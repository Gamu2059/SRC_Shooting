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
		
		if (shotDelay >= m_ShotInterval)
		{
		    for (int i = 0; i < m_MainShotPosition.Length; i++)
		    {
                var shotParam = new BulletShotParam(this);
                shotParam.Position = m_MainShotPosition[i].transform.position - transform.parent.position;
                shotParam.OrbitalIndex = 4;
                BulletController.ShotBullet(shotParam);
		    }
		    shotDelay = 0;
		}
		 

	}

	public override void ShotBomb()
	{
		base.ShotBomb();
	}
}
