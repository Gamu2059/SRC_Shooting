using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : BulletController
{
	[SerializeField]
	private float m_Speed;

	[SerializeField]
	private float m_DeadDistance = 500;

	private float m_DistCount;

	public override void OnStart()
	{
		base.OnStart();
		m_DistCount = 0;
	}

	public override void OnUpdate()
	{
		transform.Translate( transform.forward * m_Speed * Time.deltaTime, Space.World );
		m_DistCount += m_Speed * Time.deltaTime;

		if( m_DistCount > m_DeadDistance )
		{
			DestroyBullet();
		}
	}
}
