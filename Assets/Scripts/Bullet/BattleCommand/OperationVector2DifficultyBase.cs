using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度ごとに異なるVector2型の演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationVector2DifficultyBase : OperationVector2Base
{

    /// <summary>
    /// 実際の難易度をもとに代入する
    /// </summary>
    public abstract void Setup(E_DIFFICULTY? difficulty);
}
