using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度ごとに異なるVector2値を表すクラスの抽象クラス。
/// </summary>
[System.Serializable]
public abstract class DifficultyVector2Base : OperationVector2Base
{

    /// <summary>
    /// 値
    /// </summary>
    public Vector2 Value { get; set; }


    public abstract void Setup();


    public override Vector2 GetResultVector2()
    {
        return Value;
    }
}