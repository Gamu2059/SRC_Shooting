using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の演算（演算結果がfloat型になる演算）を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationFloatBase : ScriptableObject
{
    /// <summary>
    /// floatの演算結果を取得する
    /// </summary>
    public abstract float GetResultFloat();
}
