using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : CharaController
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

    /// <summary>
    /// 敵キャラのサイクル。
    /// </summary>
    private E_OBJECT_CYCLE m_Cycle;

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

    /// <summary>
    /// 初期化時のコールバック
    /// </summary>
    public Action m_OnInitialized;

    /// <summary>
    /// 画面外に出た時に破棄するかどうか
    /// </summary>
    private bool m_CanOutDestroy;



    #region Getter & Setter

    public ArgumentParamSet GetParamSet()
    {
        return m_ParamSet;
    }

    public E_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    public void SetCycle(E_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    #endregion



    private void Start()
    {
        // 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
        EnemyCharaManager.Instance.RegistEnemy(this);
    }



    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CanOutDestroy = false;

        EventUtility.SafeInvokeAction(m_OnInitialized);
    }

    /// <summary>
    /// 引数をセットする
    /// </summary>
    public virtual void SetArguments(string param)
    {
        m_ParamSet = ArgumentParamSetTranslator.TranslateFromString(param);
    }

    /// <summary>
    /// 撃破時のドロップアイテムパラメータをセットする
    /// </summary>
    public void SetDropItemParam(string param)
    {
        m_DropItemParam = ItemCreateParamTranslator.TranslateFromString(param);
    }

    /// <summary>
    /// 撃破時の変数操作パラメータをセットする
    /// </summary>
    public void SetDefeatParam(string param)
    {
        m_DefeatOperateVariableParams = OperateVariableParamTranslator.TranslateFromString(param);
    }

    protected virtual void OnBecameVisible()
    {
        if (BattleMainTimerManager.Instance != null)
        {
            SetCanOutDestroyTimer();
        }
        else
        {
            m_OnInitialized += () => SetCanOutDestroyTimer();
        }
    }

    protected virtual void OnBecameInvisible()
    {
        if (m_CanOutDestroy)
        {
            EnemyCharaManager.Instance.DestroyEnemy(this);
        }
    }

    /// <summary>
    /// 破棄可能フラグを立てるためのタイマーを設定する
    /// </summary>
    private void SetCanOutDestroyTimer()
    {
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, EnemyCharaManager.Instance.GetCanOutTime());
        timer.SetTimeoutCallBack(()=> {
            timer = null;
            m_CanOutDestroy = true;
        });

        RegistTimer(CAN_OUT_DESTROY_TIMER_KEY, timer);
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

        if (StageManager.Instance.IsOutOfField(attackBullet.transform))
        {
            return;
        }

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

        DestroyAllTimer();
        EnemyCharaManager.Instance.DestroyEnemy(this);

        ItemManager.Instance.CreateItem(transform.localPosition, m_DropItemParam);


    }

    private void OperateEventVariable()
    {
        if (m_DefeatOperateVariableParams == null)
        {
            return;
        }

        var eventContent = new EventContent();
        eventContent.ExecuteTiming = EventContent.E_EXECUTE_TIMING.IMMEDIATE;
        eventContent.EventType = EventContent.E_EVENT_TYPE.OPERATE_VARIABLE;
        eventContent.OperateVariableParams = m_DefeatOperateVariableParams;
        EventManager.Instance.ExecuteEvent(eventContent);
    }
}
