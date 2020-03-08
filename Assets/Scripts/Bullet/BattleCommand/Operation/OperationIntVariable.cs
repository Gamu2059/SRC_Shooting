using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の変数を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/variable", fileName = "OperationIntVariable", order = 0)]
[System.Serializable]
public class OperationIntVariable : OperationIntBase
{

    /// <summary>
    /// 値
    /// </summary>
    public int Value { get; set; }


    public override float GetResultFloat()
    {
        return Value;
    }

    public override int GetResultInt()
    {
        return Value;
    }
}
