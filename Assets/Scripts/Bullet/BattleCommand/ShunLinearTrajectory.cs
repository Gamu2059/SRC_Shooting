using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShunLinearTrajectory : TrajectoryBase
{
    /// <summary>
    /// 弾の進む向き
    /// </summary>
    public float m_Angle;

    /// <summary>
    /// 速さの上限
    /// </summary>
    public float vMax;

    /// <summary>
    /// 速さの下限
    /// </summary>
    public float vMin;

    /// <summary>
    /// 最高速で進む距離
    /// </summary>
    public float d1;

    /// <summary>
    /// 減速しながら進む距離
    /// </summary>
    public float d2;

    /// <summary>
    /// 最高速で進む時間
    /// </summary>
    public float m_T1;

    /// <summary>
    /// 減速中に進む時間
    /// </summary>
    public float m_T2;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ShunLinearTrajectory(float angle, float vMa, float vMi, float dis1, float dis2)
    {
        m_Angle = angle;
        vMax = vMa;
        vMin = vMi;
        d1 = dis1;
        d2 = dis2;
    }


    public override Vector3 GetPosition(float time, Vector3 basePosition)
    {
        float t1 = d1 / vMax;
        float t2 = 2 * d2 / (vMax + vMin);
        float d;

        if (time < t1)
        {
            d = vMax * time;
        }
        else if (time < t1 + t2)
        {
            d = - (time - t1) * (time - t1) * (vMax - vMin) / t2 / 2 + vMax * (time - t1) + d1;
        }
        else
        {
            d = d1 + d2 + vMin * (time - (t1 + t2));
        }

        return basePosition + Calc.RThetaToVec3(d,m_Angle);
    }
}
