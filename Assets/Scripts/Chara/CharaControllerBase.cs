using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラの制御を行うコンポーネント。
/// </summary>
public class CharaControllerBase : BehaviorBase
{

	[Header( "キャラの基礎パラメータ" )]

	[SerializeField]
	private float m_MoveSpeed = 5f;

	[Header( "弾のパラメータ" )]

	[SerializeField]
	private Bullet[] m_BulletPrefabs;

	[SerializeField]
	private BehaviorBase[] m_BombPrefabs;

	[Header( "プロテクタのパラメータ" )]

	[SerializeField]
	private Transform[] m_Protectors;

	[SerializeField]
	private float m_ProtectorRadius;

	[SerializeField]
	private float m_ProtectorSpeed;



	private float m_Rad;

	private List<Bullet>[] m_PoolBullets;


	public float GetMoveSpeed()
	{
		return m_MoveSpeed;
	}

	protected void SetMoveSpeed( float speed )
	{
		m_MoveSpeed = speed;
	}

	public BehaviorBase[] GetBulletPrefabs()
	{
		return m_BulletPrefabs;
	}

	public BehaviorBase[] GetBombPrefabs()
	{
		return m_BombPrefabs;
	}



	public override void OnUpdate()
	{
		UpdateProtector();
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

	/// <summary>
	/// 通常弾を発射する。
	/// </summary>
	public virtual void ShotBullet( int bulletIndex = 0 )
	{

	}

	/// <summary>
	/// ボムを使用する。
	/// </summary>
	public virtual void ShotBomb( int bombIndex = 0 )
	{

	}

	/// <summary>
	/// プールから弾を取得する。
	/// 足りなければ生成する。
	/// </summary>
	public Bullet GetPoolBullet( int bulletIndex = 0 )
	{
		if( m_PoolBullets == null )
		{
			m_PoolBullets = new List<Bullet>[m_BulletPrefabs.Length];
		}

		if( m_PoolBullets[bulletIndex] == null )
		{
			m_PoolBullets[bulletIndex] = new List<Bullet>();
		}

		var bullets = m_PoolBullets[bulletIndex];
		Bullet bullet = null;

		foreach( var b in bullets )
		{
			if( b.gameObject.activeSelf )
			{
				continue;
			}

			bullet = b;
			break;
		}

		if( bullet == null )
		{
			bullet = Instantiate( m_BulletPrefabs[bulletIndex] );
			BulletManager.Instance.SetBulletParent( bullet );
			bullets.Add( bullet );
		}

		bullet.gameObject.SetActive( true );
		return bullet;
	}

	protected virtual void UpdateProtector()
	{
		m_Rad += m_ProtectorSpeed * Time.deltaTime;
		m_Rad %= Mathf.PI * 2;
		float unitAngle = Mathf.PI * 2 / m_Protectors.Length;

		for( int i = 0; i < m_Protectors.Length; i++ )
		{
			float angle = unitAngle * i + m_Rad;
			float x = m_ProtectorRadius * Mathf.Cos( angle );
			float z = m_ProtectorRadius * Mathf.Sin( angle );
			m_Protectors[i].localPosition = new Vector3( x, 0, z );
			m_Protectors[i].LookAt( transform );
		}
	}
}
