using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InfC761の死亡行動パターン。<br/>
/// InfC761のハッキングボス固有の処理のため、このクラスは削除しないでください。
/// </summary>
public class InfC761HackingDeadBehavior : BattleHackingBossBehavior
{
    private BattleHackingBoss m_Boss;
    private InfC761HackingGraphicsController m_GraphicsController;

    private float m_TimeCount;
    private bool m_IsCounting;

    public InfC761HackingDeadBehavior(BattleHackingEnemyController enemy, BattleHackingBossBehaviorUnitParamSet paramSet) : base(enemy, paramSet)
    {
        m_Boss = enemy as BattleHackingBoss;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_GraphicsController = Enemy.GetComponent<InfC761HackingGraphicsController>();
        m_GraphicsController?.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_GraphicsController?.OnFinalize();
        m_GraphicsController = null;
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        if (m_GraphicsController != null)
        {
            m_GraphicsController.OnStart();
            m_GraphicsController.PlayDestroyAnimation();
        }

        m_TimeCount = 0;
        m_IsCounting = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_GraphicsController?.OnUpdate();

        if (m_IsCounting && m_TimeCount >= 1)
        {
            if (m_Boss != null)
            {
                var deadParam = m_Boss.BossGenerateParamSet.DeadEffectParam;
                BattleHackingEffectManager.Instance.CreateEffect(deadParam, m_Boss.transform);
                m_GraphicsController.HideEye();
            }
            m_IsCounting = false;
        }
        m_TimeCount += Time.deltaTime;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_GraphicsController?.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        m_GraphicsController?.OnFixedUpdate();
    }
}
