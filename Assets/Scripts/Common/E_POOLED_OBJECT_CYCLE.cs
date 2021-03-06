﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プールされるオブジェクトのサイクル。
/// </summary>
public enum E_POOLED_OBJECT_CYCLE
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
    /// プール準備状態になることを待っている状態。
    /// </summary>
    STANDBY_CHECK_POOL,

    /// <summary>
    /// プールされる準備状態。
    /// </summary>
    STANDBY_POOL,

    /// <summary>
    /// プールされた状態。
    /// </summary>
    POOLED,
}
