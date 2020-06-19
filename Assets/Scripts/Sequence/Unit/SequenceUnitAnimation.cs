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
        var usePos = m_AnimationParam.UsePosition;
        var useRot = m_AnimationParam.UseRotation;

        if(!usePos && !useRot)
        {
            return;
        }

        var pos = m_SpaceType == Space.World ? target.position : target.localPosition;
        var rot = m_SpaceType == Space.World ? target.eulerAngles : target.localEulerAngles;

        if (usePos)
        {
            pos = GetAnimVector(ref m_AnimationParam.Position, CurrentTime, ref pos);
        }

        if (useRot)
        {
            rot = GetAnimVector(ref m_AnimationParam.Rotation, CurrentTime, ref rot);
        }

        if (m_SpaceType == Space.World)
        {
            target.SetPositionAndRotation(pos, Quaternion.Euler(rot));
        }
        else
        {
            target.localPosition = pos;
            target.localEulerAngles = rot;
        }
    }

    private void ApplyEndValue(Transform target)
    {
        var usePos = m_AnimationParam.UsePosition;
        var useRot = m_AnimationParam.UseRotation;

        if (!usePos && !useRot)
        {
            return;
        }

        var pos = m_SpaceType == Space.World ? target.position : target.localPosition;
        var rot = m_SpaceType == Space.World ? target.eulerAngles : target.localEulerAngles;

        if (usePos)
        {
            pos = GetEndVector(ref m_AnimationParam.Position, ref pos);
        }

        if (useRot)
        {
            rot = GetEndVector(ref m_AnimationParam.Rotation, ref rot);
        }

        if (m_SpaceType == Space.World)
        {
            target.SetPositionAndRotation(pos, Quaternion.Euler(rot));
        }
        else
        {
            target.localPosition = pos;
            target.localEulerAngles = rot;
        }
    }

    private Vector3 GetAnimVector(ref BattleAnimationParam.BattleAnimationVectorParam param, float currentTime, ref Vector3 initVector)
    {
        var vector = initVector;

        if (param.XParam.Use)
        {
            vector.x = param.XParam.Scale * param.XParam.AnimationValue.Evaluate(currentTime);
        }

        if (param.YParam.Use)
        {
            vector.y = param.YParam.Scale * param.YParam.AnimationValue.Evaluate(currentTime);
        }

        if (param.ZParam.Use)
        {
            vector.z = param.ZParam.Scale * param.ZParam.AnimationValue.Evaluate(currentTime);
        }

        return vector;
    }

    private Vector3 GetEndVector(ref BattleAnimationParam.BattleAnimationVectorParam param, ref Vector3 initVector)
    {
        var vector = initVector;

        if (param.XParam.Use)
        {
            vector.x = param.XParam.Scale * param.XParam.EndValue;
        }

        if (param.YParam.Use)
        {
            vector.y = param.YParam.Scale * param.YParam.EndValue;
        }

        if (param.ZParam.Use)
        {
            vector.z = param.ZParam.Scale * param.ZParam.EndValue;
        }

        return vector;
    }
}
