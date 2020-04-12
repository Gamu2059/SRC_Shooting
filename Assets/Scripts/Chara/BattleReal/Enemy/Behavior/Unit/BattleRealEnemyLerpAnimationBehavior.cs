#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 座標と座標を結んで動く敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Unit/LerpAnimation", fileName = "param.behavior_unit.asset", order = 30)]
public class BattleRealEnemyLerpAnimationBehavior : BattleRealEnemyBehaviorUnitBase
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
    private AnimationCurve m_PositionLerp;
    protected AnimationCurve PositionLerp => m_PositionLerp;

    [Space()]

    [SerializeField]
    private bool m_UseRotation;
    protected bool UseRotation => m_UseRotation;

    [SerializeField]
    private Vector3 m_TargetEulerAngles;
    protected Vector3 TargetEulerAngles => m_TargetEulerAngles;

    [SerializeField]
    private AnimationCurve m_RotationLerp;
    protected AnimationCurve RotationLerp => m_RotationLerp;

    [SerializeField, Tooltip("回転において、ほぼ1周するような時に、近い方向を使って補間していくかどうか")]
    private bool m_CalcNearRotation;
    protected bool CalcNearRotation => m_CalcNearRotation;

    #endregion

    #region Field

    private Vector3 m_CurrentPosition;
    private Vector3 m_CurrentEulerAngles;
    private Vector3 m_NextPosition;
    private Vector3 m_NextEulerAngles;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        var t = Enemy.transform;
        m_CurrentPosition = SpaceType == Space.World ? t.position : t.localPosition;
        m_CurrentEulerAngles = SpaceType == Space.World ? t.eulerAngles : t.localEulerAngles;

        m_NextPosition = TargetPosition;
        m_NextEulerAngles = TargetEulerAngles;

        if (CalcNearRotation)
        {
            var x = CalcNearComb(m_CurrentEulerAngles.x, m_NextEulerAngles.x);
            var y = CalcNearComb(m_CurrentEulerAngles.y, m_NextEulerAngles.y);
            var z = CalcNearComb(m_CurrentEulerAngles.z, m_NextEulerAngles.z);

            m_CurrentEulerAngles = new Vector3(x.Item1, y.Item1, z.Item1);
            m_NextEulerAngles = new Vector3(x.Item2, y.Item2, z.Item2);
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        Move();
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= Duration;
    }

    #endregion

    private void Move()
    {
        Vector3 pos = Vector3.zero, rot = Vector3.zero;

        if (UsePosition)
        {
            var posLerp = m_PositionLerp.Evaluate(CurrentTime);
            pos = Vector3.Lerp(m_CurrentPosition, m_NextPosition, posLerp);
        }

        if (UseRotation)
        {
            var rotLerp = m_RotationLerp.Evaluate(CurrentTime);
            rot = Vector3.Lerp(m_CurrentEulerAngles, m_NextEulerAngles, rotLerp);
        }

        var t = Enemy.transform;
        if (m_SpaceType == Space.World)
        {
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
                t.localEulerAngles = rot;
            }
        }
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
