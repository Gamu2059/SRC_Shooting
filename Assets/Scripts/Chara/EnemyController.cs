using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharaControllerBase
{

	[SerializeField]
	private bool m_IsBoss;

	public override void OnSuffer( Bullet bullet, CollisionManager.ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
	}

}
