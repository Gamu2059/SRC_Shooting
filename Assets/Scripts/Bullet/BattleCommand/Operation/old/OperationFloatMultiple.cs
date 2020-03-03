//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// float型の掛け算を表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/multiple", fileName = "OperationFloatMultiple", order = 0)]
//[System.Serializable]
//public class OperationFloatMultiple : OperationFloatBase
//{

//    /// <summary>
//    /// 掛ける値の配列
//    /// </summary>
//    [SerializeField]
//    private OperationFloatBase[] m_OperationArray;


//    public override float GetResultFloat()
//    {
//        float result = 1;

//        foreach (OperationFloatBase operation in m_OperationArray)
//        {
//            result *= operation.GetResultFloat();
//        }

//        return result;
//    }
//}