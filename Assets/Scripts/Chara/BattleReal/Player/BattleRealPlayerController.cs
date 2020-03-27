#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ���A�����[�h�̃v���C���[�R���g���[��
/// </summary>
public partial class BattleRealPlayerController : BattleRealCharaController
{
    #region Define

    private enum E_STATE
    {
        /// <summary>
        /// ���A�����[�h��GAME�X�e�[�g�łȂ����ɑJ�ڂ���f�t�H���g�X�e�[�g
        /// </summary>
        NON_GAME,

        /// <summary>
        /// ���A�����[�h��GAME�X�e�[�g�̎��ɑJ�ڂ���f�t�H���g�X�e�[�g
        /// </summary>
        GAME,

        /// <summary>
        /// ���A�����[�h��GAME�X�e�[�g�̎��A���`���[�W���̎��ɑJ�ڂ���X�e�[�g
        /// </summary>
        CHARGE,

        /// <summary>
        /// �`���[�W�V���b�g����u�Ԃ����J�ڂ���X�e�[�g
        /// </summary>
        CHARGE_SHOT,

        /// <summary>
        /// �V�[�P���X�ɂ�鎩��������󂯂�X�e�[�g
        /// </summary>
        SEQUENCE,

        /// <summary>
        /// ���j���ꂽ�u�Ԃ����J�ڂ���X�e�[�g
        /// </summary>
        DEAD,
    }

    private class StateCycle : StateCycleBase<BattleRealPlayerController, E_STATE> { }

    private class InnerState : State<E_STATE, BattleRealPlayerController>
    {
        public InnerState(E_STATE state, BattleRealPlayerController target) : base(state, target) { }
        public InnerState(E_STATE state, BattleRealPlayerController target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    private const float INVINSIBLE_DURATION = 5f;
    private const string INVINSIBLE_KEY = "Invinsible";

    #region Field Inspector

    [SerializeField]
    private Transform[] m_MainShotPosition;

    [SerializeField]
    private float m_MainShotInterval;

    [SerializeField]
    private Transform m_Critical;

    [SerializeField]
    private Animator m_ShieldAnimator;

    [SerializeField]
    private Transform m_Shield;

    #endregion

    #region Field

    private StateMachine<E_STATE, BattleRealPlayerController> m_StateMachine;
    private BattleRealPlayerManagerParamSet m_ParamSet;

    private SequenceController m_SequenceController;
    private SequenceGroup m_SequenceGroup;

    private float m_ShotDelay;
    private BattleCommonEffectController m_ChargeEffect;
    private BulletController m_Laser;
    private BulletController m_Bomb;
    private bool m_IsExistEnergyCharge;

    public bool IsLaserType { get; private set; }
    public bool IsRestrictPosition;

    #endregion

    #region Open Callback

    public Action<bool> ChangeWeaponTypeAction { get; set; }

    #endregion

    #region Closed Callback

    private Action DeadPlayerAction { get; set; }

    #endregion

    #region Game Cycle

    private void Start()
    {
        BattleRealPlayerManager.RegisterPlayer(this);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_STATE, BattleRealPlayerController>();
        m_StateMachine.AddState(new InnerState(E_STATE.NON_GAME, this, new NonGameState()));
        m_StateMachine.AddState(new InnerState(E_STATE.GAME, this, new GameState()));
        m_StateMachine.AddState(new InnerState(E_STATE.CHARGE, this, new ChargeState()));
        m_StateMachine.AddState(new InnerState(E_STATE.CHARGE_SHOT, this, new ChargeShotState()));
        m_StateMachine.AddState(new InnerState(E_STATE.SEQUENCE, this, new SequenceState()));
        m_StateMachine.AddState(new InnerState(E_STATE.DEAD, this, new DeadState()));

        Troop = E_CHARA_TROOP.PLAYER;
        IsLaserType = m_ParamSet == null ? true : m_ParamSet.IsLaserType;
        IsRestrictPosition = false;

        SetEnableCollider(true);
        m_Shield.gameObject.SetActive(false);

        m_SequenceController = GetComponent<SequenceController>();
        if (m_SequenceController == null)
        {
            m_SequenceController = gameObject.AddComponent<SequenceController>();
        }
        m_SequenceController?.OnInitialize();

        // �Ƃ肠����NON_GAME�֑J�ڂ��đҋ@���Ă���
        RequestChangeState(E_STATE.GAME);
    }

    public override void OnFinalize()
    {
        m_SequenceController?.OnFinalize();
        ChangeWeaponTypeAction = null;
        m_StateMachine.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_StateMachine.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_StateMachine.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        m_StateMachine.OnFixedUpdate();
    }

    #endregion

    /// <summary>
    /// BattleRealPlayerController�̃X�e�[�g��ύX����B
    /// </summary>
    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    /// <summary>
    /// ���W�𓮑̃t�B�[���h�̈�ɐ�������B
    /// </summary>
    private void RestrictPosition()
    {
        var stageManager = BattleRealStageManager.Instance;
        stageManager.ClampMovingObjectPosition(transform);
    }

    public void SetParam(BattleRealPlayerManagerParamSet param)
    {
        m_ParamSet = param;
    }

    public void SetCallback(BattleRealPlayerManager manager)
    {
        DeadPlayerAction += manager.OnDeadPlayer;
    }

    public bool IsUsingChargeShot()
    {
        var useLaser = m_Laser != null && m_Laser.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED;
        var useBomb = m_Bomb != null && m_Bomb.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED;
        return useLaser || useBomb;
    }

    /// <summary>
    /// �ʏ�e�����
    /// </summary>
    private void ShotBullet()
    {
        if (m_ShotDelay >= m_MainShotInterval)
        {
            var levelParam = DataManager.Instance.BattleData.GetCurrentLevelParam();
            for (int i = 0; i < m_MainShotPosition.Length; i++)
            {
                var shotParam = new BulletShotParam(this);
                shotParam.Position = m_MainShotPosition[i].transform.position;
                var bullet = BulletController.ShotBullet(shotParam);

                // ����́A���[�U�[�^�C�v�̒ʏ�e�������g��
                bullet.SetNowDamage(levelParam.LaserTypeShotDamage);

                // �_�E���_���[�W��ݒ肷��
                switch (bullet)
                {
                    case BattleRealPlayerMainBullet hackerBullet:
                        hackerBullet.SetNowDownDamage(levelParam.LaserTypeShotDownDamage);
                        break;
                }
            }
            m_ShotDelay = 0;
        }

        // �����Ă����SE��炵�����̂ŁA�v���C���[�e��SE�Đ����̃^�C�~���O�ōs��
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.ShotSe);
    }

    /// <summary>
    /// �`���[�W�J�n�̈�x�����Ă΂��
    /// </summary>
    private void ChargeStart()
    {
        if (IsUsingChargeShot())
        {
            return;
        }

        var battleData = DataManager.Instance.BattleData.EnergyCount;
        m_IsExistEnergyCharge = battleData > 0;

        if (!m_IsExistEnergyCharge)
        {
            return;
        }

        if (m_ChargeEffect == null || m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.POOLED)
        {
            var paramSet = BattleRealPlayerManager.Instance.ParamSet;
            AudioManager.Instance.Play(paramSet.ChargeSe);
            m_ChargeEffect = BattleRealEffectManager.Instance.CreateEffect(paramSet.ChargeEffectParam, transform);
        }
    }

    /// <summary>
    /// �`���[�W��������u�ԂɌĂ΂��
    /// </summary>
    private void ChargeEnd()
    {
        if (IsUsingChargeShot() || !m_IsExistEnergyCharge)
        {
            return;
        }

        // �`���[�W��������u�ԂɃ��[�U�[���{�����̎��ʂ��ł��Ă��Ȃ���SE�̃^�C�~���O������Ȃ�
        if (IsLaserType)
        {
            AudioManager.Instance.Play(m_ParamSet.LaserSe);
        }
        else
        {
            AudioManager.Instance.Play(m_ParamSet.BombSe);
        }

        DataManager.Instance.BattleData.ConsumeEnergyCount(1);
        //BattleRealManager.Instance.RequestChangeState(E_BATTLE_REAL_STATE.CHARGE_SHOT);
    }

    /// <summary>
    /// ���[�U�[�����
    /// �`���[�W���������ɌĂ΂�邱�Ƃ�z�肵�Ă���
    /// </summary>
    public void ShotLaser()
    {
        if (IsUsingChargeShot())
        {
            return;
        }

        if (m_Laser != null && m_Laser.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED)
        {
            return;
        }

        if (m_ChargeEffect != null && m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            m_ChargeEffect.DestroyEffect(true);
        }

        var param = new BulletShotParam(this);
        param.Position = m_MainShotPosition[0].transform.position;
        m_Laser = BulletController.ShotBullet(param, true);

        var levelParam = DataManager.Instance.BattleData.GetCurrentLevelParam();
        m_Laser.SetNowDamage(levelParam.LaserDamagePerSeconds, E_RELATIVE.ABSOLUTE);
    }

    /// <summary>
    /// �{�������
    /// �`���[�W���������ɌĂ΂�邱�Ƃ�z�肵�Ă���
    /// </summary>
    public void ShotBomb()
    {
        if (IsUsingChargeShot())
        {
            return;
        }

        if (m_Bomb != null && m_Bomb.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED)
        {
            return;
        }

        if (m_ChargeEffect != null && m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            m_ChargeEffect.DestroyEffect(true);
        }

        var param = new BulletShotParam(this);
        param.BulletIndex = 1;
        param.BulletParamIndex = 1;
        m_Bomb = BulletController.ShotBullet(param, true);

        var levelParam = DataManager.Instance.BattleData.GetCurrentLevelParam();
        m_Bomb.SetNowDamage(levelParam.BombDamage, E_RELATIVE.ABSOLUTE);
    }

    /// <summary>
    /// �����؂�ւ������ɌĂ΂��
    /// </summary>
    private void ChangeWeapon()
    {
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.WeaponChangeSe);
    }

    public void SetInvinsible()
    {
        DestroyTimer(INVINSIBLE_KEY);
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, INVINSIBLE_DURATION);
        timer.SetTimeoutCallBack(() =>
        {
            timer = null;
            SetEnableCollider(true);
            m_Shield.gameObject.SetActive(false);
        });
        RegistTimer(INVINSIBLE_KEY, timer);

        m_Shield.gameObject.SetActive(true);
        m_ShieldAnimator.Play("battle_real_player_shield", 0);
        SetEnableCollider(false);
    }

    /// <summary>
    /// �`���[�W��`���[�W�V���b�g�������I�ɏI��������B
    /// </summary>
    public void StopChargeShot()
    {

        if (m_Laser != null)
        {
            m_Laser.DestroyBullet();
            m_Laser = null;
        }

        if (m_Bomb != null)
        {
            m_Bomb.DestroyBullet();
            m_Bomb = null;
        }

        if (m_ChargeEffect != null)
        {
            m_ChargeEffect.DestroyEffect(true);
            m_ChargeEffect = null;
        }

        AudioManager.Instance.Stop(E_CUE_SHEET.PLAYER);
    }

    private void SetEnableCollider(bool isEnable)
    {
        var c = GetCollider();

        // ��e����Ɩ��G����͔��΂̊֌W
        c.SetEnableCollider(m_Critical, isEnable);
        c.SetEnableCollider(m_Shield, !isEnable);
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

    protected override void OnEnterSufferChara(HitSufferData<BattleRealCharaController> sufferData)
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

        // ���SSE�͐F�X�ȏ����̌�ɂ��Ă����Ȃ��ƁA�v���C���[SE�̒�~�Ɋ������܂��\��������
        AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.DeadSe);
        DeadPlayerAction?.Invoke();
    }

    public void MoveBySequence(SequenceGroup sequenceGroup)
    {
        m_SequenceGroup = sequenceGroup;
        RequestChangeState(E_STATE.SEQUENCE);
    }
}
