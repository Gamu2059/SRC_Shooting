using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リサージュ曲線上の点であるVector2型の値を生成する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/lissajous", fileName = "OperationVector2Lissajous", order = 0)]
[System.Serializable]
public class OperationVector2Lissajous : OperationVector2Base
{

    /// <summary>
    /// x方向の振幅
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_XAmplitude;

    /// <summary>
    /// y方向の振幅
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_YAmplitude;

    /// <summary>
    /// x方向の角振動数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_XFrequency;

    /// <summary>
    /// y方向の角振動数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_YFrequency;

    /// <summary>
    /// 進行度（媒介変数）
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;


    public override Vector2 GetResultVector2()
    {
        float t = m_T.GetResultFloat();

        return new Vector2(
            m_XAmplitude.GetResultFloat() * Mathf.Sin(m_XFrequency.GetResultFloat() * t),
            m_YAmplitude.GetResultFloat() * Mathf.Sin(m_YFrequency.GetResultFloat() * t)
            );
    }
}