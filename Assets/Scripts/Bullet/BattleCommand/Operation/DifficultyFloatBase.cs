using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度ごとに異なるfloat値を表すクラスの抽象クラス。
/// </summary>
[System.Serializable]
public abstract class DifficultyFloatBase : OperationFloatBase
{

    /// <summary>
    /// 値
    /// </summary>
    public float Value { get; set; }


    public abstract void Setup();


    public override float GetResultFloat()
    {
        return Value;
    }
}