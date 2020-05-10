using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 前フレームからの時間を取得するためのオブジェクトを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/bulletDeltaTime", fileName = "BulletDeltaTime", order = 0)]
[System.Serializable]
public class BulletDeltaTime : OperationFloatBase
{
    /// <summary>
    /// 前フレームからの時間
    /// </summary>
    public static float DeltaTime { set; get; }

    public override float GetResultFloat()
    {
        return DeltaTime;
    }
}
