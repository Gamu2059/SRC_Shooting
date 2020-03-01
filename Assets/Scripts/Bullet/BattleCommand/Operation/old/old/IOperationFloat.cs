using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float型の演算（演算結果がfloat型になる演算）を表すインターフェース。
/// </summary>
public interface IOperationFloat
{
    /// <summary>
    /// 演算結果を取得する
    /// </summary>
    float GetResult();
}
