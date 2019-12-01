using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等加速度直線運動する弾のパラメータ（？）。シリアライズする?。
/// </summary>
public class ConstAcceleLinearTrajectory : TrajectoryBase
{

    /// <summary>
    /// 弾の進む向き
    /// </summary>
    public float m_Angle;

    /// <summary>
    /// 速さ
    /// </summary>
    public float m_Speed;

    /// <summary>
    /// 加速度の大きさ
    /// </summary>
    public float m_Acceleration;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ConstAcceleLinearTrajectory(float angle,float speed,float acceleration)
    {
        m_Angle = angle;
        m_Speed = speed;
        m_Acceleration = acceleration;
    }


    public override Vector3 GetPosition(float time,Vector3 basePosition)
    {
        return basePosition + (m_Speed * time + m_Acceleration * time * time / 2) * Calc.RThetaToVec3(1,m_Angle);
    }
}
