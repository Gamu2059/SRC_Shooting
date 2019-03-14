using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionBase
{
	/// <summary>
	/// 当たり判定情報を取得する。
	/// 複数あることを考慮して配列として取得する。
	/// </summary>
	CollisionManager.ColliderData[] GetColliderData();

	/// <summary>
	/// このオブジェクトが他の弾に当たるか。
	/// </summary>
	bool CanHitBullet();

	/// <summary>
	/// キャラと当たり続けている時の処理。
	/// </summary>
	void OnHitCharacter( CharaControllerBase chara );

	/// <summary>
	/// 弾と当たり続けている時の処理。
	/// </summary>
	void OnHitBullet( BulletController bullet );

	/// <summary>
	/// 被弾し続けているの処理。
	/// </summary>
	void OnSuffer( BulletController bullet, CollisionManager.ColliderData colliderData );
}
