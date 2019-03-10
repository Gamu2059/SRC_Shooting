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



	private void Start()
	{
		PlayerCharaManager.Instance.RegistChara( this );
	}

	/// <summary>
	/// �ʏ�e�𔭎˂���B
	/// </summary>
	public virtual void ShotBullet()
	{
		Bullet.ShotBullet( this );
	}


	/// <summary>
	/// �{�����g�p����B
	/// </summary>
	public virtual void ShotBomb( int bombIndex = 0 )
	{

	}



	/// <summary>
	/// �L�������ړ�������B
	/// �ړ����x�̓L�����Ɍ��ݐݒ肳��Ă�����̂ƂȂ�B
	/// </summary>
	/// <param name="moveDirection"> �ړ����� </param>
	public virtual void Move( Vector3 moveDirection )
	{
		Vector3 move = moveDirection.normalized * m_MoveSpeed * Time.deltaTime;
		transform.Translate( move, Space.World );
	}


	public override void OnSuffer( Bullet bullet, CollisionManager.ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
	}

	public int GetLevel()
	{
		return m_Lv;
	}

}
