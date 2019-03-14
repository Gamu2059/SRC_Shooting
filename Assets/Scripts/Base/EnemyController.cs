﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharaControllerBase
{

	[SerializeField]
	private bool m_IsBoss;

	private void Start()
	{
		EnemyCharaManager.Instance.RegistEnemy( this );
	}

	public override void OnSuffer( BulletController bullet, CollisionManager.ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
	}

}