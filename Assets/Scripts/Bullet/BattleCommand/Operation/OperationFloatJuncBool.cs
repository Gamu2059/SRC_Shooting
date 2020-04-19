#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool型の値によって条件分岐させ、float型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/junction/bool", fileName = "OperationFloatJuncBool", order = 0)]
[System.Serializable]
public class OperationFloatJuncBool : OperationFloatBase
{

    /// <summary>
    /// この値によって条件分岐する
    /// </summary>
    [SerializeField]
    private OperationBoolBase[] m_If;

    /// <summary>
    /// trueだった場合の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase[] m_True;

    /// <summary>
    /// falseだった場合の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_False;


    public override float GetResultFloat()
    {
        for (int i = 0; i < m_If.Length; i++)
        {
            if (m_If[i].GetResultBool())
            {
                return m_True[i].GetResultFloat();
            }
        }

        return m_False.GetResultFloat();
    }
}