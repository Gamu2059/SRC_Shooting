//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// InitとProcess動作を持ち、円内の乱ベクトルであるVector2型の値を持つクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/initProcess/Vector2/randomincircle", fileName = "InitProcessVector2Randomincircle", order = 0)]
//[System.Serializable]
//public class InitProcessVector2Randomincircle : InitProcessVector2
//{

//    /// <summary>
//    /// 値
//    /// </summary>
//    //[SerializeField]
//    private Vector2 m_Value;

//    /// <summary>
//    /// 半径
//    /// </summary>
//    [SerializeField]
//    private OperationFloatBase m_Radius;


//    public override void Init()
//    {
//        Process();
//    }


//    public override void Process()
//    {
//        m_Value = Random.insideUnitCircle * m_Radius.GetResultFloat();
//    }


//    public override Vector2 GetValueVector2()
//    {
//        return m_Value;
//    }
//}
