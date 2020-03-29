//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 整数のデクリメント演算を表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/decrement", fileName = "OperationIntDecrement", order = 0)]
//[System.Serializable]
//public class OperationIntDecrement : OperationIntBase
//{

//    /// <summary>
//    /// デクリメントされる数
//    /// </summary>
//    [SerializeField]
//    private OperationIntBase m_Number;


//    public override float GetResultFloat()
//    {
//        return m_Number.GetResultInt() - 1;
//    }


//    public override int GetResultInt()
//    {
//        return m_Number.GetResultInt() - 1;
//    }
//}