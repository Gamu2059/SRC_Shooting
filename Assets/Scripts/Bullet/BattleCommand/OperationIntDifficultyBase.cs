using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度ごとに異なるint型の演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationIntDifficultyBase : OperationIntBase
{

    /// <summary>
    /// 実際の難易度をもとに代入する
    /// </summary>
    public abstract void Setup();
}
