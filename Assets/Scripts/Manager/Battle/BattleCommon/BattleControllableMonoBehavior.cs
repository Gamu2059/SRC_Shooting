using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControllableMonoBehavior : ControllableMonoBehavior
{
    /// <summary>
    /// 状態遷移によって、このオブジェクトが有効になった時に呼び出される。
    /// </summary>
    public virtual void OnEnableObject() { }

    /// <summary>
    /// 状態遷移によって、このオブジェクトが無効になった時に呼び出される。
    /// </summary>
    public virtual void OnDisableObject() { }
}
