using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラの制御を行うコンポーネント。
/// </summary>
public class CharaControllerBase : ControllableMonoBehaviour, ICollisionBase
{
	/// <summary>
	/// キャラの所属。
	/// </summary>
	public enum E_CHARA_TROOP
	{
		/// <summary>
		/// プレイヤーキャラ。
		/// </summary>
		PLAYER,

		/// <summary>
		/// 敵キャラ。
		/// </summary>
		ENEMY,
	}



	#region Field

	[Header( "キャラの基礎パラメータ" )]

	[SerializeField]
	private CollisionManager.ColliderTransform[] m_ColliderTransforms;

	[SerializeField]
	private E_CHARA_TROOP m_Troop;

	[SerializeField]
	protected float m_MoveSpeed = 5f;

	[Header( "弾のパラメータ" )]

	[SerializeField]
	protected Bullet[] m_BulletPrefabs;

	[SerializeField]
	protected ControllableMonoBehaviour[] m_BombPrefabs;

	[SerializeField]
	protected BulletParam[] m_BulletParams;

	[Header( "プロテクタのパラメータ" )]

	[SerializeField]
	protected Transform[] m_Protectors;

	[SerializeField]
	protected float m_ProtectorRadius;

	[SerializeField]
	protected float m_ProtectorSpeed;

	protected float m_ProtectorRad;

	#endregion



	#region Getter & Setter

	public CollisionManager.ColliderTransform[] GetColliderTransforms()
	{
		return m_ColliderTransforms;
	}

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

	/// <summary>
	/// キャラが保持するBulletParamの個数を取得する。
	/// </summary>
	public int GetBulletParamsCount()
	{
		if( m_BulletParams == null )
		{
			return -1;
		}

		return m_BulletParams.Length;
	}

	/// <summary>
	/// 指定したインデックスのBulletParamを取得する。
	/// </summary>
	public BulletParam GetBulletParam( int bulletParamIndex = 0 )
	{
		int paramCount = GetBulletParamsCount();

		if( m_BulletPrefabs == null || paramCount < 1 )
		{
			return null;
		}

		if( bulletParamIndex < 0 || bulletParamIndex >= paramCount )
		{
			bulletParamIndex = 0;
		}

		return m_BulletParams[bulletParamIndex];
	}

	/// <summary>
	/// キャラが保持する弾プレハブの個数を取得する。
	/// </summary>
	public int GetBulletPrefabsCount()
	{
		if( m_BulletPrefabs == null )
		{
			return -1;
		}

		return m_BulletPrefabs.Length;
	}

	/// <summary>
	/// 指定したインデックスの弾のプレハブを取得する。
	/// </summary>
	public Bullet GetBulletPrefab( int bulletIndex = 0 )
	{
		int prefabCount = GetBulletPrefabsCount();

		if( m_BulletPrefabs == null || prefabCount < 1 )
		{
			return null;
		}

		if( bulletIndex < 0 || bulletIndex >= prefabCount )
		{
			bulletIndex = 0;
		}

		return m_BulletPrefabs[bulletIndex];
	}

	#endregion



	public override void OnUpdate()
	{
		UpdateProtector();
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



	public virtual CollisionManager.ColliderData[] GetColliderData()
	{
		int hitNum = m_ColliderTransforms.Length;
		var colliders = new CollisionManager.ColliderData[hitNum];

		for( int i = 0; i < hitNum; i++ )
		{
			Transform t = m_ColliderTransforms[i].Transform;
			var c = new CollisionManager.ColliderData();
			c.CenterPos = new Vector2( t.position.x, t.position.z );
			c.Size = new Vector2( t.lossyScale.x, t.lossyScale.z );
			c.Angle = -t.eulerAngles.y;
			c.ColliderType = m_ColliderTransforms[i].ColliderType;

			colliders[i] = c;
		}

		return colliders;
	}

	public virtual bool CanHitBullet()
	{
		return false;
	}

	public virtual void OnHitCharacter( CharaControllerBase chara )
	{

	}

	public virtual void OnHitBullet( Bullet bullet )
	{

	}

	public virtual void OnSuffer( Bullet bullet, CollisionManager.ColliderData colliderData )
	{

	}
}
