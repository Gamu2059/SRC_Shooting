using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 速度を線形補間する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/velocityLinearInterpo", fileName = "OperationFloatVelocityLinearInterpo", order = 0)]
[System.Serializable]
public class OperationFloatVelocityLinearInterpo : OperationFloatBase
{

    /// <summary>
    /// 時刻
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;

    /// <summary>
    /// 速度と移動距離を表すオブジェクトの配列
    /// </summary>
    [SerializeField]
    private VelocityDistance[] m_VelocityDistance;

    /// <summary>
    /// 最後のポイントでの速度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_LastVelocity;

    /// <summary>
    /// 最後のポイント以降での加速度（nullなら隣の範囲と同じ加速度）
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_LastAcceleration;


    public override float GetResultFloat()
    {
        // 時刻
        float t = m_T.GetResultFloat();

        // スキャンで進んだ所までの時刻（区切りの時刻）
        float tempTime = 0;

        // スキャンで進んだ所までの距離
        float tempDistance = 0;

        // 配列のインデックス
        int i = 0;

        // 一時的な保存用
        float tI = 2 * m_VelocityDistance[i].Distance.GetResultFloat() /
                (m_VelocityDistance[i].Velocity.GetResultFloat() + m_VelocityDistance[i + 1].Velocity.GetResultFloat());

        while (i < m_VelocityDistance.Length - 1 && tempTime + tI < t)
        {
            tempTime += tI;

            tempDistance += m_VelocityDistance[i].Distance.GetResultFloat();

            i++;

            if (m_VelocityDistance.Length - 1 <= i)
            {
                break;
            }

            Debug.Log(i);

            tI = 2 * m_VelocityDistance[i].Distance.GetResultFloat() /
                (m_VelocityDistance[i].Velocity.GetResultFloat() + m_VelocityDistance[i + 1].Velocity.GetResultFloat());// 速度を二重に求めることになってしまう。
        }

        // 最後の区間以外の場合
        if (i < m_VelocityDistance.Length - 1)
        {
            // このフェーズの開始時の速度
            float vBegin = m_VelocityDistance[i].Velocity.GetResultFloat();

            // このフェーズの終了時の速度
            float vEnd = m_VelocityDistance[i + 1].Velocity.GetResultFloat();

            // このフェーズ全体で進む総距離
            float distanceSum = m_VelocityDistance[i].Distance.GetResultFloat();

            // 加速度
            float acceleration = (vEnd * vEnd - vBegin * vBegin) / (2 * distanceSum);

            // このフェーズ内で実際に進んだ距離
            float distance = vBegin * t + acceleration * t * t / 2;

            return tempDistance + distance;
        }
        // 最後の区間の場合
        else if(/*tempTime + m_VelocityDistance[m_VelocityDistance.Length - 1].Distance.GetResultFloat()*/false)
        {
            // このフェーズの開始時の速度
            float vBegin = m_VelocityDistance[m_VelocityDistance.Length - 1].Velocity.GetResultFloat();

            // このフェーズの終了時の速度
            float vEnd = m_LastVelocity.GetResultFloat();

            // このフェーズ全体で進む総距離
            float distanceSum = m_VelocityDistance[m_VelocityDistance.Length - 1].Distance.GetResultFloat();

            // 加速度
            float acceleration = (vEnd * vEnd - vBegin * vBegin) / (2 * distanceSum);

            // このフェーズ内で実際に進んだ距離
            float distance = vBegin * t + acceleration * t * t / 2;

            return tempDistance + distance;
        }
        // 最後の区間以降の場合
        else
        {
            // このフェーズの開始時の速度
            float vBegin = m_VelocityDistance[m_VelocityDistance.Length - 1].Velocity.GetResultFloat();

            // このフェーズの終了時の速度
            float vEnd = m_LastVelocity.GetResultFloat();

            // このフェーズ全体で進む総距離
            float distanceSum = m_VelocityDistance[m_VelocityDistance.Length - 1].Distance.GetResultFloat();

            // 加速度
            float acceleration = (vEnd * vEnd - vBegin * vBegin) / (2 * distanceSum);

            // このフェーズ内で実際に進んだ距離
            float distance = vBegin * t + acceleration * t * t / 2;

            return tempDistance + distance;
        }
    }
}


/// <summary>
/// 速度と移動距離を表すクラス。
/// </summary>
[System.Serializable]
class VelocityDistance : object
{

    /// <summary>
    /// 速度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Velocity;
    public OperationFloatBase Velocity
    {
        set { m_Velocity = value; }
        get { return m_Velocity; }
    }

    /// <summary>
    /// 速度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Distance;
    public OperationFloatBase Distance
    {
        set { m_Distance = value; }
        get { return m_Distance; }
    }
}