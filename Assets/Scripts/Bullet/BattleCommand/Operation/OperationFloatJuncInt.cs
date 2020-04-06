#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の値によって条件分岐させ、float型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/junction/int", fileName = "OperationFloatJuncInt", order = 0)]
[System.Serializable]
public class OperationFloatJuncInt : OperationFloatBase
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
    private OperationFloatBase[] m_BranchValueArray;

    /// <summary>
    /// どの値にも当てはまらない場合の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_LastBranchValue;


    public override float GetResultFloat()
    {
        int index = m_If.GetResultInt();

        if (0 <= index && index < m_BranchValueArray.Length)
        {
            return m_BranchValueArray[index].GetResultFloat();
        }
        else
        {
            return m_LastBranchValue.GetResultFloat();
        }
    }
}




//for (int i = 0; i < m_BranchValueArray.Length; i++)
//{
//    if (m_If.GetResultInt() == i)
//    {
//        return m_BranchValueArray[i].GetResultFloat();
//    }
//}

//return m_LastBranchValue.GetResultFloat();
