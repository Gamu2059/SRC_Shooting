#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 矩形内にあるかどうかのbool型を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/rectInside", fileName = "OperationBoolRectInside", order = 0)]
[System.Serializable]
public class OperationBoolRectInside : OperationBoolBase
{

    /// <summary>
    /// Vector2型の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Value;

    /// <summary>
    /// 矩形の左端のx座標
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_XMin;

    /// <summary>
    /// 矩形の右端のx座標
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_XMax;

    /// <summary>
    /// 矩形の上端のy座標
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_YMin;

    /// <summary>
    /// 矩形の下端のy座標
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_YMax;


    public override bool GetResultBool()
    {
        Vector2 value = m_Value.GetResultVector2();

        return
            m_XMin.GetResultFloat() <= value.x && value.x <= m_XMax.GetResultFloat() && 
            m_YMin.GetResultFloat() <= value.y && value.y <= m_YMax.GetResultFloat();
    }
}
