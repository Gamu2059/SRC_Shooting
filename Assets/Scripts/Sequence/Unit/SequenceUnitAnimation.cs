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
    #region Define

    [Serializable]
    private struct InitData
    {
        public Vector3 Data;
        public bool IsPassX;
        public bool IsPassY;
        public bool IsPassZ;
    }

    #endregion

    [Header("Animation Parameter")]

    [SerializeField, Tooltip("WorldPosition とは書いてありますが、SpaceTypeによって座標系は変わります")]
    private InitData m_InitializeWorldPosition;

    [SerializeField, Tooltip("EulerAngles とは書いてありますが、SpaceTypeによって座標系は変わります")]
    private InitData m_InitializeEulerAngles;

    [SerializeField]
    private float m_Duration;

    [SerializeField]
    private BattleAnimationParam m_AnimationParam;

    private Vector3 m_CalcedInitPosition;
    private Vector3 m_CalcedInitRotation;

    protected override void OnStart()
    {
        base.OnStart();

        var pos = m_SpaceType == Space.World ? Target.position : Target.localPosition;
        var rot = m_SpaceType == Space.World ? Target.eulerAngles : Target.localEulerAngles;
        m_CalcedInitPosition = GetInitData(pos, m_InitializeWorldPosition);
        m_CalcedInitRotation = GetInitData(rot, m_InitializeEulerAngles);

        if (m_SpaceType == Space.World)
        {
            Target.SetPositionAndRotation(m_CalcedInitPosition, Quaternion.Euler(m_CalcedInitRotation));
        }
        else
        {
            Target.localPosition = m_CalcedInitPosition;
            Target.localEulerAngles = m_CalcedInitRotation;
        }
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

    public override void GetStartTransform(Transform target, out Space spaceType, out Vector3 position, out Vector3 rotate)
    {
        // スペースタイプによって解釈が変わるため難しい
        var pos = m_SpaceType == Space.World ? Target.position : Target.localPosition;
        var rot = m_SpaceType == Space.World ? Target.eulerAngles : Target.localEulerAngles;
        m_CalcedInitPosition = GetInitData(pos, m_InitializeWorldPosition);
        m_CalcedInitRotation = GetInitData(rot, m_InitializeEulerAngles);
        spaceType = m_SpaceType;
        position = GetInitData(target.position, m_InitializeWorldPosition);
        rotate = GetInitData(target.eulerAngles, m_InitializeEulerAngles);
    }

    private Vector3 GetInitData(Vector3 origin, InitData initData)
    {
        if (!initData.IsPassX)
        {
            origin.x = initData.Data.x;
        }

        if (!initData.IsPassY)
        {
            origin.y = initData.Data.y;
        }

        if (!initData.IsPassZ)
        {
            origin.z = initData.Data.z;
        }

        return origin;
    }

    private void ApplyAnimation(Transform target)
    {
        if (m_AnimationParam.UsePosition)
        {
            target.position = GetAnimVector(ref m_AnimationParam.Position, CurrentTime, ref m_CalcedInitPosition);
        }

        if (m_AnimationParam.UseRotation)
        {
            target.eulerAngles = GetAnimVector(ref m_AnimationParam.Rotation, CurrentTime, ref m_CalcedInitRotation);
        }
    }

    private void ApplyEndValue(Transform target)
    {
        if (m_AnimationParam.UsePosition)
        {
            target.position = GetEndVector(ref m_AnimationParam.Position, ref m_CalcedInitPosition);
        }

        if (m_AnimationParam.UseRotation)
        {
            target.eulerAngles = GetEndVector(ref m_AnimationParam.Rotation, ref m_CalcedInitRotation);
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
