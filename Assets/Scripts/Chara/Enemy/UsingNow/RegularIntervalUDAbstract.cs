using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射間隔が一定である単位弾幕の抽象クラス。
/// </summary>
public abstract class RegularIntervalUDAbstract : DanmakuCountAbstract2
{
    /// <summary>
    /// 発射間隔を取得する。
    /// </summary>
    public abstract float GetShotInterval();
    // ここを、強制的にm_Float[0]にしてやっちゃう？

    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / GetShotInterval();
    }

    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return GetShotInterval() * m_RealShotNum;
    }
}
