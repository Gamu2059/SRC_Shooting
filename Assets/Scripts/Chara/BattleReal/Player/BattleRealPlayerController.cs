using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealPlayerController : CharaController
{
    #region Game Cycle

    private void Start()
    {
        BattleRealPlayerManager.RegisterPlayer(this);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        Troop = E_CHARA_TROOP.PLAYER;
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        StartOutRingAnimation();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    #endregion

    /// <summary>
    /// 通常弾を放つ
    /// </summary>
    public virtual void ShotBullet()
    {
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.ShotSe);
    }

    /// <summary>
    /// チャージ開始の一度だけ呼ばれる
    /// </summary>
    public virtual void ChargeStart()
    {
    }

    /// <summary>
    /// チャージ中呼ばれ続ける
    /// </summary>
    public virtual void ChargeUpdate()
    {

    }

    /// <summary>
    /// チャージを放った瞬間に呼ばれる
    /// </summary>
    public virtual void ChargeRelease()
    {
    }

    /// <summary>
    /// レーザーを放つ
    /// チャージを放った後に呼ばれることを想定している
    /// </summary>
    public virtual void ShotLaser()
    {
    }

    /// <summary>
    /// ボムを放つ
    /// チャージを放った後に呼ばれることを想定している
    /// </summary>
    public virtual void ShotBomb()
    {
    }

    /// <summary>
    /// 武器を切り替えた時に呼ばれる
    /// </summary>
    public virtual void ChangeWeapon()
    {
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.WeaponChangeSe);
    }

    protected override void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var hitColliderType = sufferData.HitCollider.Transform.ColliderType;
        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;

        switch (hitColliderType)
        {
            case E_COLLIDER_TYPE.ENEMY_BULLET:
            case E_COLLIDER_TYPE.ENEMY_LASER:
            case E_COLLIDER_TYPE.ENEMY_BOMB:
                if (selfColliderType == E_COLLIDER_TYPE.CRITICAL)
                {
                    Damage(1);
                }
                break;
        }
    }

    protected override void OnEnterSufferChara(HitSufferData<CharaController> sufferData)
    {
        base.OnEnterSufferChara(sufferData);

        var hitColliderType = sufferData.HitCollider.Transform.ColliderType;
        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;

        switch (hitColliderType)
        {
            case E_COLLIDER_TYPE.ENEMY_BULLET:
            case E_COLLIDER_TYPE.ENEMY_LASER:
            case E_COLLIDER_TYPE.ENEMY_BOMB:
                if (selfColliderType == E_COLLIDER_TYPE.CRITICAL)
                {
                    Damage(1);
                }
                break;
        }
    }

    protected override void OnEnterHitItem(HitSufferData<BattleRealItemController> hitData)
    {
        base.OnEnterHitItem(hitData);

        var playerColliderType = hitData.HitCollider.Transform.ColliderType;
        if (playerColliderType != E_COLLIDER_TYPE.ITEM_GAIN)
        {
            return;
        }

        var itemColliderType = hitData.SufferCollider.Transform.ColliderType;
        switch (itemColliderType)
        {
            case E_COLLIDER_TYPE.ITEM_ATTRACT:
                hitData.OpponentObject.AttractPlayer();
                break;
            case E_COLLIDER_TYPE.ITEM_GAIN:
                GetItem(hitData.OpponentObject);
                break;
            default:
                break;
        }
    }

    private void GetItem(BattleRealItemController item)
    {
        if (item == null || item.ItemType == E_ITEM_TYPE.NONE)
        {
            return;
        }

        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.GetItemSe);
        var battleData = DataManager.Instance.BattleData;

        switch (item.ItemType)
        {
            case E_ITEM_TYPE.LIFE_RECOVERY:
                battleData.AddPlayerLife(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_SCORE:
            case E_ITEM_TYPE.BIG_SCORE:
                battleData.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_EXP:
            case E_ITEM_TYPE.BIG_EXP:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_ENERGY:
            case E_ITEM_TYPE.BIG_ENERGY:
                battleData.AddEnergyCharge(item.ItemPoint);
                break;
            default:
                break;
        }
    }

    public override void Dead()
    {
        //if (BattleManager.Instance.m_PlayerNotDead)
        //{
        //    return;
        //}

        base.Dead();

        StopChargeShot();

        // 死亡SEは色々な処理の後にしておかないと、プレイヤーSEの停止に巻き込まれる可能性がある
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.DeadSe);
        BattleRealManager.Instance.DeadPlayer();
    }

    public virtual void SetInvinsible()
    {

    }

    /// <summary>
    /// チャージやチャージショットを強制的に終了させる。
    /// </summary>
    public virtual void StopChargeShot()
    {

    }
}
