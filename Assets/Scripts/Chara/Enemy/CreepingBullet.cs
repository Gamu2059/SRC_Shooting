using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepingBullet : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // 弾源の位置の位相
    [SerializeField]
    private float angle;

    // 弾源の位置の位相速度
    [SerializeField]
    private float angleSpeed;

    // 弾源の位置の位相速度
    [SerializeField]
    private float[] shotRads;

    // 弾をばらまく角度の大きさの半分
    [SerializeField]
    private float ampRad;

    // 弾の速さ
    [SerializeField]
    private float bulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {
        // 角度を進める
        float rad = angleSpeed * Time.time;
        rad = Modulo2PI(rad);

        // 角度を反映させる
        transform.eulerAngles = CalcEulerAngles(rad);
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
        // 弾の位置を発射時の位置にする
        Vector3 pos = transform.position;

        // 発射時の角度
        float pastAngle = angleSpeed * launchTime;
        pastAngle = Modulo2PI(pastAngle);

        // 発射後の移動距離
        float distance = bulletSpeed * dTime;

        // 弾を経過した時間だけ進ませる
        pos += new Vector3(distance * Mathf.Cos(pastAngle), 0, distance * Mathf.Sin(pastAngle));

        // 弾の角度
        Vector3 eulerAngles = CalcEulerAngles(pastAngle);

        // 弾を撃つ
        //ShotBullet(0, 0, pos, eulerAngles);
    }
}