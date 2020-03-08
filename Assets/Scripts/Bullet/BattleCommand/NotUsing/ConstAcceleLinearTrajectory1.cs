using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等加速度直線運動する弾のパラメータ（？）。シリアライズする?。
/// </summary>
public class ConstAcceleLinearTrajectory1 : SimpleTrajectory
{

    /// <summary>
    /// 加速度の大きさ
    /// </summary>
    public float m_Acceleration;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ConstAcceleLinearTrajectory1(SimpleTrajectory trajectoryBase, float acceleration) : base(trajectoryBase)
    {
        m_Acceleration = acceleration;
    }


    public override TransformSimple GetTransform(float time)
    {
        return new TransformSimple(
            m_BaseTransform.m_Position + (m_Speed * time + m_Acceleration * time * time / 2) * Calc.RThetaToVec2(1, m_BaseTransform.m_Angle),
            m_BaseTransform.m_Angle,
            m_BaseTransform.m_Scale
            );
    }
}
