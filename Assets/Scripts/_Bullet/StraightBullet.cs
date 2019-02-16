using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : Bullet
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

		float rad = transform.eulerAngles.y * Mathf.Deg2Rad;
		var move = new Vector3( Mathf.Cos( rad ), 0, Mathf.Sin( rad ) ) * m_Speed *Time.deltaTime;
		transform.Translate( move, Space.World );
		m_DistCount += m_Speed * Time.deltaTime;

		if( m_DistCount > m_DeadDistance )
		{
			DestroyBullet();
		}
	}
}
