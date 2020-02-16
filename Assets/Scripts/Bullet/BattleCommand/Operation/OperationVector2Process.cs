using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InitとProcess動作をする、Vector2型の演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/process", fileName = "OperationVector2Process", order = 0)]
[System.Serializable]
public class OperationVector2Process : OperationVector2Base
{

    /// <summary>
    /// InitProcess動作のあるオブジェクト
    /// </summary>
    [SerializeField]
    private InitProcessVector2 m_InitProcessVector2;


    public override Vector2 GetResultVector2()
    {
        return m_InitProcessVector2.GetValueVector2();
    }
}




///// <summary>
///// 値
///// </summary>
////[SerializeField]
//private Vector2 m_Value;

///// <summary>
///// 半径
///// </summary>
//[SerializeField]
//private OperationFloatBase m_Radius;


//public override void Init()
//{
//    //Process();
//}


//public override void Process()
//{
//    //m_Value = Random.insideUnitCircle * m_Radius.GetResultFloat();
//}
