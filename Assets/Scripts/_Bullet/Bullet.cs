using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全ての弾オブジェクトの基礎クラス。
/// </summary>
public class Bullet : BehaviorBase
{




	[HideInInspector]
	public bool IsStarted;

	/// <summary>
	/// 弾を発射したキャラ
	/// </summary>
	[SerializeField]
	private CharaControllerBase m_Owner;

	/// <summary>
	/// 弾の標的となっているキャラ
	/// </summary>
	[SerializeField]
	private CharaControllerBase m_Target;

	[SerializeField]
	private BulletParam m_BulletParam;

	[SerializeField]
	private BulletOrbitalParam m_OrbitalParam;

	protected float m_NowLifeTime;
	protected float m_NowHitSize;
	protected float m_NowSpeed;
	protected bool m_IsReverseHacked; // ハッカーのリバースハックを受けたかどうか

	public BulletParam GetBulletParam()
	{
		return m_BulletParam;
	}

	public BulletOrbitalParam GetOrbitalParam()
	{
		return m_OrbitalParam;
	}


	/// <summary>
	/// 最初のフレームで呼び出される。
	/// </summary>
	public override void OnStart()
	{
		base.OnStart();
	}

	/// <summary>
	/// 最初以降のフレームで呼び出される。
	/// </summary>
	public override void OnUpdate()
	{
		base.OnUpdate();

	}

	/// <summary>
	/// StartやUpdateの後で呼び出される。
	/// </summary>
	public override void OnLateUpdate()
	{
		base.OnLateUpdate();
	}



	/// <summary>
	/// この弾を発射させる。
	/// </summary>
	public virtual void ShotBullet( BulletParam bulletParam, int orbitalIndex = -1 )
	{
		if( bulletParam == null )
		{
			DestroyBullet();
			return;
		}

		BulletManager.Instance.AddBullet( this );
		m_BulletParam = bulletParam;
	}

	/// <summary>
	/// この弾を破棄する。
	/// </summary>
	public virtual void DestroyBullet()
	{
		BulletManager.Instance.CheckRemovingBullet( this );
		gameObject.SetActive( false );
	}

	/// <summary>
	/// この弾の軌道情報を上書きする。
	/// </summary>
	public void ChangeOrbital( BulletOrbitalParam orbitalParam )
	{

	}
}
