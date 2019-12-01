using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の軌道の抽象クラス。
/// </summary>
public abstract class TrajectoryBase : object
{
    /// <summary>
    /// 弾の位置を取得する。
    /// </summary>
    public abstract Vector3 GetPosition(float time, Vector3 basePosition);
}
