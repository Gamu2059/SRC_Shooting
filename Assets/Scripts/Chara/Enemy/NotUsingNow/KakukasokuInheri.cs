#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KakukasokuInheri : DanmakusAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // 角加速度
    [SerializeField]
    private float angleAcceleration;

    // 角速度
    [SerializeField]
    private float angleSpeed;

    // 角度
    [SerializeField]
    private float angle;

    // 進み具合
    [SerializeField]
    private float phase;

    // 弾の速さ
    [SerializeField]
    private float bulletSpeed = 10;


    protected override void Awake()
    {
        CalcRealShotNumDelegate[] calcRealShotNum = { ShotNum };
        CalcLaunchTimeDelegate[] calcLaunchTime = { LaunchTime };
        ShotBulletsDelegate[] shotBullets = { ShotBullets };

        base.Awake(calcRealShotNum, calcLaunchTime, shotBullets);
    }


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected float ShotNum()
    {
        return Time.time / shotInterval;
    }


    // 発射時刻を計算する
    protected float LaunchTime(float realShotNum)
    {
        return shotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected void ShotBullets(float launchTime, float dTime)
    {
        Vector3 pos = transform.position;

        float pastAngleSpeed = angleSpeed;

        angleSpeed += angleAcceleration * shotInterval;

        angle += (pastAngleSpeed + angleSpeed) / 2 * shotInterval;
        angle = Modulo2PI(angle);

        float distance = bulletSpeed * dTime;

        pos += new Vector3(distance * Mathf.Cos(angle), 0, distance * Mathf.Sin(angle));

        Vector3 eulerAngles;

        eulerAngles = CalcEulerAngles(angle);

        BulletParam bulletParam = GetBulletParam();

        BulletShotParam bulletShotParam = new BulletShotParam(this,0,0,0,pos, eulerAngles,transform.localScale);

        BulletController.ShotBullet(bulletShotParam);

        phase = angleSpeed / (Mathf.PI * 2) * shotInterval;
    }
}