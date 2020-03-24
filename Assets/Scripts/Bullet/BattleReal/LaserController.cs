using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザーオブジェクトの基礎クラス。
/// </summary>
public class LaserController : BulletController
{
    private Vector3 m_OwnerLocalPosition;
    private Vector3 m_OwnerLocalRotation;

    private Vector3 m_InitDeltaPosition;
    private Vector3 m_InitDeltaRotation;

    public override void OnInitialize()
    {
        base.OnInitialize();

        var ownerTransform = GetBulletOwner().transform;
        m_OwnerLocalPosition = ownerTransform.localPosition;
        m_OwnerLocalRotation = ownerTransform.localEulerAngles;

        m_InitDeltaPosition = GetPosition() - m_OwnerLocalPosition;
        m_InitDeltaRotation = GetRotation() - m_OwnerLocalRotation;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var ownerTransform = GetBulletOwner().transform;
        SetPosition(ownerTransform.localPosition + m_InitDeltaPosition);
    }
}
