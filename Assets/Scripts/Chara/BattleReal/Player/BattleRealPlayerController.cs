#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using BattleReal.BulletGenerator;

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

    #endregion

    #region Field

    private StateMachine<E_BATTLE_REAL_PLAYER_STATE, BattleRealPlayerController> m_StateMachine;
    private BattleRealPlayerManagerParamSet m_ParamSet;

    private SequenceController m_SequenceController;
    private SequenceGroup m_SequenceGroup;

    private Transform m_Critical;
    private Transform m_Shield;

    private E_BATTLE_REAL_PLAYER_STATE m_DefaultGameState;
    private BattleCommonEffectController m_ShieldEffect;
    private BattleCommonEffectController m_ChargeEffect;
    private BulletController m_Laser;
    private BulletController m_Bomb;
    private bool m_IsExistEnergyCharge;

    private PlayerNormalBulletGenerator m_NormalBulletGenerator;
    private PlayerLaserGenerator m_LaserGenerator;
    private PlayerBombGenerator m_BombGenerator;
    private IDisposable m_LaserDestory;
    private IDisposable m_BombDestroy;

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

        var bulletGeneratorParamSet = m_ParamSet.NormalBulletGeneratorParamSet;
        var bulletGenerator = BattleRealBulletGeneratorManager.Instance.CreateBulletGenerator(bulletGeneratorParamSet, this);
        m_NormalBulletGenerator = bulletGenerator as PlayerNormalBulletGenerator;

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

    public void OnChangeStateBattleRealManager(E_BATTLE_REAL_STATE state)
    {
        if (state == E_BATTLE_REAL_STATE.GAME)
        {
            m_DefaultGameState = E_BATTLE_REAL_PLAYER_STATE.GAME;
        }
        else
        {
            m_DefaultGameState = E_BATTLE_REAL_PLAYER_STATE.NON_GAME;
            StopShotBullet();
        }
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

    private void StartShotBullet()
    {
        m_NormalBulletGenerator?.StartShot();
    }

    private void StopShotBullet()
    {
        m_NormalBulletGenerator?.StopShot();
    }

    /// <summary>
    /// チャージエフェクトをつける
    /// </summary>
    private void FireChargeEffect()
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

        var existChargeEffect = !(m_ChargeEffect == null || m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.POOLED);
        if (existChargeEffect)
        {
            AudioManager.Instance.Stop(E_CUE_SHEET.PLAYER_CHARGE);
            m_ChargeEffect.DestroyEffect(true);
            m_ChargeEffect = null;
        }

        var chargeLevel = DataManager.Instance.BattleData.ChargeLevel.Value;
        var paramSet = BattleRealPlayerManager.Instance.ParamSet;
        var effect = paramSet.ChargeEffectParams[chargeLevel];
        m_ChargeEffect = BattleRealEffectManager.Instance.CreateEffect(effect, transform);
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

        AudioManager.Instance.Stop(E_CUE_SHEET.PLAYER_CHARGE);
        AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_CHARGE_SHOT);
    }

    /// <summary>
    /// チャージショットを放つ
    /// </summary>
    private void ChargeShot()
    {
        DataManager.Instance.BattleData.ConsumeEnergyStock();
        BattleRealUiManager.Instance.FrontViewEffect.StopEffect();
        BattleRealEffectManager.Instance.ResumeAllEffect();

        if (IsLaserType)
        {
            ShotLaser();
        }
        else
        {
            ShotBomb();
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

        if (m_ChargeEffect != null && m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            m_ChargeEffect.DestroyEffect(true);
        }

        var paramset = m_ParamSet.LaserGeneratorParamSet;
        var generator = BattleRealBulletGeneratorManager.Instance.CreateBulletGenerator(paramset, this);
        m_LaserGenerator = generator as PlayerLaserGenerator;
        m_LaserDestory = m_LaserGenerator.OnDestroyObservable.Subscribe(_ =>
        {
            m_LaserDestory?.Dispose();
            m_LaserDestory = null;
            m_LaserGenerator = null;
        });

        BattleRealCameraManager.Instance.Shake(m_ParamSet.LaserShakeParam);
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

        if (m_ChargeEffect != null && m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            m_ChargeEffect.DestroyEffect(true);
        }

        var paramset = m_ParamSet.BombGeneratorParamSet;
        var generator = BattleRealBulletGeneratorManager.Instance.CreateBulletGenerator(paramset, this);
        m_BombGenerator = generator as PlayerBombGenerator;
        m_BombDestroy = m_BombGenerator.OnDestroyObservable.Subscribe(_ =>
        {
            m_BombDestroy?.Dispose();
            m_BombDestroy = null;
            m_BombGenerator = null;
        });

        BattleRealCameraManager.Instance.Shake(m_ParamSet.BombShakeParam);
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
        m_LaserGenerator?.StopChargeShot();

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
        var useLaser = m_LaserGenerator != null && m_LaserGenerator.Cycle != E_POOLED_OBJECT_CYCLE.POOLED;
        var useBomb = m_BombGenerator != null && m_BombGenerator.Cycle != E_POOLED_OBJECT_CYCLE.POOLED;
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
            //    break;
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
