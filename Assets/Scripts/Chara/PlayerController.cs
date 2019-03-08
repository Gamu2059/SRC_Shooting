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



	/// <summary>
	/// �ʏ�e�𔭎˂���B
	/// </summary>
	/// <param name="bulletIndex">���˂������e��index</param>
	/// <param name="bulletParamIndex">�e�̋O�����߂�p�����[�^��index</param>
	public virtual Bullet ShotBullet( int bulletIndex, int bulletParamIndex )
	{
		if( bulletIndex < 0 || bulletIndex >= m_BulletPrefabs.Length )
		{
			return null;
		}

		if( bulletParamIndex < 0 || bulletParamIndex >= m_BulletParams.Length )
		{
			return null;
		}

		BulletParam bulletParam = m_BulletParams[bulletParamIndex];

		if( bulletParam == null )
		{
			return null;
		}

		return null;

		//GameObject bulletPrefab = m_BulletPrefabs[bulletIndex].gameObject;
		//Bullet bullet = GetPoolBullet( bulletIndex );
		//bullet.ShotBullet( this, transform.position, transform.eulerAngles, bulletPrefab.transform.localScale, bulletIndex, bulletParam, -1 );

		//return bullet;



		//float angle = bulletRot.y;
		//angle -= initSpreadParam.DeltaAngle * ( i - ( bulletNum - 1 ) / 2f );
		//bulletRot.y = angle;

		//// ���w��̉�]�͔����v����Unity�̉�]�͎��v�Ȃ̂�angle���t�ɂ��A�������Ɍ�����ɂ�90�x�����K�v������
		//angle = ( -angle + 90 ) * Mathf.Deg2Rad;
		//Vector3 offsetPos = new Vector3( Mathf.Cos( angle ), 0, Mathf.Sin( angle ) ) * initSpreadParam.Radius;
		//bulletPos += offsetPos;

		//Bullet bullet = GetPoolBullet( bulletIndex );
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
		Debug.LogWarning( 11111 );
	}
}
