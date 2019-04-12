using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum E_BULLET_CYCLE
{
    /// <summary>
    /// 発射される直前。
    /// </summary>
    STANDBY_UPDATE,

    /// <summary>
    /// 発射された後、動いている状態。
    /// </summary>
    UPDATE,

    /// <summary>
    /// プールされる準備状態。
    /// </summary>
    STANDBY_POOL,

    /// <summary>
    /// プーリングされた状態。
    /// </summary>
    POOLED,
}