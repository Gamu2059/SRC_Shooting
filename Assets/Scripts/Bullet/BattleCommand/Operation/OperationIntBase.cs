using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationIntBase : OperationFloatBase
{
    /// <summary>
    /// intの演算結果を取得する
    /// </summary>
    public abstract int GetResultInt();
}
