using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の条件付き操作のある線形の演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/process/conditional/linear", fileName = "OperationIntProcCondLinear", order = 0)]
[System.Serializable]
public class OperationIntProcCondLinear : OperationIntProcCondBase
{

    /// <summary>
    /// 値
    /// </summary>
    //[SerializeField]
    private int m_Value;

    /// <summary>
    /// 初期値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Init;

    /// <summary>
    /// 増分
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Inc;

    /// <summary>
    /// 回数
    /// </summary>
    [SerializeField]
    private OperationIntBase m_TimeNum;


    public override float GetResultFloat()
    {
        return m_Value;
    }


    public override int GetResultInt()
    {
        return m_Value;
    }


    public override void Process()
    {
        m_Value += m_Inc.GetResultInt();
    }


    public override void Init()
    {
        m_Value = m_Init.GetResultInt();
    }


    public override bool IsTrue()
    {
        // 増えていくなら
        if (m_Inc.GetResultInt() > 0)
        {
            return m_Value < m_Init.GetResultInt() + m_Inc.GetResultInt() * m_TimeNum.GetResultInt();
        }
        // 減っていくなら
        else
        {
            return m_Value > m_Init.GetResultInt() + m_Inc.GetResultInt() * m_TimeNum.GetResultInt();
        }

        //return m_Value != m_Init + m_Inc * m_TimeNumOperation.GetResultInt();
    }
}





//// 増えていくなら
//if (m_Inc > 0)
//{
//    return m_Value < m_Init + m_Inc * m_TimeNum;
//}
//// 減っていくなら
//else
//{
//    return m_Value > m_Init + m_Inc * m_TimeNum;
//}