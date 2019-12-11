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
    /// �ʏ�e�����
    /// </summary>
    public virtual void ShotBullet()
    {
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.ShotSe);
    }

    /// <summary>
    /// �`���[�W�J�n�̈�x�����Ă΂��
    /// </summary>
    public virtual void ChargeStart()
    {
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.ChargeSe);
    }

    /// <summary>
    /// �`���[�W���Ă΂ꑱ����
    /// </summary>
    public virtual void ChargeUpdate()
    {

    }

    /// <summary>
    /// �`���[�W��������u�ԂɌĂ΂��
    /// </summary>
    public virtual void ChargeRelease()
    {
        // �`���[�W��������u�ԂɃ��[�U�[���{�����̎��ʂ��ł��Ă��Ȃ���SE�̃^�C�~���O������Ȃ�
        var playerManager = BattleRealPlayerManager.Instance;
        if (playerManager.IsLaserType)
        {
            AudioManager.Instance.Play(playerManager.ParamSet.LaserSe);
        }
        else
        {
            AudioManager.Instance.Play(playerManager.ParamSet.BombSe);
        }
    }

    /// <summary>
    /// ���[�U�[�����
    /// �`���[�W���������ɌĂ΂�邱�Ƃ�z�肵�Ă���
    /// </summary>
    public virtual void ShotLaser()
    {
    }

    /// <summary>
    /// �{�������
    /// �`���[�W���������ɌĂ΂�邱�Ƃ�z�肵�Ă���
    /// </summary>
    public virtual void ShotBomb()
    {
    }

    /// <summary>
    /// �����؂�ւ������ɌĂ΂��
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

        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.GetItemSe);
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
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.DeadSe);
        BattleRealManager.Instance.DeadPlayer();
    }

    public virtual void SetInvinsible()
    {

    }
}
