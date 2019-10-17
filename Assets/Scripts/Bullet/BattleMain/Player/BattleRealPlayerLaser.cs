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

    private float m_Time;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        var ownerTransform = GetBulletOwner().transform;
        m_OwnerPosition = ownerTransform.localPosition;
        m_OwnerRotation = ownerTransform.localEulerAngles;

        m_InitDeltaPosition = transform.localPosition - m_OwnerPosition;
        m_InitDeltaRotation = transform.localEulerAngles - m_OwnerRotation;

        m_Time = 0;

        m_Animator.Play("player_laser_lv1");
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ownerPosition = GetBulletOwner().transform.position + m_InitDeltaPosition;
        ownerPosition.y = 10;
        SetPosition(ownerPosition);

        //m_Animator.Update(Time.deltaTime);
        //m_Particle.Simulate(m_Time, true, true);
        m_Time += Time.deltaTime;
    }
}
