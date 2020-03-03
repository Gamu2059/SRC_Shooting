using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の剰余の値によって条件分岐させ、float型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/modulo", fileName = "OperationFloatModulo", order = 0)]
[System.Serializable]
public class OperationFloatModulo : OperationFloatBase
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


    public override float GetResultFloat()
    {
        // 剰余
        int remainder = m_If.GetResultInt() % m_BranchValueArray.Length;

        return m_BranchValueArray[remainder].GetResultFloat();
    }
}