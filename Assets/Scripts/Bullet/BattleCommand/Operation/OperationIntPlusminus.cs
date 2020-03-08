using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の加減算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/plusminus", fileName = "OperationIntPlusminus", order = 0)]
[System.Serializable]
public class OperationIntPlusminus : OperationIntBase
{

    /// <summary>
    /// 足す値の配列
    /// </summary>
    [SerializeField]
    private OperationIntBase[] m_Plus;

    /// <summary>
    /// 引く値の配列
    /// </summary>
    [SerializeField]
    private OperationIntBase[] m_Minus;


    public override float GetResultFloat()
    {
        int resultPlus = 0;

        if (m_Plus != null)
        {
            foreach (OperationIntBase operation in m_Plus)
            {
                resultPlus += operation.GetResultInt();
            }
        }

        int resultMinus = 0;

        if (m_Minus != null)
        {
            foreach (OperationIntBase operation in m_Minus)
            {
                resultMinus += operation.GetResultInt();
            }
        }

        return resultPlus - resultMinus;
    }


    public override int GetResultInt()
    {
        int resultPlus = 0;

        if (m_Plus != null)
        {
            foreach (OperationIntBase operation in m_Plus)
            {
                resultPlus += operation.GetResultInt();
            }
        }

        int resultMinus = 0;

        if (m_Minus != null)
        {
            foreach (OperationIntBase operation in m_Minus)
            {
                resultMinus += operation.GetResultInt();
            }
        }

        return resultPlus - resultMinus;
    }
}