#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのプレイヤーコントローラ
/// </summary>
public partial class BattleRealPlayerController : BattleRealCharaController, IStateCallback<E_BATTLE_REAL_PLAYER_STATE>
{
    #region Define

    private class StateCycle : StateCycleBase<BattleRealPlayerController, E_BATTLE_REAL_PLAYER_STATE> { }

    private class InnerState : State<E_BATTLE_REAL_PLAYER_STATE, BattleRealPlayerController>
    {
        public InnerState(E_BATTLE_REAL_PLAYER_STATE state, BattleRealPlayerController target) : base(state, target) { }
        public InnerState(E_BATTLE_REAL_PLAYER_STATE state, BattleRealPlayerController target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    private const string INVINSIBLE_KEY = "Invinsible";

    #region Field Inspector

    [SerializeField]
    private Transform[] m_MainShotPosition;

    [SerializeField]
    private float m_MainShotInterval;

    #endregion

    #region Field

    private StateMachine<E_BATTLE_REAL_PLAYER_STATE, BattleRealPlayerController> m_StateMachine;
    private BattleRealPlayerManagerParamSet m_ParamSet;

    private SequenceController m_SequenceController;
    private SequenceGroup m_SequenceGroup;

    private Transform m_Critical;
    private Transform m_Shield;

    private E_BATTLE_REAL_PLAYER_STATE m_DefaultGameState;
    private float m_ShotDelay;
    private BattleCommonEffectController m_ShieldEffect;
    private BattleCommonEffectController m_ChargeEffect;
    private BulletController m_Laser;
    private BulletController m_Bomb;
    private bool m_IsExistEnergyCharge;

    public bool IsDead { get; private set; }
    public bool IsLaserType { get; private set; }
    public bool IsRestrictPosition;

    #endregion

    #region Open Callback

    public Action<bool> ChangeWeaponTypeAction { get; set; }
    public Action<E_BATTLE_REAL_PLAYER_STATE> ChangeStateAction { get; set; }

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_REAL_PLAYER_STATE, BattleRealPlayerController>();
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_PLAYER_STATE.NON_GAME, this, new NonGameState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_PLAYER_STATE.GAME, this, new GameState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_PLAYER_STATE.CHARGE, this, new ChargeState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_PLAYER_STATE.CHARGE_SHOT, this, new ChargeShotState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_PLAYER_STATE.SEQUENCE, this, new SequenceState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_PLAYER_STATE.DEAD, this, new DeadState()));

        Troop = E_CHARA_TROOP.PLAYER;
        IsLaserType = m_ParamSet == null ? true : m_ParamSet.IsLaserType;
        IsRestrictPosition = false;
        IsDead = false;
        m_DefaultGameState = E_BATTLE_REAL_PLAYER_STATE.NON_GAME;

        m_SequenceController = GetComponent<SequenceController>();
        if (m_SequenceController == null)
        {
            m_SequenceController = gameObject.AddComponent<SequenceController>();
        }
        m_SequenceController?.OnInitialize();

        m_Critical = GetCollider().GetColliderTransform(E_COLLIDER_TYPE.CRITICAL).Transform;
        m_Shield = GetCollider().GetColliderTransform(E_COLLIDER_TYPE.DEFAULT).Transform;
        SetEnableCollider(true);

        // とりあえずNON_GAMEへ遷移して待機しておく
        RequestChangeState(E_BATTLE_REAL_PLAYER_STATE.NON_GAME);
    }

    public override void OnFinalize()
    {
        m_SequenceController?.OnFinalize();
        ChangeStateAction = null;
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

    #region Change State

    /// <summary>
    /// BattleRealPlayerControllerのステートを変更する。
    /// </summary>
    private void RequestChangeState(E_BATTLE_REAL_PLAYER_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    /// <summary>
    /// デフォルトゲームステートを切り替える。
    /// </summary>
    public void SetDefaultGameState(bool isBattleRealManagerGameState)
    {
        m_DefaultGameState = isBattleRealManagerGameState ? E_BATTLE_REAL_PLAYER_STATE.GAME : E_BATTLE_REAL_PLAYER_STATE.NON_GAME;
    }

    /// <summary>
    /// プレイヤーのステートをデフォルトゲームステートに切り替える。
    /// </summary>
    private void RequestChangeDefaultGameState()
    {
        RequestChangeState(m_DefaultGameState);
    }

    /// <summary>
    /// 死亡ステートに切り替える。
    /// </summary>
    public void RequestChangeToDeadState()
    {
        RequestChangeState(E_BATTLE_REAL_PLAYER_STATE.DEAD);
    }

    #endregion

    /// <summary>
    /// 座標を動体フィールド領域に制限する。
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

    #region Shot

    /// <summary>
    /// 通常弾を放つ
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

                // 現状は、レーザータイプの通常弾だけを使う
                bullet.SetNowDamage(levelParam.LaserTypeShotDamage);

                // ダウンダメージを設定する
                switch (bullet)
                {
                    case BattleRealPlayerMainBullet hackerBullet:
                        hackerBullet.SetNowDownDamage(levelParam.LaserTypeShotDownDamage);
                        break;
                }
            }
            m_ShotDelay = 0;
        }

        // 押している間SEを鳴らしたいので、プレイヤー弾のSE再生このタイミングで行う
        AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_SHOT_01);
    }

    /// <summary>
    /// チャージ開始の一度だけ呼ばれる
    /// </summary>
    private void ChargeStart()
    {
        if (IsUsingChargeShot())
        {
            return;
        }

        m_IsExistEnergyCharge = DataManager.Instance.BattleData.EnergyStock.Value > 0;

        if (!m_IsExistEnergyCharge)
        {
            return;
        }

        if (m_ChargeEffect == null || m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.POOLED)
        {
            var paramSet = BattleRealPlayerManager.Instance.ParamSet;
            AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_CHARGE_01);
            m_ChargeEffect = BattleRealEffectManager.Instance.CreateEffect(paramSet.ChargeEffectParam, transform);
        }
    }

    /// <summary>
    /// チャージを放った瞬間に呼ばれる
    /// </summary>
    private void ChargeEnd()
    {
        if (IsUsingChargeShot() || !m_IsExistEnergyCharge)
        {
            return;
        }

        AudioManager.Instance.Stop(E_CUE_SHEET.PLAYER);
        // チャージを放った瞬間にレーザーかボムかの識別ができていないとSEのタイミングが合わない
        if (IsLaserType)
        {
            AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_LASER);
        }
        else
        {
            AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_BOMB);
        }

        DataManager.Instance.BattleData.ConsumeEnergyStock();
    }

    /// <summary>
    /// チャージショットを放つ
    /// </summary>
    private void ChargeShot()
    {
        BattleRealUiManager.Instance.FrontViewEffect.StopEffect();
        BattleRealEffectManager.Instance.ResumeAllEffect();

        if (IsLaserType)
        {
            ShotLaser();
            BattleRealCameraManager.Instance.Shake(m_ParamSet.LaserShakeParam);
        }
        else
        {
            ShotBomb();
            BattleRealCameraManager.Instance.Shake(m_ParamSet.BombShakeParam);
        }
    }

    /// <summary>
    /// レーザーを放つ
    /// チャージを放った後に呼ばれることを想定している
    /// </summary>
    private void ShotLaser()
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
    /// ボムを放つ
    /// チャージを放った後に呼ばれることを想定している
    /// </summary>
    private void ShotBomb()
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
    /// 武器を切り替えた時に呼ばれる
    /// </summary>
    private void ChangeWeapon()
    {
        AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_WEAPON_CHANGE);
    }

    #endregion

    /// <summary>
    /// 無敵状態にする。
    /// </summary>
    public void SetInvinsible(float invinsibleDuration, bool showShield = true)
    {
        DestroyTimer(INVINSIBLE_KEY);
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, invinsibleDuration);
        timer.SetTimeoutCallBack(() =>
        {
            timer = null;
            SetEnableCollider(true);

            m_ShieldEffect?.DestroyEffect(true);
            m_ShieldEffect = null;
        });
        RegistTimer(INVINSIBLE_KEY, timer);

        if (showShield)
        {
            m_ShieldEffect = BattleRealEffectManager.Instance.CreateEffect(m_ParamSet.ShieldEffectParam, transform);
        }
        SetEnableCollider(false);
    }

    /// <summary>
    /// チャージやチャージショットを強制的に終了させる。
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

    private bool IsUsingChargeShot()
    {
        var useLaser = m_Laser != null && m_Laser.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED;
        var useBomb = m_Bomb != null && m_Bomb.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED;
        return useLaser || useBomb;
    }

    private void SetEnableCollider(bool isEnable)
    {
        var c = GetCollider();

        // 被弾判定と無敵判定は反対の関係
        c.SetEnableCollider(m_Critical, isEnable);
        c.SetEnableCollider(m_Shield, !isEnable);
    }

    #region Suffer & Hit

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
            case E_COLLIDER_TYPE.ENEMY_MAIN_BODY:
            case E_COLLIDER_TYPE.ENEMY_SUB_BODY:
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

        AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_GET_ITEM);
        var battleData = DataManager.Instance.BattleData;

        switch (item.ItemType)
        {
            case E_ITEM_TYPE.LIFE_RECOVERY:
                battleData.AddPlayerLife(item.ItemPoint);
                break;
            //case E_ITEM_TYPE.SMALL_SCORE:
            //case E_ITEM_TYPE.BIG_SCORE:
            //    battleData.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_EXP:
            case E_ITEM_TYPE.BIG_EXP:
                battleData.AddExp(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_ENERGY:
            case E_ITEM_TYPE.BIG_ENERGY:
                battleData.AddEnergyCharge(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SECRET:
                battleData.IncreaseSecretItem();
                break;
            default:
                break;
        }
    }

    #endregion

    public override void Dead()
    {
        base.Dead();

        var testDataManager = BattleTestDataManager.Instance;
        if (testDataManager != null && testDataManager.IsNotPlayerDead)
        {
            return;
        }

        IsDead = true;
    }

    public void MoveBySequence(SequenceGroup sequenceGroup)
    {
        m_SequenceGroup = sequenceGroup;
        RequestChangeState(E_BATTLE_REAL_PLAYER_STATE.SEQUENCE);
    }
}
