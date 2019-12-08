using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アルキメデスの螺旋を描く軌道を表すクラス。
/// </summary>
public class ArchimedesTrajectory : SimpleTrajectory
{

    /// <summary>
    /// 角速度
    /// </summary>
    public float m_AngleSpeed;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ArchimedesTrajectory(SimpleTrajectory trajectoryBase, float angleSpeed) : base(trajectoryBase)
    {
        m_AngleSpeed = angleSpeed;
    }


    public override TransformSimple GetTransform(float time)
    {
        return new TransformSimple(
            m_BaseTransform.m_Position + Calc.RThetaToVec3(m_Speed * time, m_Angle + m_AngleSpeed * time),
            m_BaseTransform.m_Angle,
            m_BaseTransform.m_Scale
            );
    }
}
