using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度ごとに異なるfloat型の演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationFloatDifficultyBase : OperationFloatBase
{

    /// <summary>
    /// 実際の難易度をもとに代入する
    /// </summary>
    public abstract void Setup(E_DIFFICULTY? difficulty);
}
