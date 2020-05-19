using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の変数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/variable", fileName = "OperationVector2Variable", order = 0)]
[System.Serializable]
public class OperationVector2Variable : OperationVector2Base
{

    /// <summary>
    /// 値
    /// </summary>
    public Vector2 Value { get; set; }


    public override Vector2 GetResultVector2()
    {
        return Value;
    }
}





///// <summary>
///// 値を書き換える
///// </summary>
//public void SetValueVector2(Vector2 value)
//{
//    Value = value;
//}
