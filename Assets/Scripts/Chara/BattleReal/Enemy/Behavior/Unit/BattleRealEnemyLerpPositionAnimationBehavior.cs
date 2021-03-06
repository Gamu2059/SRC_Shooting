﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 座標と座標を結んで動く敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Unit/LerpPositionAnimation", fileName = "param.behavior_unit.asset")]
public class BattleRealEnemyLerpPositionAnimationBehavior : BattleRealEnemyBehaviorUnitBase
{
    #region Field Inspector

    [Header("Lerp Animation Parameter")]

    [SerializeField]
    private Space m_SpaceType;
    protected Space SpaceType => m_SpaceType;

    [SerializeField]
    private float m_Duration;
    protected float Duration => m_Duration;

    [Space()]

    [SerializeField]
    private bool m_UsePosition;
    protected bool UsePosition => m_UsePosition;

    [SerializeField]
    private Vector3 m_TargetPosition;
    protected Vector3 TargetPosition => m_TargetPosition;

    [SerializeField]
    private bool m_IsPassX;
    protected bool IsPassX => m_IsPassX;

    [SerializeField]
    private bool m_IsPassY;
    protected bool IsPassY => m_IsPassY;

    [SerializeField]
    private bool m_IsPassZ;
    protected bool IsPassZ => m_IsPassZ;

    [SerializeField]
    private AnimationCurve m_PositionLerp;
    protected AnimationCurve PositionLerp => m_PositionLerp;

    [Space()]

    [SerializeField]
    private bool m_UseRotation;
    protected bool UseRotation => m_UseRotation;

    [SerializeField, Tooltip("Y軸での回転速度")]
    private float m_RotateAngleSpeed;
    protected float RotateAngleSpeed => m_RotateAngleSpeed;

    #endregion

    #region Field

    private Vector3 m_CurrentPosition;
    private Vector3 m_NextPosition;
    private float m_CurrentEulerAngle;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        var t = Enemy.transform;
        m_CurrentPosition = SpaceType == Space.World ? t.position : t.localPosition;
        m_CurrentEulerAngle = SpaceType == Space.World ? t.eulerAngles.y : t.localEulerAngles.y;
        m_NextPosition = TargetPosition;
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Move();
        m_CurrentEulerAngle += RotateAngleSpeed * deltaTime;
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= Duration;
    }

    #endregion

    private void Move()
    {
        Vector3 pos = m_CurrentPosition;

        if (UsePosition)
        {
            var posLerp = m_PositionLerp.Evaluate(CurrentTime);
            pos = Vector3.Lerp(m_CurrentPosition, m_NextPosition, posLerp);
            if (IsPassX)
            {
                pos.x = m_CurrentPosition.x;
            }
            if (IsPassY)
            {
                pos.y = m_CurrentPosition.y;
            }
            if (IsPassZ)
            {
                pos.z = m_CurrentPosition.z;
            }
        }

        var t = Enemy.transform;
        if (m_SpaceType == Space.World)
        {
            var rot = t.eulerAngles;
            rot.y = m_CurrentEulerAngle;

            if (UsePosition && UseRotation)
            {
                t.SetPositionAndRotation(pos, Quaternion.Euler(rot));
            }
            else if (UsePosition)
            {
                t.position = pos;
            }
            else if (UseRotation)
            {
                t.eulerAngles = rot;
            }
        }
        else
        {
            if (UsePosition)
            {
                t.localPosition = pos;
            }
            if (UseRotation)
            {
                var rot = t.localEulerAngles;
                rot.y = m_CurrentEulerAngle;
                t.localEulerAngles = rot;
            }
        }
    }
}
