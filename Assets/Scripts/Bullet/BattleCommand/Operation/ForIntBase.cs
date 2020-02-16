using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// for文を表し、int型の値を持つクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class ForIntBase : ForBase
{
    /// <summary>
    /// 値を取得する
    /// </summary>
    public abstract int GetValueInt();
}
