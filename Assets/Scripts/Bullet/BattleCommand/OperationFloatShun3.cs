#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等加速度直線運動から等速直線運動に移るような値の動きを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/shun3", fileName = "OperationFloatShun3", order = 0)]
[System.Serializable]
public class OperationFloatShun3 : OperationFloatBase
{

    /// <summary>
    /// 初速度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_V1;

    /// <summary>
    /// 最終的な速さ
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_V3;

    /// <summary>
    /// 最初の等速直線運動時の移動距離
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_X1;
    
    /// <summary>
    /// 等加速度直線運動時の移動距離
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_X2;

    /// <summary>
    /// 関数の引数にあたる値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;


    public override float GetResultFloat()
    {
        float v1 = m_V1.GetResultFloat();
        float v3 = m_V3.GetResultFloat();
        float x1 = m_X1.GetResultFloat();
        float x2 = m_X2.GetResultFloat();
        float t = m_T.GetResultFloat();

        // 運動が切り替わる時刻
        float t1 = x1 / v1;
        float t2 = 2 * x2 / (v1 + v3);
        float t12 = t1 + t2;

        if (t < t1)
        {
            return v1 * t;
        }
        else if(t < t12)
        {
            // 現在の速さ
            float v = v1 + (v3 - v1) * (t - t1) / t2;

            return x1 + (v1 + v) * (t - t1) / 2;
        }
        else
        {
            return x1 + x2 + v3 * (t - t12);
        }
    }
}