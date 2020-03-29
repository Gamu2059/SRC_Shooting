using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の発射からの時刻を取得するためのオブジェクトを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/bulletTime", fileName = "BulletTime", order = 0)]
[System.Serializable]
public class BulletTime : OperationFloatBase
{
    /// <summary>
    /// 弾の発射からの時刻
    /// </summary>
    public static float m_Time { set; get; }

    public override float GetResultFloat()
    {
        return m_Time;
    }
}
