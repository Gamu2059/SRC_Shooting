using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharaControllerBase
{

	[System.Serializable]
	public enum E_PLAYER_LIFE_CYCLE
	{
		AHEAD,
		SORTIE,
		DEAD,
		DEAD_AHEAD,
	}

	[SerializeField]
	private int m_Lv;

	[SerializeField]
	private int m_Exp;

	[SerializeField]
	private int m_NowHp;

	[SerializeField]
	private int m_MaxHp;

	[SerializeField]
	private E_PLAYER_LIFE_CYCLE m_LifeCycle;

	[SerializeField]
	private float m_HitSize;

	[SerializeField]
	private GameObject m_BulletPrefab;

	[SerializeField]
	private GameObject m_BombPrefab;

	public override void OnSuffer( Bullet bullet, CollisionManager.ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
		Debug.Log( 11111 );
	}
}
