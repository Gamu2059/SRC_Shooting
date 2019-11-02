using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���A�����[�h�̃v���C���[�R���g���[��
/// </summary>
public class BattleRealPlayerController : CharaController
{
    /// <summary>
    /// �v���C���[�L�����̃��C�t�T�C�N��
    /// </summary>
    [System.Serializable]
    public enum E_PLAYER_LIFE_CYCLE
    {
        /// <summary>
        /// �퓬��ʂɂ͏o�Ă��Ȃ�
        /// </summary>
        AHEAD,

        /// <summary>
        /// ���ݐ퓬��
        /// </summary>
        SORTIE,

        /// <summary>
        /// ���S�ɂ��퓬��ʂ���ޏ�
        /// </summary>
        DEAD,
    }

    #region Field

    private BattleRealPlayerParamSet m_ParamSet;

    private float m_ShotRemainTime;

    #endregion

    #region Game Cycle

    private void Start()
    {
        // �J������p�ŁA�����I�Ƀ}�l�[�W���ɃL������ǉ����邽�߂�Unity��Start��p���Ă��܂�
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

    /// <summary>
    /// �ʏ�e�𔭎˂���B
    /// </summary>
    public virtual void ShotBullet()
    {
        AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.PLAYER, "SE_Player_Shot01");
    }

    public void ChargeLaser()
    {

    }

    public virtual void ShotLaser()
    {

    }

    public void ChargeBomb()
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

        switch (item.ItemType)
        {
            case E_ITEM_TYPE.SMALL_SCORE:
                BattleRealPlayerManager.Instance.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_SCORE:
                BattleRealPlayerManager.Instance.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_SCORE_UP:
                BattleRealPlayerManager.Instance.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_SCORE_UP:
                BattleRealPlayerManager.Instance.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_EXP:
                BattleRealPlayerManager.Instance.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_EXP:
                BattleRealPlayerManager.Instance.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_BOMB:
                BattleRealPlayerManager.Instance.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_BOMB:
                BattleRealPlayerManager.Instance.AddExp(item.ItemPoint);
                break;
        }
    }

    public int GetLevel()
    {
        //return BattleRealPlayerManager.Instance.GetCurrentLevel().Value;
        return 0;
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
