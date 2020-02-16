using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の軌道情報の抽象クラス。
/// </summary>
public abstract class TrajectoryBase : ScriptableObject
{
    public abstract TransformSimple GetTransform(TrajectoryBasis trajectoryBasis, float time);
}
