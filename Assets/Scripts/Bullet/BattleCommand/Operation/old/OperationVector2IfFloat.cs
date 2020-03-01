//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// float型の値によって条件分岐させ、Vector2型の値を生成する演算を表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/if/float", fileName = "OperationVector2IfFloat", order = 0)]
//[System.Serializable]
//public class OperationVector2IfFloat : OperationVector2Base
//{

//    /// <summary>
//    /// この値によって条件分岐する
//    /// </summary>
//    [SerializeField]
//    private OperationFloatBase m_Float;

//    /// <summary>
//    /// 条件分岐の配列
//    /// </summary>
//    [SerializeField]
//    private Vector2Float[] m_BranchArray;

//    /// <summary>
//    /// 最も大きい値の域に属する場合
//    /// </summary>
//    //[UnityEngine.Serialization.FormerlySerializedAs("m_LastBranchValue")]
//    [SerializeField]
//    private OperationVector2Base m_LastBranchValue;

//    /// <summary>
//    /// ローカルなfloat値
//    /// </summary>
//    //[UnityEngine.Serialization.FormerlySerializedAs("m_LocalFloat")]
//    [SerializeField]
//    private OperationFloatVariable m_LocalFloat;


//    public override Vector2 GetResultVector2()
//    {
//        m_LocalFloat.Value = m_Float.GetResultFloat();

//        for (int i = 0;i < m_BranchArray.Length;i++)
//        {
//            if (m_Float.GetResultFloat() <= m_BranchArray[i].m_Float.GetResultFloat())
//            {
//                for (int j = 0; j < i; j++)
//                {
//                    m_LocalFloat.Value -= m_BranchArray[j].m_Float.GetResultFloat();
//                }

//                return m_BranchArray[i].m_Vector2.GetResultVector2();
//            }
//        }

//        for (int j = 0; j < m_BranchArray.Length; j++)
//        {
//            m_LocalFloat.Value -= m_BranchArray[j].m_Float.GetResultFloat();
//        }

//        return m_LastBranchValue.GetResultVector2();
//    }
//}