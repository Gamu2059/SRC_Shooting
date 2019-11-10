#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniKinkoInheritance : DanmakusAbstract
{

    // 発射間隔
    [SerializeField]
    private float kinkoShotInterval;

    // 角速度
    [SerializeField]
    private float kinkoAngleSpeed;

    // 弾の速さ
    [SerializeField]
    private float kinkoBulletSpeed = 10;

    // 発射間隔
    [SerializeField]
    private float omniShotInterval;

    // way数
    [SerializeField]
    private int omniWay;

    // 弾の速さ
    [SerializeField]
    private float omniBulletSpeed = 10;


    protected void Awake()
    {
        CalcRealShotNum[] calcRealShotNum = { KinkoCalcRealShotNum , OmniCalcRealShotNum };
        CalcLaunchTime[] calcLaunchTime = { KinkoCalcLaunchTime, OmniCalcLaunchTime };
        ShotBullets[] shotBullets = { KinkoShotBullets, OmniShotBullets };

        base.Awake(calcRealShotNum, calcLaunchTime, shotBullets);
    }


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {
        // 角度を進める
        float rad = kinkoAngleSpeed * Time.time;
        rad = Modulo2PI(rad);

        // 角度を反映させる
        transform.eulerAngles = CalcEulerAngles(rad);
    }


    // 現在のあるべき発射回数を計算する(小数)
    protected float KinkoCalcRealShotNum()
    {
        return Time.time / kinkoShotInterval;
    }


    // 発射時刻を計算する
    protected float KinkoCalcLaunchTime(float realShotNum)
    {
        return kinkoShotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected void KinkoShotBullets(float launchTime, float dTime)
    {
        Vector3 pos = transform.position;

        float pastRad = kinkoAngleSpeed * launchTime;
        pastRad = Modulo2PI(pastRad);

        float distance = kinkoBulletSpeed * dTime;

        pos += new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad));

        Vector3 eulerAngles;

        eulerAngles = CalcEulerAngles(pastRad);

        // 弾を撃つ
        BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
        BulletController.ShotBullet(bulletShotParam);
    }


    // 現在のあるべき発射回数を計算する(小数)
    protected float OmniCalcRealShotNum()
    {
        return Time.time / omniShotInterval;
    }


    // 発射時刻を計算する
    protected float OmniCalcLaunchTime(float realShotNum)
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
}