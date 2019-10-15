using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleRealEnemyController : CharaController
{
    public const string HIT_INVINCIBLE_TIMER_KEY = "HitInvincibleTimer";

    [Space()]
    [Header("敵専用 パラメータ")]
    [SerializeField]
    protected int m_Score;

    [SerializeField, Tooltip("被弾直後の無敵時間")]
    private float m_OnHitInvincibleDuration;

    #region Field

    private string m_LookId;

    private BattleRealEnemyGenerateParamSet m_GenerateParamSet;
    protected BattleRealEnemyGenerateParamSet GenerateParamSet => m_GenerateParamSet;

    private BattleRealEnemyBehaviorParamSet m_BehaviorParamSet;
    protected BattleRealEnemyBehaviorParamSet BehaviorParamSet => m_BehaviorParamSet;

    private E_POOLED_OBJECT_CYCLE m_Cycle;

    /// <summary>
    /// 出現して以降、画面に映ったかどうか
    /// </summary>
    private bool m_IsShowFirst;

    public bool IsOutOfEnemyField { get; private set; }

    protected ArgumentParamSet m_ParamSet;

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

        if (m_GenerateParamSet != null)
        {
            InitHp(m_GenerateParamSet.Hp);
            SetScore(m_GenerateParamSet.Score);
        }

        m_IsShowFirst = false;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        IsOutOfEnemyField = BattleRealEnemyManager.Instance.IsOutOfField(this);
        if (IsOutOfEnemyField)
        {
            if (m_IsShowFirst)
            {
                Destroy();
            }
        }
        else
        {
            m_IsShowFirst = true;
        }
    }

    #endregion

    /// <summary>
    /// 引数をセットする
    /// </summary>
    public virtual void SetArguments(string param)
    {
        m_ParamSet = ArgumentParamSetTranslator.TranslateFromString(param);
    }

    /// <summary>
    /// 撃破時スコアをセットする
    /// </summary>
    public void SetScore(int score)
    {
        m_Score = score;
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

    protected override void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var bullet = sufferData.OpponentObject;
        if (IsSufferEchoBullet(bullet))
        {
            return;
        }

        if (BattleRealStageManager.Instance.IsOutOfField(bullet.transform))
        {
            return;
        }

        if (m_OnHitInvincibleDuration <= 0)
        {
            Damage(1);
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
            Damage(1);
        }
    }

    public override void Dead()
    {
        base.Dead();

        if (m_GenerateParamSet != null)
        {
            BattleRealItemManager.Instance.CreateItem(transform.position, m_GenerateParamSet.ItemCreateParam);

            var events = m_GenerateParamSet.DefeatEvents;
            if (events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    /* デバッグ用 */
                    Debug.Log(gameObject + ".m_Score = " + m_Score);

                    BattleRealPlayerManager.Instance.AddScore(m_Score);
                    BattleRealEventManager.Instance.ExecuteEvent(events[i]);
                }
            }
        }

        Destroy();
    }

    public void Destroy()
    {
        BattleRealEnemyManager.Instance.DestroyEnemy(this);
    }
}
