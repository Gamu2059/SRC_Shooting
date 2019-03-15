using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharaController
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
	protected float m_MoveSpeed = 5f;

	[SerializeField]
	private E_PLAYER_LIFE_CYCLE m_LifeCycle;

	[SerializeField]
	private float m_HitSize;

	[SerializeField]
	private GameObject m_BulletPrefab;

	[SerializeField]
	private GameObject m_BombPrefab;

	[Header( "プロテクタのパラメータ" )]

	[SerializeField]
	protected Transform[] m_Protectors;

	[SerializeField]
	protected float m_ProtectorRadius;

	[SerializeField]
	protected float m_ProtectorSpeed;

	protected float m_ProtectorRad;



	private void Start()
	{
		PlayerCharaManager.Instance.RegistChara( this );
	}

	/// <summary>
	/// 通常弾を発射する。
	/// </summary>
	public virtual void ShotBullet()
	{
		BulletController.ShotBullet( this );
	}


	/// <summary>
	/// ボムを使用する。
	/// </summary>
	public virtual void ShotBomb( int bombIndex = 0 )
	{

	}



	/// <summary>
	/// キャラを移動させる。
	/// 移動速度はキャラに現在設定されているものとなる。
	/// </summary>
	/// <param name="moveDirection"> 移動方向 </param>
	public virtual void Move( Vector3 moveDirection )
	{
		Vector3 move = moveDirection.normalized * m_MoveSpeed * Time.deltaTime;
		transform.Translate( move, Space.World );
	}


	public override void OnSuffer( BulletController bullet, ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
	}

	public int GetLevel()
	{
		return m_Lv;
	}

	protected virtual void UpdateProtector()
	{
		m_ProtectorRad += m_ProtectorSpeed * Time.deltaTime;
		m_ProtectorRad %= Mathf.PI * 2;
		float unitAngle = Mathf.PI * 2 / m_Protectors.Length;

		for( int i = 0; i < m_Protectors.Length; i++ )
		{
			float angle = unitAngle * i + m_ProtectorRad;
			float x = m_ProtectorRadius * Mathf.Cos( angle );
			float z = m_ProtectorRadius * Mathf.Sin( angle );
			m_Protectors[i].localPosition = new Vector3( x, 0, z );
			m_Protectors[i].LookAt( transform );
		}
	}

}
