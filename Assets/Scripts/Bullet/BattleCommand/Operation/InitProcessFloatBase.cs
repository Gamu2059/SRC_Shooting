using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InitとProcess動作を持ち、float型の値を持つクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class InitProcessFloatBase : InitProcessBase
{
    /// <summary>
    /// 値を取得する
    /// </summary>
    public abstract float GetValueFloat();
}
