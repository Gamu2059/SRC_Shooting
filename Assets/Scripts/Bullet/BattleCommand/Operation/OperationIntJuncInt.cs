using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の値によって条件分岐させ、int型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/junction/int", fileName = "OperationIntJuncInt", order = 0)]
[System.Serializable]
public class OperationIntJuncInt : OperationIntBase
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
    private OperationIntBase[] m_BranchValueArray;

    /// <summary>
    /// どの値にも当てはまらない場合の値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_LastBranchValue;


    public override float GetResultFloat()
    {
        //for (int i = 0; i < m_BranchValueArray.Length; i++)
        //{
        //    if (m_If.GetResultInt() == i)
        //    {
        //        return m_BranchValueArray[i].GetResultFloat();
        //    }
        //}

        //return m_LastBranchValue.GetResultFloat();

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


    public override int GetResultInt()
    {
        //for (int i = 0; i < m_BranchValueArray.Length; i++)
        //{
        //    if (m_If.GetResultInt() == i)
        //    {
        //        return m_BranchValueArray[i].GetResultInt();
        //    }
        //}

        //return m_LastBranchValue.GetResultInt();

        int index = m_If.GetResultInt();

        if (0 <= index && index < m_BranchValueArray.Length)
        {
            return m_BranchValueArray[index].GetResultInt();
        }
        else
        {
            return m_LastBranchValue.GetResultInt();
        }
    }
}