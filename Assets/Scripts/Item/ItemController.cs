using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 全てのアイテムオブジェクトの基礎クラス。
/// </summary>
[RequireComponent(typeof(BattleObjectCollider))]
public class ItemController : ControllableMonoBehaviour, ICollisionBase
{
    [Serializable]
    public enum E_ITEM_CYCLE
    {
        /// <summary>
        /// 発射される直前。
        /// </summary>
        STANDBY_UPDATE,

        /// <summary>
        /// 発射された後、動いている状態。
        /// </summary>
        UPDATE,

        /// <summary>
        /// プールされる準備状態。
        /// </summary>
        STANDBY_POOL,

        /// <summary>
        /// プーリングされた状態。
        /// </summary>
        POOLED,
    }

    private BattleObjectCollider m_Collider;

    /// <summary>
	/// この弾が発射された瞬間に呼び出される処理。
	/// </summary>
	public override void OnInitialize()
    {
        base.OnInitialize();

        if (m_Collider == null)
        {
            m_Collider = GetComponent<BattleObjectCollider>();
        }

        var m_TimerDict = new Dictionary<string, Timer>();
    }

    protected virtual void OnBecameInvisible()
    {
        // 画面から見えなくなったら弾を破棄する
        //DestroyBullet();
    }

    /// <summary>
	/// この弾の当たり判定情報を取得する。
	/// </summary>
	public virtual ColliderData[] GetColliderData()
    {
        return m_Collider.GetColliderData();
    }

    /// <summary>
    /// この弾が他の弾に衝突するかどうかを取得する。
    /// </summary>
    public virtual bool CanHitBullet()
    {
        return m_Collider.CanHitBullet();
    }

    /// <summary>
    /// この弾がキャラに衝突した時に呼び出される処理。
    /// </summary>
    /// <param name="chara">この弾に衝突されたキャラ</param>
    public virtual void OnHitCharacter(CharaController chara)
    {

    }

    /// <summary>
    /// この弾が他の弾に衝突した時に呼び出される処理。
    /// </summary>
    /// <param name="bullet">この弾に衝突された他の弾</param>
    public virtual void OnHitBullet(BulletController bullet)
    {

    }

    /// <summary>
    /// この弾が他の弾から衝突された時に呼び出される処理。
    /// OnHitBulletはこちら側から衝突した場合に対して、この処理は向こう側から衝突してきた場合に呼び出される。
    /// </summary>
    /// <param name="bullet">この弾に衝突してきた他の弾</param>
    /// <param name="colliderData">衝突を検出したこの弾の衝突情報</param>
    public virtual void OnSuffer(BulletController bullet, ColliderData colliderData)
    {

    }
}
