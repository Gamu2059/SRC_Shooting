using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 演算結果がbool型になる演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationBoolBase : ScriptableObject
{
    /// <summary>
    /// bool型の演算結果を取得する
    /// </summary>
    public abstract bool GetResultBool();
}
