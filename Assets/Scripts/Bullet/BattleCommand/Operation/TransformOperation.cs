using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵や弾の物理的な状態を表す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/transform/transform", fileName = "TransformOperation", order = 0)]
[System.Serializable]
public class TransformOperation : ScriptableObject
{

    /// <summary>
    /// 位置
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Position;

    /// <summary>
    /// 角度
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Angle;

    /// <summary>
    /// 大きさ
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Scale;


    /// <summary>
    /// 演算結果を取得する
    /// </summary>
    public TransformSimple GetResultValues()
    {
        return new TransformSimple(m_Position.GetResultVector2(), m_Angle.GetResultFloat(), m_Scale.GetResultFloat());
    }
}