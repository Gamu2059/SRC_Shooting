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

    [HideInInspector]
    public E_POOLED_OBJECT_CYCLE Cycle;

    /// <summary>
    /// エフェクトの所有者との位置関係を維持するかどうか。
    /// </summary>
    [HideInInspector]
    public bool IsAllowOwner;

    /// <summary>
    /// エフェクトの所有者との相対位置。
    /// </summary>
    [HideInInspector]
    public Vector3 RelatedAllowPos;

    /// <summary>
    /// エフェクトの所有者との角度関係を維持するかどうか。<br/>
    /// IsAllowOwner == true && IsAllowOwnerAngle == true とすると所有者との相対位置に回転も考慮されるようになる。
    /// </summary>
    [HideInInspector]
    public bool IsAllowOwnerAngle;

    private bool m_IsAutoDestroyDuration;

    private float m_Duration;
    public float Duration { get; private set; }

    private float m_NowLifeTime;
    public float NowLifeTime { get; private set; }

    private PlaySoundParam[] m_PlaySoundParams;

    /// <summary>
    /// 自動消去の時に呼び出されるアクション。
    /// </summary>
    public Action OnCompleteEffect;

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
            RelatedAllowPos = Vector3.zero;
        }
        else
        {
            transform.position = position + Owner.position;
            RelatedAllowPos = position;
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

        IsAllowOwner = paramSet.IsAllowOwnerPosition && Owner != null;
        IsAllowOwnerAngle = paramSet.IsAllowOwnerRotation && Owner != null;
        m_IsAutoDestroyDuration = paramSet.IsAutoDestroyDuration;
        m_Duration = paramSet.Duration;

        m_PlaySoundParams = paramSet.PlaySoundParams;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_NowLifeTime = 0;
    }

    public override void OnFinalize()
    {
        OnCompleteEffect = null;
        Owner = null;
        base.OnFinalize();
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

        if (m_PlaySoundParams != null)
        {
            var auidioManager = AudioManager.Instance;
            foreach (var p in m_PlaySoundParams)
            {
                auidioManager.Play(p);
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Owner != null)
        {
            if (IsAllowOwner && IsAllowOwnerAngle)
            {
                var ownerPos = Owner.transform.position;
                transform.position = ownerPos + Owner.rotation * RelatedAllowPos;
            }
            else if (IsAllowOwner)
            {
                var ownerPos = Owner.transform.position;
                transform.position = ownerPos + RelatedAllowPos;
            }
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
            OnCompleteEffect?.Invoke();
            OnCompleteEffect = null;
            DestroyEffect(true);
        }
    }

    #endregion

    public virtual void DestroyEffect(bool isImmediateStop)
    {
        if (Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            Stop(isImmediateStop);
            // マネージャが自動的に回収することを期待してプールスタンバイにする
            Cycle = E_POOLED_OBJECT_CYCLE.STANDBY_CHECK_POOL;
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
