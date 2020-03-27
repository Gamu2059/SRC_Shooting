using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の基底クラス。
/// </summary>
public class BattleRealEnemyBase : BattleRealCharaController
{
    #region Field

    protected BattleRealEnemyParamBase Param { get; private set; }

    protected Vector2 PrePosition { get; private set; }

    protected Vector2 MoveDir { get; private set; }

    private string m_LookId;

    private E_POOLED_OBJECT_CYCLE m_Cycle;

    /// <summary>
    /// 移動方向を常に正面とするかどうか
    /// </summary>
    public bool IsLookMoveDir;

    /// <summary>
    /// 画面外に出た時に自動的に破棄するかどうか
    /// </summary>
    public bool WillDestroyOnOutOfEnemyField;

    /// <summary>
    /// ボスかどうか
    /// </summary>
    public bool IsBoss { get; protected set; }

    /// <summary>
    /// 既に死亡しているか
    /// </summary>
    public bool IsDead { get; protected set; }

    /// <summary>
    /// 敵フィールドの範囲外にいるかどうか
    /// </summary>
    public bool IsOutOfEnemyField { get; private set; }

    /// <summary>
    /// 出現して以降、画面に映ったかどうか
    /// </summary>
    protected bool IsShowFirst { get; private set; }

    private MaterialEffect m_MaterialEffect;

    protected SequenceController SequenceController { get; private set; }

    #endregion

    #region Get & Set

    public string GetLookId()
    {
        return m_LookId;
    }

    public void SetLookId(string id)
    {
        m_LookId = id;
    }

    public E_POOLED_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    public void SetCycle(E_POOLED_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    #endregion

    #region Game Cycle

    private void Start()
    {
        // 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
        BattleRealEnemyManager.RegisterEnemy(this);
    }

    public void SetParam(BattleRealEnemyParamBase param)
    {
        Param = param;
        SetBulletSetParam(param.BulletSetParam);
        OnSetParamSet();
    }
    protected virtual void OnSetParamSet() { }

    public override void OnInitialize()
    {
        base.OnInitialize();

        Troop = E_CHARA_TROOP.ENEMY;
        IsShowFirst = false;
        IsLookMoveDir = true;
        IsBoss = false;
        IsDead = false;
        WillDestroyOnOutOfEnemyField = true;

        if (Param != null)
        {
            InitHp(Param.Hp);
        }

        m_MaterialEffect = GetComponent<MaterialEffect>();
        m_MaterialEffect?.OnInitialize();

        SequenceController = GetComponent<SequenceController>();
        if (SequenceController == null)
        {
            SequenceController = gameObject.AddComponent<SequenceController>();
        }
        SequenceController?.OnInitialize();

        OnInitializeCollider();
    }

    protected virtual void OnInitializeCollider()
    {
        GetCollider().SetEnableAllCollider(true);
    }

    public override void OnStart()
    {
        base.OnStart();

        // OnInitializeの時点では生成直後であり、その後に生成側で座標を初期化している可能性がある
        // そのためサイクルに乗った最初の時にPrePositionを初期化する
        PrePosition = transform.position.ToVector2XZ();
    }

    public override void OnFinalize()
    {
        SequenceController?.OnFinalize();
        m_MaterialEffect?.OnFinalize();
        SequenceController = null;
        m_MaterialEffect = null;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_MaterialEffect?.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        var pos = transform.position.ToVector2XZ();
        MoveDir = pos - PrePosition;
        PrePosition = pos;
        if (IsLookMoveDir)
        {
            transform.LookAt(transform.position + MoveDir.ToVector3XZ(), transform.up);
        }

        IsOutOfEnemyField = BattleRealEnemyManager.Instance.IsOutOfField(this);
        if (IsOutOfEnemyField)
        {
            if (IsShowFirst && WillDestroyOnOutOfEnemyField)
            {
                OnRetireDestroy();
            }
        }
        else
        {
            IsShowFirst = true;
        }
    }

    #endregion

    #region Suffer

    protected override void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var bullet = sufferData.OpponentObject;
        var stageM = BattleRealStageManager.Instance;
        if (stageM.IsOutOfField(transform) || stageM.IsOutOfField(bullet.transform))
        {
            return;
        }

        var sufferCollider = sufferData.SufferCollider;
        if (sufferCollider.Transform.ColliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            var hitCollider = sufferData.HitCollider;
            switch (hitCollider.Transform.ColliderType)
            {
                case E_COLLIDER_TYPE.PLAYER_BULLET:
                case E_COLLIDER_TYPE.PLAYER_BOMB:
                    Damage(bullet.GetNowDamage());
                    break;
                case E_COLLIDER_TYPE.PLAYER_LASER:
                    // レーザーは1秒あたりのダメージが入っているのでこうする
                    Damage(bullet.GetNowDamage() * Time.deltaTime);
                    break;
            }
        }
    }

    protected override void OnStaySufferBullet(HitSufferData<BulletController> sufferData)
    {
        base.OnStaySufferBullet(sufferData);

        var bullet = sufferData.OpponentObject;
        var sufferCollider = sufferData.SufferCollider;
        if (sufferCollider.Transform.ColliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            var hitCollider = sufferData.HitCollider;
            switch (hitCollider.Transform.ColliderType)
            {
                case E_COLLIDER_TYPE.PLAYER_BULLET:
                    Damage(bullet.GetNowDamage());
                    break;
                case E_COLLIDER_TYPE.PLAYER_LASER:
                    // レーザーは1秒あたりのダメージが入っているのでこうする
                    Damage(bullet.GetNowDamage() * Time.deltaTime);
                    break;
            }
        }
    }

    protected override void OnDamage()
    {
        base.OnDamage();
        AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.DamageSe);

        if (m_MaterialEffect != null)
        {
            var mat = Param.DamageEffectMaterial;
            var dur = Param.DamageEffectDuration;
            m_MaterialEffect.ChangeMaterial(mat, dur);
        }
    }

    #endregion

    public sealed override void Dead()
    {
        base.Dead();

        IsDead = true;
        GetCollider().SetEnableAllCollider(false);
        OnDead();
    }

    protected virtual void OnDead() { }

    protected void OnDefeatDestroy()
    {
        BattleRealItemManager.Instance.CreateItem(transform.position, Param.DefeatItemParam);
        DataManager.Instance.BattleData.AddScore(Param.DefeatScore);

        Destroy();
    }

    protected void ExecuteDefeatEvent()
    {
        BattleRealEventManager.Instance.AddEvent(Param.DefeatEvents);
    }

    protected void OnRetireDestroy()
    {
        Destroy();
        ExecuteRetireEvent();
    }

    protected void ExecuteRetireEvent()
    {
        BattleRealEventManager.Instance.AddEvent(Param.RetireEvents);
    }

    /// <summary>
    /// 死亡ではなく強制削除
    /// </summary>
    protected void Destroy()
    {
        BattleRealEnemyManager.Instance.DestroyEnemy(this);
    }
}
