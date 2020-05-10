using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射の瞬間の、弾の発射からの時刻を取得するためのオブジェクトを表すクラス。（スクリプタブルオブジェクトにする必要はあるか？）
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/bulletTime", fileName = "BulletTime", order = 0)]
[System.Serializable]
public class BulletDTime : OperationFloatBase
{
    /// <summary>
    /// 弾の発射からの時刻
    /// </summary>
    public static float DTime { set; get; }

    public override float GetResultFloat()
    {
        return DTime;
    }
}
