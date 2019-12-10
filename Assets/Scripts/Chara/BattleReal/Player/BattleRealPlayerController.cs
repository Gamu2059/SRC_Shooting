using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealPlayerController : CharaController
{
    #region Field

    private BattleRealPlayerParamSet m_ParamSet;

    private float m_ShotRemainTime;

    #endregion

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
        m_ShotRemainTime = 0;
        StartOutRingAnimation();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_ShotRemainTime -= Time.deltaTime;
    }

    #endregion

    public void SetParamSet(BattleRealPlayerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    public virtual void ShotBullet()
    {
        AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.PLAYER, "SE_Player_Shot01");
    }

    public virtual void ChargeUpdate()
    {

    }

    public virtual void ChargeShot()
    {

    }

    public virtual void ShotLaser()
    {

    }

    public virtual void ShotBomb()
    {

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
        if (item == null)
        {
            return;
        }

        AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.PLAYER, "SE_Player_Getitem");
        var battleData = DataManager.Instance.BattleData;

        switch (item.ItemType)
        {
            case E_ITEM_TYPE.SMALL_SCORE:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_SCORE:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_SCORE_UP:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_SCORE_UP:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_EXP:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_EXP:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_BOMB:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_BOMB:
                battleData.AddExp(item.ItemPoint);
                break;
        }
    }

    public override void Dead()
    {
        if (BattleManager.Instance.m_PlayerNotDead)
        {
            return;
        }

        base.Dead();
        AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.PLAYER, "SE_Player_Hit");
        BattleRealManager.Instance.DeadPlayer();
    }

    public virtual void SetInvinsible()
    {

    }
}
