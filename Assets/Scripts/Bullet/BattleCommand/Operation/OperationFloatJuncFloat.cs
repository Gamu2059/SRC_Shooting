using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の値によって条件分岐させ、float型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/junction/float", fileName = "OperationFloatJuncFloat", order = 0)]
[System.Serializable]
public class OperationFloatJuncFloat : OperationFloatBase
{

    /// <summary>
    /// この値によって条件分岐する
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_If;

    /// <summary>
    /// 条件分岐の配列
    /// </summary>
    [SerializeField]
    private DomoperFloatFloat[] m_BranchArray;

    /// <summary>
    /// どの領域内にもない場合の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_LastBranchValue;


    public override float GetResultFloat()
    {
        for (int i = 0; i < m_BranchArray.Length; i++)
        {
            if (m_BranchArray[i].DomainFloat.IsInsideFloat(m_If.GetResultFloat()))
            {
                return m_BranchArray[i].Float.GetResultFloat();
            }
        }

        return m_LastBranchValue.GetResultFloat();
    }
}