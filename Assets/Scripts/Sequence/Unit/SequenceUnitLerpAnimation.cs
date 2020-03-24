#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 現在の座標と角度から、AnimationCurveを用いて指定した位置と角度へと補間する。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Unit/LerpAnimation", fileName = "lerp_animation.sequence_unit.asset", order = 2)]
public class SequenceUnitLerpAnimation : SequenceUnit
{
    [Header("Lerp Animation Parameter")]

    [SerializeField]
    private Vector3 m_TargetPosition;

    [SerializeField]
    private Vector3 m_TargetEulerAngles;

    [SerializeField]
    private float m_Duration;

    [SerializeField]
    private AnimationCurve m_PositionLerp;

    [SerializeField]
    private AnimationCurve m_RotationLerp;

    [SerializeField, Tooltip("回転において、ほぼ1周するような時に、近い方向を使って補間していくかどうか")]
    private bool m_CalcNearRotation;

    private Vector3 m_OnStartPosition;
    private Vector3 m_OnStartEulerAngles;
    private Vector3 m_NextPosition;
    private Vector3 m_NextEulerAngles;

    protected override void OnStart()
    {
        base.OnStart();

        m_OnStartPosition = m_SpaceType == Space.World ? Target.position : Target.localPosition;
        m_OnStartEulerAngles = m_SpaceType == Space.World ? Target.eulerAngles : Target.localEulerAngles;

        m_NextPosition = m_TargetPosition;
        m_NextEulerAngles = m_TargetEulerAngles;

        if (m_CalcNearRotation)
        {
            var x = CalcNearComb(m_OnStartEulerAngles.x, m_NextEulerAngles.x);
            var y = CalcNearComb(m_OnStartEulerAngles.y, m_NextEulerAngles.y);
            var z = CalcNearComb(m_OnStartEulerAngles.z, m_NextEulerAngles.z);

            m_OnStartEulerAngles = new Vector3(x.Item1, y.Item1, z.Item1);
            m_NextEulerAngles = new Vector3(x.Item2, y.Item2, z.Item2);

        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        var posLerp = m_PositionLerp.Evaluate(CurrentTime);
        var rotLerp = m_RotationLerp.Evaluate(CurrentTime);

        var pos = Vector3.Lerp(m_OnStartPosition, m_NextPosition, posLerp);
        var rot = Vector3.Lerp(m_OnStartEulerAngles, m_NextEulerAngles, rotLerp);

        if (m_SpaceType == Space.World)
        {
            Target.SetPositionAndRotation(pos, Quaternion.Euler(rot));
        }
        else
        {
            Target.localPosition = pos;
            Target.localEulerAngles = rot;
        }
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= m_Duration;
    }

    private ValueTuple<float, float> CalcNearComb(float start, float next)
    {
        var s1 = start;
        var s2 = CalcNear180(start);
        var n1 = next;
        var n2 = CalcNear180(next);
        var list = new List<ValueTuple<float, ValueTuple<float, float>>>()
        {
            (Mathf.Abs(n1 - s1), (s1, n1)),
            (Mathf.Abs(n2 - s1), (s1, n2)),
            (Mathf.Abs(n1 - s2), (s2, n1)),
            (Mathf.Abs(n2 - s2), (s2, n2)),
        };

        var index = 0;
        var min = list[0].Item1;
        for (var i = 1; i < list.Count; i++)
        {
            if (list[i].Item1 < min)
            {
                index = i;
                min = list[i].Item1;
            }
        }

        return list[index].Item2;
    }

    private float CalcNear180(float value)
    {
        value = (value % 360 + 360) % 360;
        return value < 180 ? value : value - 360;
    }
}
