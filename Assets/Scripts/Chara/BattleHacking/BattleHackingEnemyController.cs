using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHackingEnemyController : CommandCharaController
{
    [SerializeField, Tooltip("被弾直後の無敵時間")]
    private float m_OnHitInvincibleDuration;

    [SerializeField, Tooltip("撃破時の獲得スコア")]
    private int m_Score;

    #region Field

    private string m_LookId;

    private BattleRealEnemyGenerateParamSet m_GenerateParamSet;
    protected BattleRealEnemyGenerateParamSet GenerateParamSet => m_GenerateParamSet;

    private BattleRealEnemyBehaviorParamSet m_BehaviorParamSet;
    protected BattleRealEnemyBehaviorParamSet BehaviorParamSet => m_BehaviorParamSet;

    /// <summary>
    /// 敵キャラのサイクル。
    /// </summary>
    private E_POOLED_OBJECT_CYCLE m_Cycle;

    protected Vector2 PrePosition { get; private set; }

    protected Vector2 MoveDir { get; private set; }

    /// <summary>
    /// 移動方向を常に正面とするかどうか
    /// </summary>
    protected bool m_IsLookMoveDir;

    /// <summary>
    /// 出現して以降、画面に映ったかどうか
    /// </summary>
    protected bool IsShowFirst { get; private set; }

    public bool IsOutOfEnemyField { get; private set; }

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

        //Troop = E_CHARA_TROOP.ENEMY;
        //IsShowFirst = false;
        //m_IsLookMoveDir = true;

        //if (m_GenerateParamSet != null)
        //{
        //    InitHp(m_GenerateParamSet.Hp);
        //    SetScore(m_GenerateParamSet.Score);
        //}
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

    /// <summary>
    /// 撃破時スコアをセットする
    /// </summary>
    public void SetScore(int score)
    {
        m_Score = score;
    }

    public void SetParamSet(BattleRealEnemyGenerateParamSet generateParamSet, BattleRealEnemyBehaviorParamSet behaviorParamSet)
    {
        //m_GenerateParamSet = generateParamSet;
        //m_BehaviorParamSet = behaviorParamSet;

        //SetBulletSetParam(m_BehaviorParamSet.BulletSetParam);

        OnSetParamSet();
    }

    protected virtual void OnSetParamSet()
    {

    }

    public override void Dead()
    {
        base.Dead();

        BattleRealPlayerManager.Instance.AddScore(m_Score);

        //if (m_GenerateParamSet != null)
        //{
        //    BattleRealItemManager.Instance.CreateItem(transform.position, m_GenerateParamSet.ItemCreateParam);

        //    var events = m_GenerateParamSet.DefeatEvents;
        //    if (events != null)
        //    {
        //        for (int i = 0; i < events.Length; i++)
        //        {
        //            BattleRealPlayerManager.Instance.AddScore(m_Score);
        //            BattleRealEventManager.Instance.ExecuteEvent(events[i]);
        //        }
        //    }
        //}

        Destroy();
    }

    public void Destroy()
    {
        BattleHackingEnemyManager.Instance.DestroyEnemy(this);
    }
}
