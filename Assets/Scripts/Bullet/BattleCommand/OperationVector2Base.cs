using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationVector2Base : ScriptableObject
{
    /// <summary>
    /// Vector2の演算結果を取得する
    /// </summary>
    public abstract Vector2 GetResultVector2();
}