using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 初期座標p、初期角度dから、AnimationCurveを用いて平行移動と回転を行う。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Unit/Animation", fileName = "animation.sequence_unit.asset", order = 0)]
public class SequenceUnitAnimation : SequenceUnit
{
    [SerializeField]
    private Vector3 m_InitWorldPosition;

    [SerializeField]
    private Vector3 m_InitEulerAngles;

    [SerializeField]
    private float m_Duration;

    [SerializeField]
    private BattleAnimationParam m_AnimationParam;

    private float m_CurrentTime;

    public override void OnStart(Transform target)
    {
        base.OnStart(target);
        m_CurrentTime = 0;

        target.position = m_InitWorldPosition;
        target.eulerAngles = m_InitEulerAngles;
    }

    public override void OnUpdate(Transform target, float deltaTime)
    {
        base.OnUpdate(target, deltaTime);
        m_CurrentTime += deltaTime;
        ApplyAnimation(target);
    }

    public override void OnEnd(Transform target)
    {
        ApplyEndValue(target);
        base.OnEnd(target);
    }

    public override bool IsEnd()
    {
        return m_CurrentTime >= m_Duration;
    }

    public override void GetStartTransform(Transform target, out Vector3 position, out Vector3 rotate)
    {
        position = m_InitWorldPosition;
        rotate = m_InitWorldPosition;
    }

    private void ApplyAnimation(Transform target)
    {
        if (m_AnimationParam.UsePosition)
        {
            target.position = GetAnimVector(ref m_AnimationParam.Position, m_CurrentTime, ref m_InitWorldPosition);
        }

        if (m_AnimationParam.UseRotation)
        {
            target.eulerAngles = GetAnimVector(ref m_AnimationParam.Rotation, m_CurrentTime, ref m_InitEulerAngles);
        }
    }

    private void ApplyEndValue(Transform target)
    {
        if (m_AnimationParam.UsePosition)
        {
            target.position = GetEndVector(ref m_AnimationParam.Position, ref m_InitWorldPosition);
        }

        if (m_AnimationParam.UseRotation)
        {
            target.eulerAngles = GetEndVector(ref m_AnimationParam.Rotation, ref m_InitEulerAngles);
        }
    }

    private Vector3 GetAnimVector(ref BattleAnimationParam.BattleAnimationVectorParam param, float currentTime, ref Vector3 initVector)
    {
        var vector = initVector;

        if (param.XParam.Use)
        {
            vector.x = param.XParam.AnimationValue.Evaluate(currentTime);
        }

        if (param.YParam.Use)
        {
            vector.y = param.YParam.AnimationValue.Evaluate(currentTime);
        }

        if (param.ZParam.Use)
        {
            vector.z = param.ZParam.AnimationValue.Evaluate(currentTime);
        }

        return vector;
    }

    private Vector3 GetEndVector(ref BattleAnimationParam.BattleAnimationVectorParam param, ref Vector3 initVector)
    {
        var vector = initVector;

        if (param.XParam.Use)
        {
            vector.x = param.XParam.EndValue;
        }

        if (param.YParam.Use)
        {
            vector.y = param.YParam.EndValue;
        }

        if (param.ZParam.Use)
        {
            vector.z = param.ZParam.EndValue;
        }

        return vector;
    }
}
