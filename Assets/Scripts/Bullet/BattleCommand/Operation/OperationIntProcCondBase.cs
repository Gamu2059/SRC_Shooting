using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型の条件付き操作のある演算を表すクラスの基底クラス。
/// </summary>
[System.Serializable]
public abstract class OperationIntProcCondBase : OperationIntProcBase
{
    /// <summary>
    /// 条件判定をする
    /// </summary>
    public abstract bool IsTrue();
}
