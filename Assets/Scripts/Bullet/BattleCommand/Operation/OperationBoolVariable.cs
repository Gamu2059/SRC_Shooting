using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool型の変数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bool/variable", fileName = "OperationBoolVariable", order = 0)]
[System.Serializable]
public class OperationBoolVariable : OperationBoolBase
{

    /// <summary>
    /// 値
    /// </summary>
    public bool Value { get; set; }


    public override bool GetResultBool()
    {
        return Value;
    }
}





///// <summary>
///// 値を書き換える
///// </summary>
//public void SetValueFloat(float value)
//{
//    Value = value;
//}
