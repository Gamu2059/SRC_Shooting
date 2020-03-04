#pragma warning disable 0649

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
    [Header("Animation Parameter")]

    [SerializeField]
    private Vector3 m_InitWorldPosition;

    [SerializeField]
    private Vector3 m_InitEulerAngles;

    [SerializeField]
    private float m_Duration;

    [SerializeField]
    private BattleAnimationParam m_AnimationParam;

    protected override void OnStart()
    {
        base.OnStart();

        Target.position = m_InitWorldPosition;
        Target.eulerAngles = m_InitEulerAngles;
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        ApplyAnimation(Target);
    }

    protected override void OnEnd()
    {
        ApplyEndValue(Target);
        base.OnEnd();
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= m_Duration;
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
            target.position = GetAnimVector(ref m_AnimationParam.Position, CurrentTime, ref m_InitWorldPosition);
        }

        if (m_AnimationParam.UseRotation)
        {
            target.eulerAngles = GetAnimVector(ref m_AnimationParam.Rotation, CurrentTime, ref m_InitEulerAngles);
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
