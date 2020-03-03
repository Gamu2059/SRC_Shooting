//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// InitとProcess動作をする、float型の演算を表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/process", fileName = "OperationFloatProcess", order = 0)]
//[System.Serializable]
//public class OperationFloatProcess : OperationFloatBase
//{

//    /// <summary>
//    /// InitProcess動作のあるオブジェクト
//    /// </summary>
//    [SerializeField]
//    private InitProcessFloatBase m_InitProcessFloat;


//    public override float GetResultFloat()
//    {
//        return m_InitProcessFloat.GetValueFloat();
//    }
//}





/////// <summary>
/////// 値
/////// </summary>
//////[SerializeField]
////private float m_Value;

/////// <summary>
/////// 最小値
/////// </summary>
////[SerializeField]
////private OperationFloatBase m_Min;

/////// <summary>
/////// 最大値
/////// </summary>
////[SerializeField]
////private OperationFloatBase m_Max;


////public override void Init()
////{
////    //m_Value = Random.Range(m_Min.GetResultFloat(), m_Max.GetResultFloat());
////}


////public override void Process()
////{
////    //m_Value = Random.Range(m_Min.GetResultFloat(), m_Max.GetResultFloat());
////}
