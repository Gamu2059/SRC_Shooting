#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスのチャージゲージを制御する。
/// </summary>
public class BossChargeController : ControllableMonoBehavior
{
    [SerializeField]
    private Renderer m_RemainDamageGageRenderer;

    [SerializeField]
    private Renderer m_RemainTimeGageRenderer;

    private MaterialPropertyBlock m_RemainDamageGageProp;
    private MaterialPropertyBlock m_RemainTimeGageProp;
    private bool m_IsPlaying;
    private BattleRealBossController m_TargetBoss;
    private int m_ThreshPropID;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_RemainDamageGageProp = new MaterialPropertyBlock();
        m_RemainTimeGageProp = new MaterialPropertyBlock();
        m_RemainDamageGageRenderer.GetPropertyBlock(m_RemainDamageGageProp);
        m_RemainTimeGageRenderer.GetPropertyBlock(m_RemainTimeGageProp);
        m_ThreshPropID = Shader.PropertyToID("_Thresh");
        Stop();
    }

    public override void OnFinalize()
    {
        m_RemainTimeGageProp.Clear();
        m_RemainTimeGageProp = null;
        m_RemainDamageGageProp.Clear();
        m_RemainDamageGageProp = null;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!m_IsPlaying)
        {
            return;
        }

        if (m_TargetBoss == null || !m_TargetBoss.IsCharging)
        {
            Stop();
        }

        var remainTime = m_TargetBoss.ChargeRemainTime;
        var duration = m_TargetBoss.ChargeDuration;
        var currentDamage = m_TargetBoss.DamageFromChargeStart;
        var maxDamage = m_TargetBoss.DamageUntilChargeFailure;

        var timeRate = duration > 0 ? remainTime / duration : 0f;
        var damageRate = maxDamage > 0 ? (maxDamage - currentDamage) / maxDamage : 0f;

        m_RemainTimeGageProp.SetFloat(m_ThreshPropID, timeRate);
        m_RemainDamageGageProp.SetFloat(m_ThreshPropID, damageRate);
        m_RemainTimeGageRenderer.SetPropertyBlock(m_RemainTimeGageProp);
        m_RemainDamageGageRenderer.SetPropertyBlock(m_RemainDamageGageProp);
    }

    #endregion

    public void Play(BattleRealBossController boss)
    {
        if (boss == null)
        {
            return;
        }

        m_IsPlaying = true;
        m_TargetBoss = boss;
        m_RemainDamageGageRenderer.gameObject.SetActive(true);
        m_RemainTimeGageRenderer.gameObject.SetActive(true);
    }

    public void Stop()
    {
        m_RemainDamageGageRenderer.gameObject.SetActive(false);
        m_RemainTimeGageRenderer.gameObject.SetActive(false);
        m_IsPlaying = false;
    }
}
