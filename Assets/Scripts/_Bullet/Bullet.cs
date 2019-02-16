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

	public override void OnStart()
	{
		base.OnStart();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();
	}

	/// <summary>
	/// この弾を発射させる。
	/// </summary>
	public void ShotBullet()
	{
		BulletManager.Instance.AddBullet( this );
	}

	/// <summary>
	/// この弾を破棄する。
	/// </summary>
	public virtual void DestroyBullet()
	{
		BulletManager.Instance.RemoveBullet( this );
		gameObject.SetActive( false );
	}
}
