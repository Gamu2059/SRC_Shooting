using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントの壁の状態。
/// </summary>
public enum E_WALL_CYCLE
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
