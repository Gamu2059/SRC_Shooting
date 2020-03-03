using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 演算を表すクラスの基底クラス。
/// </summary>
public abstract class OperationBase<T> : ScriptableObject
{

    /// <summary>
    /// 演算結果を取得する
    /// </summary>
    public abstract T GetResult();
}