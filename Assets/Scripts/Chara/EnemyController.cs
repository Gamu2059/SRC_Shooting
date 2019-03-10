using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharaControllerBase
{

	[SerializeField]
	private bool m_IsBoss;

	private void Start()
	{
		EnemyCharaManager.Instance.RegistChara( this );
	}

	public override void OnSuffer( Bullet bullet, CollisionManager.ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
	}

}
