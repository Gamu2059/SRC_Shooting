#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinCurveInheri : DanmakusAbstract
{

    // 発射間隔
    [SerializeField]
    private float SineshotInterval;

    // way数
    [SerializeField]
    private int Sineway;

    // 発射地点の円の半径
    [SerializeField]
    private float circleRadius;

    // 角速度
    [SerializeField]
    private float angleSpeed;

    // 弾の速さ
    [SerializeField]
    private float SinebulletSpeed = 10;

    // 発射間隔
    [SerializeField]
    private float omniShotInterval;

    // way数
    [SerializeField]
    private int omniWay;

    // 弾の速さ
    [SerializeField]
    private float omniBulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected float SineShotNum()
    {
        return Time.time / SineshotInterval;
    }


    // 発射時刻を計算する
    protected float SineLaunchTime(float realShotNum)
    {
        return SineshotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected void SineShotBullets(float launchTime, float dTime)
    {

        float pastRad = angleSpeed * launchTime;
        pastRad = Modulo2PI(pastRad);

        float distance = SinebulletSpeed * dTime;

        for (int i = 0; i < Sineway; i++)
        {
            // way数による角度
            float wayRad = Mathf.PI * i / Sineway + Mathf.PI / Sineway / 2;

            // 正弦波のどの位置か(1次元に投影した時の位置)
            float phase = Mathf.Cos(pastRad - wayRad);

            Vector3 pos = transform.position;

            pos += new Vector3(circleRadius * phase * Mathf.Cos(wayRad), 0, circleRadius * phase * Mathf.Sin(wayRad));
            pos += new Vector3(distance * Mathf.Cos(wayRad + Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad + Mathf.PI / 2));

            Vector3 eulerAngles;

            eulerAngles = CalcEulerAngles(wayRad + Mathf.PI / 2);

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
            BulletController.ShotBullet(bulletShotParam);

            pos = transform.position;
            pos += new Vector3(circleRadius * phase * Mathf.Cos(wayRad), 0, circleRadius * phase * Mathf.Sin(wayRad));
            pos += new Vector3(distance * Mathf.Cos(wayRad - Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad - Mathf.PI / 2));

            eulerAngles = CalcEulerAngles(wayRad - Mathf.PI / 2);

            // 弾を撃つ
            bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }


    // 現在のあるべき発射回数を計算する(小数)
    protected float OmniShotNum()
    {
        return Time.time / omniShotInterval;
    }


    // 発射時刻を計算する
    protected float OmniLaunchTime(float realShotNum)
    {
        return omniShotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected void OmniShotBullets(float launchTime, float dTime)
    {

        float distance = omniBulletSpeed * dTime;

        // ランダムな角度
        float rad0 = Random.Range(0, Mathf.PI * 2);

        for (int i = 0; i < omniWay; i++)
        {
            // 1つの弾の角度
            float rad = rad0 + Mathf.PI * 2 * i / omniWay;

            // その弾の発射角度
            Vector3 eulerAngles;
            eulerAngles = CalcEulerAngles(rad);

            // 発射された弾の現在の位置
            Vector3 pos = transform.position;
            pos += new Vector3(distance * Mathf.Cos(rad), 0, distance * Mathf.Sin(rad));

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }


    protected void Awake()
    {
        CalcRealShotNum[] calcRealShotNum = { SineShotNum, OmniShotNum };
        CalcLaunchTime[] calcLaunchTime = { SineLaunchTime, OmniLaunchTime };
        ShotBullets[] shotBullets = { SineShotBullets, OmniShotBullets };

        base.Awake(calcRealShotNum, calcLaunchTime, shotBullets);
    }
}