using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// for文を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class ForBase : InitProcessBase
{
    /// <summary>
    /// 条件判定をする
    /// </summary>
    public abstract bool IsTrue();
}
