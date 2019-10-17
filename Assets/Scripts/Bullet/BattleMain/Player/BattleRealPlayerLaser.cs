using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealPlayerLaser : BulletController
{
    [SerializeField]
    private Animator m_Animator;

    private Vector3 m_OwnerPosition;
    private Vector3 m_OwnerRotation;

    private Vector3 m_InitDeltaPosition;
    private Vector3 m_InitDeltaRotation;

    public override void OnInitialize()
    {
        base.OnInitialize();

        var ownerTransform = GetBulletOwner().transform;
        m_OwnerPosition = ownerTransform.localPosition;
        m_OwnerRotation = ownerTransform.localEulerAngles;

        m_InitDeltaPosition = transform.localPosition - m_OwnerPosition;
        m_InitDeltaRotation = transform.localEulerAngles - m_OwnerRotation;

        m_Animator.SetInteger("ChargeLevel", 1);
    }

    public override void OnFinalize()
    {
        m_Animator.SetInteger("ChargeLevel", 0);
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ownerPosition = GetBulletOwner().transform.position + m_InitDeltaPosition;
        ownerPosition.y = 10;
        SetPosition(ownerPosition);
    }
}
