using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラの制御を行うコンポーネント。
/// </summary>
public class CharaControllerBase : BehaviorBase
{

	public enum E_CHARA_TROOP
	{
		// プレイヤーキャラ
		PLAYER,

		// 敵キャラ
		ENEMY,
	}

	[Header( "キャラの基礎パラメータ" )]

	[SerializeField]
	private E_CHARA_TROOP m_Troop;

	[SerializeField]
	private float m_MoveSpeed = 5f;

	[Header( "弾のパラメータ" )]

	[SerializeField]
	private Bullet[] m_BulletPrefabs;

	[SerializeField]
	private BehaviorBase[] m_BombPrefabs;

	[SerializeField]
	private BulletParam[] m_BulletParams;

	[Header( "プロテクタのパラメータ" )]

	[SerializeField]
	private Transform[] m_Protectors;

	[SerializeField]
	private float m_ProtectorRadius;

	[SerializeField]
	private float m_ProtectorSpeed;



	private float m_Rad;

	private List<Bullet>[] m_PoolBullets;



	public E_CHARA_TROOP GetTroop()
	{
		return m_Troop;
	}

	public float GetMoveSpeed()
	{
		return m_MoveSpeed;
	}

	protected void SetMoveSpeed( float speed )
	{
		m_MoveSpeed = speed;
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
	/// <param name="bulletIndex">発射したい弾のindex</param>
	/// <param name="bulletParamIndex">弾の軌道を定めるパラメータのindex</param>
	public virtual void ShotBullet( int bulletIndex, int bulletParamIndex )
	{
		if( bulletIndex < 0 || bulletIndex >= m_BulletPrefabs.Length )
		{
			return;
		}

		if( bulletParamIndex < 0 || bulletParamIndex >= m_BulletParams.Length )
		{
			return;
		}

		BulletParam bulletParam = m_BulletParams[bulletParamIndex];

		if( bulletParam == null )
		{
			return;
		}

		GameObject bulletPrefab = m_BulletPrefabs[bulletIndex].gameObject;
		Bullet bullet = GetPoolBullet( bulletIndex );
		bullet.ShotBullet( this, transform.position, transform.eulerAngles, bulletPrefab.transform.localScale, bulletIndex, bulletParam, -1 );

		//float angle = bulletRot.y;
		//angle -= initSpreadParam.DeltaAngle * ( i - ( bulletNum - 1 ) / 2f );
		//bulletRot.y = angle;

		//// 数学上の回転は反時計だがUnityの回転は時計なのでangleを逆にし、青軸方向に向けるには90度足す必要がある
		//angle = ( -angle + 90 ) * Mathf.Deg2Rad;
		//Vector3 offsetPos = new Vector3( Mathf.Cos( angle ), 0, Mathf.Sin( angle ) ) * initSpreadParam.Radius;
		//bulletPos += offsetPos;

		//Bullet bullet = GetPoolBullet( bulletIndex );
	}

	/// <summary>
	/// ボムを使用する。
	/// </summary>
	public virtual void ShotBomb( int bombIndex = 0 )
	{

	}

	/// <summary>
	/// このキャラが保持する弾のプレハブを返す。
	/// 複製物を使用したい場合は、GetPoolBulletから取得して下さい。
	/// </summary>
	public Bullet GetOriginalBullet( int bulletIndex = 0 )
	{
		if( bulletIndex < 0 || bulletIndex >= m_BulletPrefabs.Length )
		{
			return null;
		}

		return m_BulletPrefabs[bulletIndex];
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
