using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 演算結果がfloat型の領域になる領域演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class DomainFloatBase : ScriptableObject
{
    /// <summary>
    /// 与えられた一点の値が領域内にあるかどうか
    /// </summary>
    public abstract bool IsInsideFloat(float value);
}
