//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// InitとProcess動作を持ち、乱数であるfloat型の値を持つクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/initProcess/float/random", fileName = "InitProcessFloatRandom", order = 0)]
//[System.Serializable]
//public class InitProcessFloatRandom : InitProcessFloatBase
//{

//    /// <summary>
//    /// 値
//    /// </summary>
//    //[SerializeField]
//    private float m_Value;

//    /// <summary>
//    /// 最小値
//    /// </summary>
//    [SerializeField]
//    private OperationFloatBase m_Min;

//    /// <summary>
//    /// 最大値
//    /// </summary>
//    [SerializeField]
//    private OperationFloatBase m_Max;


//    public override void Init()
//    {
//        Process();
//    }


//    public override void Process()
//    {
//        m_Value = Random.Range(m_Min.GetResultFloat(), m_Max.GetResultFloat());
//    }


//    public override float GetValueFloat()
//    {
//        return m_Value;
//    }
//}
