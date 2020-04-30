#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の値によって条件分岐させ、Vector2型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/junction/int", fileName = "OperationVector2JuncInt", order = 0)]
[System.Serializable]
public class OperationVector2JuncInt : OperationVector2Base
{

    /// <summary>
    /// この値によって条件分岐する
    /// </summary>
    [SerializeField]
    private OperationIntBase m_If;

    /// <summary>
    /// 条件分岐の配列
    /// </summary>
    [SerializeField]
    private OperationVector2Base[] m_Vector2Array;

    /// <summary>
    /// どの値にも当てはまらない場合の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2Last;


    public override Vector2 GetResultVector2()
    {
        int index = m_If.GetResultInt();

        if (0 <= index && index < m_Vector2Array.Length)
        {
            return m_Vector2Array[index].GetResultVector2();
        }
        else
        {
            return m_Vector2Last.GetResultVector2();
        }
    }
}