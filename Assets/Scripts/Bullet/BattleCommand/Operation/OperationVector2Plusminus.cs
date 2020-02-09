using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の加減算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/plusminus", fileName = "OperationVector2Plusminus", order = 0)]
[System.Serializable]
public class OperationVector2Plusminus : OperationVector2Base
{

    /// <summary>
    /// 足す値の配列
    /// </summary>
    [SerializeField]
    private OperationVector2Base[] m_OperationPlus;

    /// <summary>
    /// 引く値の配列
    /// </summary>
    [SerializeField]
    private OperationVector2Base[] m_OperationMinus;


    public override Vector2 GetResultVector2()
    {
        Vector2 PlusResult = Vector2.zero;

        foreach (OperationVector2Base plus in m_OperationPlus)
        {
            PlusResult += plus.GetResultVector2();
        }

        Vector2 minusResult = Vector2.zero;

        foreach (OperationVector2Base minus in m_OperationMinus)
        {
            minusResult += minus.GetResultVector2();
        }

        return PlusResult - minusResult;
    }
}