#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealPlayerLaser : BulletController
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private ParticleSystem m_Particle;

    private Vector3 m_OwnerPosition;
    private Vector3 m_OwnerRotation;

    private Vector3 m_InitDeltaPosition;
    private Vector3 m_InitDeltaRotation;

    private float m_AnimationNormalizedTime;
    private float m_ParticleTime;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_IsLookMoveDir = false;

        BattleRealBulletManager.Instance.ToHackingAction += OnPause;
        BattleRealBulletManager.Instance.FromHackingAction += OnResume;

        m_Animator.updateMode = AnimatorUpdateMode.Normal;
        m_Animator.enabled = false;
        m_Animator.speed = 1;

        var ownerTransform = GetBulletOwner().transform;
        m_OwnerPosition = ownerTransform.localPosition;
        m_OwnerRotation = ownerTransform.localEulerAngles;

        m_InitDeltaPosition = transform.localPosition - m_OwnerPosition;
        m_InitDeltaRotation = transform.localEulerAngles - m_OwnerRotation;

        m_AnimationNormalizedTime = 0;
        m_ParticleTime = 0;
        m_Animator.Play("player_laser_lv1");
    }

    public override void OnFinalize()
    {
        BattleRealBulletManager.Instance.FromHackingAction -= OnResume;
        BattleRealBulletManager.Instance.ToHackingAction -= OnPause;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ownerPosition = GetBulletOwner().transform.position + m_InitDeltaPosition;
        ownerPosition.y = 10;
        SetPosition(ownerPosition);

        m_Animator.Update(Time.deltaTime);
        m_ParticleTime += Time.deltaTime;
    }

    private void OnPause()
    {
        SetParticleSpeed(0f);
        m_AnimationNormalizedTime = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    private void OnResume()
    {
        SetParticleSpeed(1f);
        m_Animator.Play("player_laser_lv1", 0, m_AnimationNormalizedTime);
    }

    private void SetParticleSpeed(float value)
    {
        var pMain = m_Particle.main;
        pMain.simulationSpeed = value;
    }
}
