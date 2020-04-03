using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingEnemyController : CommandCharaController
{
    #region Field

    private string m_LookId;

    public BattleHackingEnemyGenerateParamSet GenerateParamSet { get; private set; }

    public BattleHackingEnemyBehaviorParamSet BehaviorParamSet { get; private set; }

    /// <summary>
    /// 敵キャラのサイクル。
    /// </summary>
    private E_POOLED_OBJECT_CYCLE m_Cycle;

    protected Vector2 PrePosition { get; private set; }

    protected Vector2 MoveDir { get; private set; }

    /// <summary>
    /// 画面外に出た時に自動的に破棄するかどうか
    /// </summary>
    protected bool m_WillDestroyOnOutOfEnemyField;

    /// <summary>
    /// 移動方向を常に正面とするかどうか
    /// </summary>
    protected bool m_IsLookMoveDir;

    public bool IsBoss { get; private set; }

    /// <summary>
    /// 出現して以降、画面に映ったかどうか
    /// </summary>
    protected bool IsShowFirst { get; private set; }

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

    public override void OnInitialize()
    {
        base.OnInitialize();

        Troop = E_CHARA_TROOP.ENEMY;
        IsShowFirst = false;
        m_WillDestroyOnOutOfEnemyField = true;
        m_IsLookMoveDir = true;

        if (GenerateParamSet != null)
        {
            InitHp(GenerateParamSet.Hp);
        }

        m_MaterialEffect = GetComponent<MaterialEffect>();
        m_MaterialEffect?.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_MaterialEffect?.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        m_MaterialEffect?.OnStart();
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

        IsOutOfEnemyField = BattleHackingEnemyManager.Instance.IsOutOfField(this);
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

    public void SetParamSet(BattleHackingEnemyGenerateParamSet generateParamSet, BattleHackingEnemyBehaviorParamSet behaviorParamSet)
    {
        GenerateParamSet = generateParamSet;
        BehaviorParamSet = behaviorParamSet;

        IsBoss = generateParamSet.IsBoss;

        SetBulletSetParam(BehaviorParamSet.BulletSetParam);

        OnSetParamSet();
    }

    protected virtual void OnSetParamSet()
    {

    }

    protected override void OnEnterSufferBullet(HitSufferData<BattleHackingFreeTrajectoryBulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var bullet = sufferData.OpponentObject;
        if (BattleRealStageManager.Instance.IsOutOfField(bullet.transform))
        {
            return;
        }

        Damage(1);
    }

    protected override void OnDamage()
    {
        base.OnDamage();
        AudioManager.Instance.Play(E_COMMON_SOUND.ENEMY_DAMAGE_01);

        if (m_MaterialEffect != null)
        {
            var mat = GenerateParamSet.DamageEffectMaterial;
            var dur = GenerateParamSet.DamageEffectDuration;
            m_MaterialEffect.ChangeMaterial(mat, dur);
        }
    }

    public override void Dead()
    {
        base.Dead();

        if (GenerateParamSet != null)
        {
            DataManager.Instance.BattleData.AddScore(GenerateParamSet.Score);
        }

        Destroy();
    }

    public void Destroy()
    {
        BattleHackingEnemyManager.Instance.DestroyEnemy(this);
    }
}
