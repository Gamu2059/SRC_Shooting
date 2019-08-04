using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharaController
{
    public const string HIT_INVINCIBLE_TIMER_KEY = "HitInvincibleTimer";

    [Space()]
    [Header("敵専用 パラメータ")]

    [SerializeField, Tooltip("アイテムの生成情報")]
    private ItemCreateParam m_ItemCreateParam;

    [SerializeField, Tooltip("ボスかどうか")]
    private bool m_IsBoss;

    [SerializeField, Tooltip("死亡時エフェクト")]
    private GameObject m_DeadEffect;

    [SerializeField, Tooltip("被弾直後の無敵時間")]
    private float m_OnHitInvincibleDuration;

    /// <summary>
    /// マスターデータから取得するパラメータセット
    /// </summary>
    protected StringParamSet m_ParamSet;

    /// <summary>
    /// 敵キャラのサイクル。
    /// </summary>
    private E_OBJECT_CYCLE m_Cycle;

    private bool m_CanOutDestroy;

    private bool m_OnBecameBeforeInitialize;



    #region Getter & Setter

    public StringParamSet GetParamSet()
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

        if (m_OnBecameBeforeInitialize)
        {
            RegistTimer("CanOutDestroy", Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, EnemyCharaManager.Instance.GetCanOutTime(), () =>
            {
                m_CanOutDestroy = true;
            }));
        }
        m_OnBecameBeforeInitialize = false;
    }

    public virtual void SetStringParam(string param)
    {
        m_ParamSet = StringParamTranslator.TranslateString(param);
    }



    protected virtual void OnBecameVisible()
    {
        if (BattleMainTimerManager.Instance != null)
        {
            //RegistTimer("CanOutDestroy", Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, EnemyCharaManager.Instance.GetCanOutTime(), () =>
            //{
            //    m_CanOutDestroy = true;
            //}));
        }
        else
        {
            m_OnBecameBeforeInitialize = true;
        }
    }

    protected virtual void OnBecameInvisible()
    {
        if (m_CanOutDestroy)
        {
            EnemyCharaManager.Instance.DestroyEnemy(this);
        }
    }



    public override void SufferBullet(BulletController attackBullet, ColliderData attackData, ColliderData targetData)
    {
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
        ItemManager.Instance.CreateItem(transform.localPosition, m_ItemCreateParam);
    }
}
