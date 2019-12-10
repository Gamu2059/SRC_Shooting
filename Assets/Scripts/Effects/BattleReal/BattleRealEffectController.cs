#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのエフェクトの基礎クラス。
/// </summary>
public class BattleRealEffectController : ControllableMonoBehavior
{
    #region Field Inspector

    /// <summary>
    /// このエフェクトのグループ名。
    /// 同じ名前同士のエフェクトは、違うものから生成されたものであっても、プールから再利用される。
    /// </summary>
    [SerializeField]
    private string m_EffectGroupId = "Default Effect";
    public string EffectGroupId => m_EffectGroupId;

    [SerializeField]
    private ParticleSystem[] m_ParticleSystems;

    [SerializeField]
    private Animator[] m_Animators;

    #endregion

    #region Field

    /// <summary>
    /// このエフェクトを発生させたもの。
    /// </summary>
    private Transform m_Owner;
    public Transform Owner {
        get => m_Owner;
        set => m_Owner = value;
    }

    /// <summary>
    /// このエフェクトの状態
    /// </summary>
    [SerializeField]
    private E_POOLED_OBJECT_CYCLE m_Cycle;
    public E_POOLED_OBJECT_CYCLE Cycle {
        get => m_Cycle;
        set => m_Cycle = value;
    }

    /// <summary>
    /// 発生元の位置を追従するかどうか
    /// </summary>
    private bool m_IsAllowOwner;
    public bool IsAllowOwner {
        get => m_IsAllowOwner;
        set => m_IsAllowOwner = value;
    }

    /// <summary>
    /// 相対追従座標
    /// </summary>
    private Vector3 m_RelatedAllowPos;
    public Vector3 RelatedAllowPos {
        get => m_RelatedAllowPos;
        set => m_RelatedAllowPos = value;
    }

    /// <summary>
    /// 継続時間
    /// </summary>
    private float m_Duration;
    public float Duration {
        get => m_Duration;
        set => m_Duration = value;
    }

    private float m_NowLifeTime;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_Duration = -1;
        m_NowLifeTime = 0;
    }

    public override void OnStart()
    {
        base.OnStart();

        if (m_ParticleSystems != null)
        {
            foreach (var p in m_ParticleSystems)
            {
                p.Play(true);
            }
        }

        if (m_Animators != null)
        {
            foreach (var a in m_Animators)
            {
                a.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsAllowOwner && m_Owner != null)
        {
            var ownerPos = m_Owner.transform.position;
            transform.position = ownerPos + m_RelatedAllowPos;
        }

        if (m_Animators != null)
        {
            foreach (var a in m_Animators)
            {
                a.Update(Time.deltaTime);
            }
        }

        if (m_Duration >= 0)
        {
            if (m_NowLifeTime >= m_Duration)
            {
                DestoryEffect(true);
            }
        }

        m_NowLifeTime += Time.deltaTime;
    }

    #endregion

    public virtual void DestoryEffect(bool isImmediateStop)
    {
        if (m_Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            Stop(isImmediateStop);
            BattleRealEffectManager.Instance.CheckPoolEffect(this);
        }
    }

    public virtual void Pause()
    {
        if (m_ParticleSystems != null)
        {
            foreach (var p in m_ParticleSystems)
            {
                p.Pause(true);
            }
        }
    }

    public virtual void Resume()
    {
        if (m_ParticleSystems != null)
        {
            foreach (var p in m_ParticleSystems)
            {
                p.Play();
            }
        }
    }

    public virtual void Stop(bool isImmediate)
    {
        if (m_ParticleSystems != null)
        {
            ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting;
            if (isImmediate)
            {
                stopBehavior = ParticleSystemStopBehavior.StopEmittingAndClear;
            }

            foreach (var p in m_ParticleSystems)
            {
                p.Stop(true, stopBehavior);
            }
        }
    }
}
