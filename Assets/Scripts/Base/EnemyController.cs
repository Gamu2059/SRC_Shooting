using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharaController
{

	[SerializeField]
	private bool m_IsBoss;

	private void Start()
	{
		EnemyCharaManager.Instance.RegistEnemy( this );
	}

	public override void OnSuffer( BulletController bullet, ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
	}

}
