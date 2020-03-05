using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleRealEnemyController : CharaController
{
    #region Field

    private string m_LookId;

    protected BattleRealEnemyGenerateParamSet GenerateParamSet { get; private set; }

    protected BattleRealEnemyBehaviorParamSet BehaviorParamSet { get; private set; }

    private E_POOLED_OBJECT_CYCLE m_Cycle;

    protected Vector2 PrePosition { get; private set; }

    protected Vector2 MoveDir { get; private set; }

    /// <summary>
    /// 移動方向を常に正面とするかどうか
    /// </summary>
    protected bool m_IsLookMoveDir;

    /// <summary>
    /// 画面外に出た時に自動的に破棄するかどうか
    /// </summary>
    protected bool m_WillDestroyOnOutOfEnemyField;

    /// <summary>
    /// 出現して以降、画面に映ったかどうか
    /// </summary>
    protected bool IsShowFirst { get; private set; }

    public bool IsBoss { get; protected set; }

    /// <summary>
    /// 既に死亡しているか
    /// </summary>
    public bool IsDead { get; private set; }

    public bool IsOutOfEnemyField { get; private set; }

    private MaterialEffect m_MaterialEffect;

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

    public override void OnInitialize()
    {
        base.OnInitialize();

        Troop = E_CHARA_TROOP.ENEMY;
        IsShowFirst = false;
        m_IsLookMoveDir = true;
        IsBoss = false;
        IsDead = false;
        m_WillDestroyOnOutOfEnemyField = true;

        if (GenerateParamSet != null)
        {
            InitHp(GenerateParamSet.Hp);
        }

        m_MaterialEffect = GetComponent<MaterialEffect>();
        m_MaterialEffect?.OnInitialize();

        OnInitializeCollider();
    }

    protected virtual void OnInitializeCollider()
    {
        GetCollider().SetEnableAllCollider(true);
    }

    public override void OnFinalize()
    {
        m_MaterialEffect?.OnFinalize();
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
        if (m_IsLookMoveDir)
        {
            transform.LookAt(transform.position + 100 * MoveDir.ToVector3XZ());
        }

        IsOutOfEnemyField = BattleRealEnemyManager.Instance.IsOutOfField(this);
        if (IsOutOfEnemyField)
        {
            if (IsShowFirst)
            {
                Destroy();
            }
        }
        else
        {
            IsShowFirst = true;
        }
    }

    #endregion

    public void SetParamSet(BattleRealEnemyGenerateParamSet generateParamSet, BattleRealEnemyBehaviorParamSet behaviorParamSet)
    {
        GenerateParamSet = generateParamSet;
        BehaviorParamSet = behaviorParamSet;

        SetBulletSetParam(BehaviorParamSet.BulletSetParam);

        OnSetParamSet();
    }

    protected virtual void OnSetParamSet()
    {

    }

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
            var mat = GenerateParamSet.DamageEffectMaterial;
            var dur = GenerateParamSet.DamageEffectDuration;
            m_MaterialEffect.ChangeMaterial(mat, dur);
        }
    }

    public sealed override void Dead()
    {
        base.Dead();

        IsDead = true;
        GetCollider().SetEnableAllCollider(false);
        OnDead();
    }

    protected virtual void OnDead()
    {
        float defeatTime = 0;

        if (GenerateParamSet != null)
        {
            // シーケンシャルエフェクトを登録する
            var effect = GenerateParamSet.DefeatSequentialEffect;
            if (effect != null)
            {
                BattleRealEffectManager.Instance.RegisterSequentialEffect(effect, transform);
            }

            defeatTime = GenerateParamSet.DefeatHideTime;
        }

        if (defeatTime <= 0)
        {
            OnDefeatDestroy();
        }
        else
        {
            var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, defeatTime);
            timer.SetTimeoutCallBack(() => OnDefeatDestroy());
            TimerManager.Instance.RegistTimer(timer);
        }

        ExecuteDefeatEvent();
    }

    protected void OnDefeatDestroy()
    {
        BattleRealItemManager.Instance.CreateItem(transform.position, GenerateParamSet.DefeatItemParam);
        DataManager.Instance.BattleData.AddScore(GenerateParamSet.DefeatScore);

        Destroy();
    }

    protected void ExecuteDefeatEvent()
    {
        var events = GenerateParamSet.DefeatEvents;
        if (events != null)
        {
            for (int i = 0; i < events.Length; i++)
            {
                BattleRealEventManager.Instance.AddEvent(events[i]);
            }
        }
    }

    /// <summary>
    /// 死亡ではなく強制削除
    /// </summary>
    public void Destroy()
    {
        BattleRealEnemyManager.Instance.DestroyEnemy(this);
    }
}
