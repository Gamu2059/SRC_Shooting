﻿using System.Collections;
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



	#region Field Inspector

	[Header( "キャラの基礎パラメータ" )]

	[SerializeField, Tooltip( "キャラの所属" )]
	private E_CHARA_TROOP m_Troop;

	[SerializeField, Tooltip( "キャラが用いる弾の組み合わせ" )]
	private BulletSetParam m_BulletSetParam;

	[SerializeField, Tooltip( "キャラの衝突情報" )]
	private BattleObjectCollider m_Collider;

	[Header( "キャラの基礎ステータス" )]

	[SerializeField, Tooltip( "キャラの現在HP" )]
	private int m_NowHp;

	[SerializeField, Tooltip( "キャラの最大HP" )]
	private int m_MaxHp;

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

	/// <summary>
	/// このキャラを回復する。
	/// </summary>
	public virtual void Recover( int recover )
	{
		if( recover <= 0 )
		{
			return;
		}

		m_NowHp = Mathf.Clamp( m_NowHp + recover, 0, m_MaxHp );
	}

	/// <summary>
	/// このキャラにダメージを与える。
	/// HPが0になった場合は死ぬ。
	/// </summary>
	public virtual void Damage( int damage )
	{
		if( damage <= 0 )
		{
			return;
		}

		m_NowHp = Mathf.Clamp( m_NowHp - damage, 0, m_MaxHp );

		if( m_NowHp == 0 )
		{
			Dead();
		}
	}

	/// <summary>
	/// このキャラを死亡させる。
	/// </summary>
	public virtual void Dead()
	{

	}


	/// <summary>
	/// このキャラの衝突情報を取得する。
	/// </summary>
	public virtual ColliderData[] GetColliderData()
	{
		return m_Collider.GetColliderData();
	}

	/// <summary>
	/// このキャラ自身から弾に当たることがあるかどうか。
	/// 基本的にキャラから弾に当たりに行くことはないので、falseが返ってくる。
	/// </summary>
	public virtual bool CanHitBullet()
	{
		return false;
		//return m_Collider.CanHitBullet();
	}

	/// <summary>
	/// このキャラが他のキャラに当たった場合のコールバック。
	/// </summary>
	public virtual void OnHitCharacter( CharaController chara )
	{

	}

	/// <summary>
	/// このキャラが他の弾に当たった場合のコールバック。
	/// </summary>
	public virtual void OnHitBullet( BulletController bullet )
	{

	}

	/// <summary>
	/// 他の弾がこのキャラに当たった場合のコールバック。
	/// </summary>
	public virtual void OnSuffer( BulletController bullet, ColliderData colliderData )
	{
		Damage( 1 );
	}
}
