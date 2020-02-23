//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// int型の操作のある線形の演算を表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/process/linear", fileName = "OperationIntProcLinear", order = 0)]
//[System.Serializable]
//public class OperationIntProcLinear : OperationIntProcBase
//{

//    /// <summary>
//    /// 値
//    /// </summary>
//    //[SerializeField]
//    private int m_Value;

//    /// <summary>
//    /// 初期値
//    /// </summary>
//    [SerializeField]
//    private int m_Init;

//    /// <summary>
//    /// 増分
//    /// </summary>
//    [SerializeField]
//    private int m_Inc;


//    public override float GetResultFloat()
//    {
//        return m_Value;
//    }


//    public override int GetResultInt()
//    {
//        return m_Value;
//    }


//    public override void Init()
//    {
//        m_Value = m_Init;
//    }


//    public override void Process()
//    {
//        m_Value += m_Inc;
//    }
//}
