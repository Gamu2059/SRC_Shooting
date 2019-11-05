using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LissajousAlice : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // way数
    [SerializeField]
    private int way;

    // x軸方向の振幅
    [SerializeField]
    private float ampX;

    // x軸方向の各振動数
    [SerializeField]
    private float angFreqX;

    // y軸方向の振幅
    [SerializeField]
    private float ampY;

    // y軸方向の各振動数
    [SerializeField]
    private float angFreqY;

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


        // リサージュの中心から見た発射位置
        Vector3 relativeLaunchPos = new Vector3(ampX * Mathf.Sin(angFreqX * launchTime),0, ampY * Mathf.Sin(angFreqY * launchTime));

        // 発射後の移動距離
        float distance = bulletSpeed * dTime;

        // 発射角度
        float frontLaunchRad = Mathf.Atan2(ampY * angFreqY * Mathf.Cos(angFreqY * launchTime),ampX * angFreqX * Mathf.Cos(angFreqX * launchTime));

        for (int i = 0; i < way; i++)
        {
            // way数による角度のズレ
            float wayRad = frontLaunchRad + Mathf.PI * 2 * i / way;

            // 発射された弾の位置
            Vector3 pos = transform.position;
            pos += relativeLaunchPos;
            pos += new Vector3(distance * Mathf.Cos(wayRad), 0, distance * Mathf.Sin(wayRad));

            Vector3 eulerAngles;

            eulerAngles = CalcEulerAngles(wayRad);

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }
}