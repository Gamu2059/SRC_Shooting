using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEnemyController : CommandCharaController
{
    [SerializeField, Tooltip("死亡時エフェクト")]
    private GameObject m_DeadEffect;

    [SerializeField, Tooltip("被弾直後の無敵時間")]
    private float m_OnHitInvincibleDuration;

    [SerializeField, Tooltip("撃破時の獲得スコア")]
    private int m_Score;

    /// <summary>
    /// マスターデータから取得するパラメータセット
    /// </summary>
    protected ArgumentParamSet m_ParamSet;

    /// <summary>
    /// 敵キャラのサイクル。
    /// </summary>
    private E_OBJECT_CYCLE m_Cycle;

    /// <summary>
    /// 表示されてから非表示になるかどうか。
    /// </summary>
    private bool m_CanOutDestroy;

    /// <summary>
    /// ボスかどうか。
    /// </summary>
    [SerializeField]
    private bool m_IsBoss;



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



    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CanOutDestroy = false;
    }

    public virtual void SetStringParam(string param)
    {
        m_ParamSet = ArgumentParamSetTranslator.TranslateFromString(param);
    }



    protected virtual void OnBecameVisible()
    {
        //RegistTimer("CanOutDestroy", Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, BattleRealEnemyManager.Instance.GetCanOutTime(), () =>
        //{
        //    m_CanOutDestroy = true;
        //}));
    }

    protected virtual void OnBecameInvisible()
    {
        if (m_CanOutDestroy)
        {
            BattleHackingEnemyManager.Instance.DestroyEnemy(this);
        }
    }

    public override void Dead()
    {
        base.Dead();

        DestroyAllTimer();
        BattleHackingEnemyManager.Instance.DestroyEnemy(this);

        if (m_IsBoss)
        {
            //BattleManager.Instance.TransitionBattleMain();
        }

        BattleRealPlayerManager.Instance.AddScore(m_Score);
    }
}
