using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラの制御を行うコンポーネント。
/// </summary>
[RequireComponent( typeof( BattleObjectCollider ) )]
public class CharaController : ControllableMonoBehaviour, ICollisionBase
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
	private E_CHARA_TROOP m_Troop;

	[Header( "弾のパラメータ" )]

	[SerializeField]
	private BulletSetParam m_BulletSetParam;

	[SerializeField]
	private BattleObjectCollider m_Collider;

	#endregion



	#region Getter & Setter

	public BattleObjectCollider GetCollider()
	{
		return m_Collider;
	}

	public E_CHARA_TROOP GetTroop()
	{
		return m_Troop;
	}

	public BulletSetParam GetBulletSetParam()
	{
		return m_BulletSetParam;
	}

	public void SetBulletSetParam( BulletSetParam param )
	{
		m_BulletSetParam = param;
	}

	/// <summary>
	/// キャラが保持するBulletParamの個数を取得する。
	/// </summary>
	public int GetBulletParamsCount()
	{
		if( GetBulletSetParam().GetBulletParams() == null )
		{
			return -1;
		}

		return GetBulletSetParam().GetBulletParams().Length;
	}

	/// <summary>
	/// 指定したインデックスのBulletParamを取得する。
	/// </summary>
	public BulletParam GetBulletParam( int bulletParamIndex = 0 )
	{
		int paramCount = GetBulletParamsCount();

		if( GetBulletSetParam().GetBulletParams() == null || paramCount < 1 )
		{
			return null;
		}

		if( bulletParamIndex < 0 || bulletParamIndex >= paramCount )
		{
			bulletParamIndex = 0;
		}

		return GetBulletSetParam().GetBulletParams()[bulletParamIndex];
	}

	/// <summary>
	/// キャラが保持する弾プレハブの個数を取得する。
	/// </summary>
	public int GetBulletPrefabsCount()
	{
		if( GetBulletSetParam().GetBulletPrefabs() == null )
		{
			return -1;
		}

		return GetBulletSetParam().GetBulletPrefabs().Length;
	}

	/// <summary>
	/// 指定したインデックスの弾のプレハブを取得する。
	/// </summary>
	public BulletController GetBulletPrefab( int bulletIndex = 0 )
	{
		int prefabCount = GetBulletPrefabsCount();

		if( GetBulletSetParam().GetBulletPrefabs() == null || prefabCount < 1 )
		{
			return null;
		}

		if( bulletIndex < 0 || bulletIndex >= prefabCount )
		{
			bulletIndex = 0;
		}

		return GetBulletSetParam().GetBulletPrefabs()[bulletIndex];
	}

	#endregion



	public override void OnInitialize()
	{
		base.OnInitialize();
		m_Collider = GetComponent<BattleObjectCollider>();
	}

	public virtual ColliderData[] GetColliderData()
	{
		return m_Collider.GetColliderData();
	}

	public virtual bool CanHitBullet()
	{
		return m_Collider.CanHitBullet();
	}

	public virtual void OnHitCharacter( CharaController chara )
	{

	}

	public virtual void OnHitBullet( BulletController bullet )
	{

	}

	public virtual void OnSuffer( BulletController bullet, ColliderData colliderData )
	{

	}
}
