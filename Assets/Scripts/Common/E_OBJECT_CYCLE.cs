using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトのサイクル。
/// </summary>
public enum E_OBJECT_CYCLE
{
    /// <summary>
    /// UPDATEの準備状態。
    /// </summary>
    STANDBY_UPDATE,

    /// <summary>
    /// 有効になっている状態。
    /// </summary>
    UPDATE,

    /// <summary>
    /// 破棄される準備状態。
    /// </summary>
    STANDBY_DESTROYED,

    /// <summary>
    /// 破棄された状態。
    /// </summary>
    DESTROYED,
}
