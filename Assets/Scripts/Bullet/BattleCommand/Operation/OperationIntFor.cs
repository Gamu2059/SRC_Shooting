using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// for文を表す、int型の演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/for", fileName = "OperationIntFor", order = 0)]
[System.Serializable]
public class OperationIntFor : OperationIntBase
{

    /// <summary>
    /// for文を表すオブジェクト
    /// </summary>
    [SerializeField]
    private ForIntBase m_ForIntBase;


    public override float GetResultFloat()
    {
        return m_ForIntBase.GetValueInt();
    }


    public override int GetResultInt()
    {
        return m_ForIntBase.GetValueInt();
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


///// <summary>
///// 値
///// </summary>
////[SerializeField]
//private int m_Value;

///// <summary>
///// 初期値
///// </summary>
//[SerializeField]
//private OperationIntBase m_Init;

///// <summary>
///// 増分
///// </summary>
//[SerializeField]
//private OperationIntBase m_Inc;

///// <summary>
///// 回数
///// </summary>
//[UnityEngine.Serialization.FormerlySerializedAs("m_TimeNumber")]
//[SerializeField]
//private OperationIntBase m_TimeNum;


//public override void Process()
//{

//}


//public override void Init()
//{

//}


//public override bool IsTrue()
//{
//    //// 増えていくなら
//    //if (m_Inc.GetResultInt() > 0)
//    //{
//    //    return m_Value < m_Init.GetResultInt() + m_Inc.GetResultInt() * m_TimeNum.GetResultInt();
//    //}
//    //// 減っていくなら
//    //else
//    //{
//    //    return m_Value > m_Init.GetResultInt() + m_Inc.GetResultInt() * m_TimeNum.GetResultInt();
//    //}

//    ////return m_Value != m_Init + m_Inc * m_TimeNumOperation.GetResultInt();

//    return false;
//}
