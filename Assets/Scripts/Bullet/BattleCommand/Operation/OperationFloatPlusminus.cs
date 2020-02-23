using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の加減算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/plusminus", fileName = "OperationFloatPlusminus", order = 0)]
[System.Serializable]
public class OperationFloatPlusminus : OperationFloatBase
{

    /// <summary>
    /// 足す値の配列
    /// </summary>
    [UnityEngine.Serialization.FormerlySerializedAs("m_OperationArray")]
    [SerializeField]
    private OperationFloatBase[] m_Plus;

    /// <summary>
    /// 引く値の配列
    /// </summary>
    [SerializeField]
    private OperationFloatBase[] m_Minus;


    public override float GetResultFloat()
    {
        float resultPlus = 0;

        foreach (OperationFloatBase operation in m_Plus)
        {
            resultPlus += operation.GetResultFloat();
        }

        float resultMinus = 0;

        // なぜかnullになったので
        if (m_Minus != null)
        {
            foreach (OperationFloatBase operation in m_Minus)
            {
                resultMinus += operation.GetResultFloat();
            }
        }

        return resultPlus - resultMinus;
    }
}