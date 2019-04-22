using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEnemyController : CommandCharaController
{
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



    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CanOutDestroy = false;
    }

    public virtual void SetStringParam(string param)
    {
        m_ParamSet = StringParamTranslator.TranslateString(param);
    }



    protected virtual void OnBecameVisible()
    {
        RegistTimer("CanOutDestroy", Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, EnemyCharaManager.Instance.GetCanOutTime(), () =>
        {
            m_CanOutDestroy = true;
        }));
    }

    protected virtual void OnBecameInvisible()
    {
        if (m_CanOutDestroy)
        {
            CommandEnemyCharaManager.Instance.DestroyEnemy(this);
        }
    }

    public override void Dead()
    {
        base.Dead();

        DestroyAllTimer();
        CommandEnemyCharaManager.Instance.DestroyEnemy(this);
    }
}
