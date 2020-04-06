#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の値によって条件分岐させ、Vector2型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/junction/float", fileName = "OperationVector2JuncFloat", order = 0)]
[System.Serializable]
public class OperationVector2JuncFloat : OperationVector2Base
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
    private DomoperFloatVector2[] m_BranchArray;

    /// <summary>
    /// どの領域内にもない場合の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_LastBranchValue;


    public override Vector2 GetResultVector2()
    {
        for (int i = 0; i < m_BranchArray.Length; i++)
        {
            if (m_BranchArray[i].DomainFloat.IsInsideFloat(m_If.GetResultFloat()))
            {
                return m_BranchArray[i].Vec2.GetResultVector2();
            }
        }

        return m_LastBranchValue.GetResultVector2();
    }
}