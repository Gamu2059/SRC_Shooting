using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 演算結果がfloat型になる演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationFloatBase : ScriptableObject
{
    /// <summary>
    /// floatの演算結果を取得する
    /// </summary>
    public abstract float GetResultFloat();
}
