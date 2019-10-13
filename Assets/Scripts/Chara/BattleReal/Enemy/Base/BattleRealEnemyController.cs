using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleRealEnemyController : CharaController
{
    public const string CAN_OUT_DESTROY_TIMER_KEY = "CanOutDestroyTimer";
    public const string HIT_INVINCIBLE_TIMER_KEY = "HitInvincibleTimer";

    [Space()]
    [Header("敵専用 パラメータ")]

    [SerializeField, Tooltip("ボスかどうか")]
    private bool m_IsBoss;

    [SerializeField, Tooltip("死亡時エフェクト")]
    private GameObject m_DeadEffect;

    [SerializeField, Tooltip("被弾直後の無敵時間")]
    private float m_OnHitInvincibleDuration;

    private string m_LookId;

    private BattleRealEnemyGenerateParamSet m_GenerateParamSet;
    protected BattleRealEnemyGenerateParamSet GenerateParamSet => m_GenerateParamSet;

    private BattleRealEnemyBehaviorParamSet m_BehaviorParamSet;
    protected BattleRealEnemyBehaviorParamSet BehaviorParamSet => m_BehaviorParamSet;


    /// <summary>
    /// 敵キャラのサイクル。
    /// </summary>
    private E_POOLED_OBJECT_CYCLE m_Cycle;

    public bool IsOutOfEnemyField { get; private set; }

    /// <summary>
    /// マスターデータから取得するパラメータセット
    /// </summary>
    protected ArgumentParamSet m_ParamSet;

    /// <summary>
    /// アイテムドロップパラメータ
    /// </summary>
    protected ItemCreateParam m_DropItemParam;

    /// <summary>
    /// 撃破時の変数操作パラメータ
    /// </summary>
    protected OperateVariableParam[] m_DefeatOperateVariableParams;



    #region Get & Set

    public string GetLookId()
    {
        return m_LookId;
    }

    public void SetLookId(string id)
    {
        m_LookId = id;
    }

    public ArgumentParamSet GetParamSet()
    {
        return m_ParamSet;
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



    private void Start()
    {
        // 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
        BattleRealEnemyManager.RegisterEnemy(this);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (m_GenerateParamSet != null)
        {
            InitHp(m_GenerateParamSet.Hp);
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        IsOutOfEnemyField = BattleRealEnemyManager.Instance.IsOutOfEnemyField(this);
        if (IsOutOfEnemyField)
        {
            Destroy();
        }
    }

    /// <summary>
    /// 引数をセットする
    /// </summary>
    public virtual void SetArguments(string param)
    {
        m_ParamSet = ArgumentParamSetTranslator.TranslateFromString(param);
    }

    public void SetParamSet(BattleRealEnemyGenerateParamSet paramSet)
    {
        m_GenerateParamSet = paramSet;
        m_BehaviorParamSet = m_GenerateParamSet.EnemyBehaviorParamSet;

        OnSetParamSet();
    }

    protected virtual void OnSetParamSet()
    {

    }

    protected virtual void OnBecameVisible()
    {
        //if (BattleRealTimerManager.Instance != null)
        //{
        //    SetCanOutDestroyTimer();
        //}
        //else
        //{
        //    m_OnInitialized += () => SetCanOutDestroyTimer();
        //}
    }

    /// <summary>
    /// 指定した弾がEchoBullet、かつ被弾済みのインデックスならばtrueを返す。
    /// </summary>
    protected bool IsSufferEchoBullet(BulletController bullet)
    {
        // EchoBulletかつ被弾済みならtrueを返す
        if (bullet is EchoBullet)
        {
            var echoBullet = bullet as EchoBullet;
            if (EchoBulletIndexGenerater.Instance.IsRegisteredChara(echoBullet.GetRootIndex(), this))
            {
                return true;
            }
        }

        return false;
    }

    public override void SufferBullet(BulletController attackBullet, ColliderData attackData, ColliderData targetData)
    {
        if (IsSufferEchoBullet(attackBullet))
        {
            return;
        }

        //if (BattleRealStageManager.Instance.IsOutOfField(attackBullet.transform))
        //{
        //    return;
        //}

        if (m_OnHitInvincibleDuration <= 0)
        {
            base.SufferBullet(attackBullet, attackData, targetData);
            return;
        }

        Timer timer = GetTimer(HIT_INVINCIBLE_TIMER_KEY);

        if (timer == null)
        {
            timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, m_OnHitInvincibleDuration, () =>
            {
                timer = null;
            });
            RegistTimer(HIT_INVINCIBLE_TIMER_KEY, timer);
            base.SufferBullet(attackBullet, attackData, targetData);
        }
    }

    public override void Dead()
    {
        base.Dead();

        //DestroyAllTimer();

        //BattleRealItemManager.Instance.CreateItem(transform.localPosition, m_DropItemParam);

        Destroy();
    }

    private void OperateEventVariable()
    {
        if (m_DefeatOperateVariableParams == null)
        {
            return;
        }

        //var eventContent = new EventContent();
        //eventContent.ExecuteTiming = EventContent.E_EXECUTE_TIMING.IMMEDIATE;
        //eventContent.EventType = EventContent.E_EVENT_TYPE.OPERATE_VARIABLE;
        //eventContent.OperateVariableParams = m_DefeatOperateVariableParams;
        //BattleRealEventManager.Instance.ExecuteEvent(eventContent);
    }

    public void Destroy()
    {
        BattleRealEnemyManager.Instance.DestroyEnemy(this);
    }
}
