#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepWaveParticle : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // 弾源の円運動の半径
    [SerializeField]
    private float circleRadius;

    // 弾源の位置の位相
    [SerializeField]
    private float angle;

    // 弾源の位置の位相速度
    [SerializeField]
    private float angleSpeed;

    // 弾源の位置の位相加速度
    [SerializeField]
    private float angleAcceleration;

    // 弾源円の中心座標
    [SerializeField]
    private Vector3 circleCenterPos;

    // 全方位に弾を発射するかどうか
    [SerializeField]
    private bool omniDirection;

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


        // 前回発射時の角速度
        float pastAngleSpeed = angleSpeed;

        // 発射時の角速度
        angleSpeed += angleAcceleration * shotInterval;

        // 発射時の角度
        angle += (pastAngleSpeed + angleSpeed) / 2 * shotInterval;
        angle = Modulo2PI(angle);

        // 発射後の移動距離
        float distance = bulletSpeed * dTime;

        // このwayの発射角度
        float launchRad;

        if (omniDirection)
        {
            launchRad = Random.Range(0, Mathf.PI * 2);
        }
        else
        {
            launchRad = angle + Mathf.PI;
        }

        // 弾の位置を敵本体の位置にする
        Vector3 pos = transform.position;

        // 弾源の位置を弾源円の中心の位置にする
        pos += circleCenterPos;

        // 弾源の位置に移動する
        pos += new Vector3(circleRadius * Mathf.Cos(angle), 0, circleRadius * Mathf.Sin(angle));

        // 弾を経過した時間だけ進ませる
        pos += new Vector3(distance * Mathf.Cos(launchRad), 0, distance * Mathf.Sin(launchRad));

        // 弾の角度
        Vector3 eulerAngles = CalcEulerAngles(launchRad);

        // 弾を撃つ
        BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
        BulletController.ShotBullet(bulletShotParam);
    }
}