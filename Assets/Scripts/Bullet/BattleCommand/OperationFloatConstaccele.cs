#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値の等加速度運動を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/constaccele", fileName = "OperationFloatConstaccele", order = 0)]
[System.Serializable]
public class OperationFloatConstaccele : OperationFloatBase
{

    /// <summary>
    /// 初速度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_V0;

    /// <summary>
    /// 加速度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Acceleration;

    /// <summary>
    /// 関数の引数にあたる値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;


    public override float GetResultFloat()
    {
        float t = m_T.GetResultFloat();

        return m_V0.GetResultFloat() * t + m_Acceleration.GetResultFloat() * t * t / 2;
    }
}