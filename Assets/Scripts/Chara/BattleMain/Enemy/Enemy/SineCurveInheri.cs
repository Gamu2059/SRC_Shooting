using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineCurveInheri : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // 発射位置
    [SerializeField]
    private Vector3 relativeLaunchPos;

    // way数
    [SerializeField]
    private int way;

    // 発射地点の円の半径
    [SerializeField]
    private float circleRadius;

    // 角速度
    [SerializeField]
    private float angleSpeed;

    // 弾の速さ
    [SerializeField]
    private float bulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected override float CalcNowShotNum()
    {
        return Time.time / shotInterval;
    }


    // 発射時刻を計算する
    protected override float CalcLaunchTime()
    {
        return shotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected override void ShotBullets(float launchTime, float dTime)
    {

        // 発射時の角度
        float pastRad = angleSpeed * launchTime;
        pastRad = Modulo2PI(pastRad);

        // 発射後の移動距離
        float distance = bulletSpeed * dTime;
    }
}