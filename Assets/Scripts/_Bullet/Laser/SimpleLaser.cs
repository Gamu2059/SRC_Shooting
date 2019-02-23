using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLaser : Bullet
{

	[SerializeField]
	private Vector3 m_AnchoredPosition;

	protected override void OnUpdateBulletOrbital()
	{
		base.OnUpdateBulletOrbital();

		transform.position = m_Owner.transform.position;
	}

}
