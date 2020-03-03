using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の剰余の値によって条件分岐させ、Vector2型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/modulo", fileName = "OperationVector2Modulo", order = 0)]
[System.Serializable]
public class OperationVector2Modulo : OperationVector2Base
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
    private OperationVector2Base[] m_BranchValueArray;


    public override Vector2 GetResultVector2()
    {
        // 剰余
        int remainder = m_If.GetResultInt() % m_BranchValueArray.Length;

        return m_BranchValueArray[remainder].GetResultVector2();
    }
}