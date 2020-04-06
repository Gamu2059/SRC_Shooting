#pragma warning disable 0649

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

        if (m_Plus != null)
        {
            foreach (OperationFloatBase operation in m_Plus)
            {
                resultPlus += operation.GetResultFloat();
            }
        }

        float resultMinus = 0;

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