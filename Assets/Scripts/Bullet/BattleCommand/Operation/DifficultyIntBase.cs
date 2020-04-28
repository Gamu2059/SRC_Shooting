using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度ごとに異なるint値を表すクラスの抽象クラス。
/// </summary>
[System.Serializable]
public abstract class DifficultyIntBase : OperationIntBase
{

    /// <summary>
    /// 値
    /// </summary>
    public int Value { get; set; }


    public abstract void Setup();


    public override int GetResultInt()
    {
        return Value;
    }
}