#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// エフェクトの基礎クラス。
/// </summary>
public class BattleCommonEffectController : ControllableMonoBehavior
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
    public Transform Owner { get; private set; }

    [SerializeField]
    private E_POOLED_OBJECT_CYCLE m_Cycle;
    public E_POOLED_OBJECT_CYCLE Cycle {
        get => m_Cycle;
        set => m_Cycle = value;
    }

    private bool m_IsAllowOwner;
    public bool IsAllowOwner { get; private set; }

    private Vector3 m_RelatedAllowPos;

    private bool m_IsAutoDestroyDuration;

    private float m_Duration;
    public float Duration { get; private set; }

    private float m_NowLifeTime;
    public float NowLifeTime { get; private set; }

    #endregion

    #region Game Cycle

    public void OnCreateEffect(EffectParamSet paramSet, Transform owner)
    {
        if (paramSet == null)
        {
            Debug.LogWarning("EffectParamSetがありません。");
            return;
        }

        Owner = owner;

        var position = paramSet.GetFirePosition();
        var rotation = paramSet.GetFireRotation();
        var scale = paramSet.GetFireScale();

        if (Owner == null || paramSet.FirePositionRelative == E_RELATIVE.ABSOLUTE)
        {
            transform.position = position;
            m_RelatedAllowPos = Vector3.zero;
        }
        else
        {
            transform.position = position + Owner.position;
            m_RelatedAllowPos = position;
        }

        if (Owner == null || paramSet.FireRotationRelative == E_RELATIVE.ABSOLUTE)
        {
            transform.eulerAngles = rotation;
        }
        else
        {
            transform.eulerAngles = rotation + Owner.eulerAngles;
        }

        if (Owner == null || paramSet.FireScaleRelative == E_RELATIVE.ABSOLUTE)
        {
            transform.localScale = scale;
        }
        else
        {
            scale.x *= Owner.localScale.x;
            scale.y *= Owner.localScale.y;
            scale.z *= Owner.localScale.z;
            transform.localScale = scale;
        }

        m_IsAllowOwner = paramSet.IsAllowOwnerPosition && Owner != null;
        m_IsAutoDestroyDuration = paramSet.IsAutoDestroyDuration;
        m_Duration = paramSet.Duration;
    }

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

        if (m_IsAllowOwner && Owner != null)
        {
            var ownerPos = Owner.transform.position;
            transform.position = ownerPos + m_RelatedAllowPos;
        }

        if (m_Animators != null)
        {
            foreach (var a in m_Animators)
            {
                a.Update(Time.deltaTime);
            }
        }

        m_NowLifeTime += Time.deltaTime;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (m_IsAutoDestroyDuration && m_Duration >= 0 && m_NowLifeTime >= m_Duration)
        {
            DestroyEffect(true);
        }
    }

    #endregion

    public virtual void DestroyEffect(bool isImmediateStop)
    {
        if (m_Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            Stop(isImmediateStop);
            // マネージャが自動的に回収することを期待してプールスタンバイにする
            m_Cycle = E_POOLED_OBJECT_CYCLE.STANDBY_CHECK_POOL;
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
