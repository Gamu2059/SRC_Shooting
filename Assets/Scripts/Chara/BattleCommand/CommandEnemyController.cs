using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEnemyController : CommandCharaController
{
    [Space()]
    [Header("敵専用 パラメータ")]

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

    private bool m_CanOutDestroy;



    #region Getter & Setter

    public StringParamSet GetParamSet()
    {
        return m_ParamSet;
    }

    #endregion



    private void Start()
    {
        // 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
        CommandEnemyCharaManager.Instance.RegistEnemy(this);
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
        m_CanOutDestroy = true;
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
