using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2型の操作のある演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationVector2ProcBase : OperationVector2Base
{
    /// <summary>
    /// 初期値を設定する
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 操作をする
    /// </summary>
    public abstract void Process();
}
