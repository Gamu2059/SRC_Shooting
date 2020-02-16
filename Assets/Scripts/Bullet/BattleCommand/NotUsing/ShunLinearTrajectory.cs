using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShunLinearTrajectory : SimpleTrajectory
{

    /// <summary>
    /// 速さの下限
    /// </summary>
    public float m_MinSpeed;

    /// <summary>
    /// 最高速で進む距離
    /// </summary>
    public float m_FastDistance;

    /// <summary>
    /// 減速しながら進む距離
    /// </summary>
    public float m_SlowdownDistance;

    /// <summary>
    /// 最高速で進む時間
    /// </summary>
    public float m_FastTime;

    /// <summary>
    /// 減速中に進む時間
    /// </summary>
    public float m_SlowdownTime;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ShunLinearTrajectory(SimpleTrajectory trajectoryBase, float vMi, float dis1, float dis2) : base(trajectoryBase)
    {
        m_MinSpeed = vMi;
        m_FastDistance = dis1;
        m_SlowdownDistance = dis2;
        m_FastTime = m_FastDistance / m_Speed;
        m_SlowdownTime = 2 * m_SlowdownDistance / (m_Speed + m_MinSpeed);
    }


    public override TransformSimple GetTransform(float time)
    {
        float distance;

        if (time < m_FastTime)
        {
            distance = m_Speed * time;
        }
        else if (time < m_FastTime + m_SlowdownTime)
        {
            distance = - (time - m_FastTime) * (time - m_FastTime) * (m_Speed - m_MinSpeed) / m_SlowdownTime / 2 + m_Speed * (time - m_FastTime) + m_FastDistance;
        }
        else
        {
            distance = m_FastDistance + m_SlowdownDistance + m_MinSpeed * (time - (m_FastTime + m_SlowdownTime));
        }

        return new TransformSimple(
            m_BaseTransform.m_Position + Calc.RThetaToVec2(distance, m_BaseTransform.m_Angle),
            m_BaseTransform.m_Angle,
            m_BaseTransform.m_Scale
            );
    }
}
