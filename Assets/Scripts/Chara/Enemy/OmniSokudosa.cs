#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniSokudosa : DanmakusAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // way数
    [SerializeField]
    private int way;

    // 弾の速さ
    [SerializeField]
    private float bulletSpeed = 10;


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

        float distance = bulletSpeed * dTime;

        // ランダムな角度
        float rad0 = Random.Range(0, Mathf.PI * 2);

        for (int i = 0; i < way; i++)
        {
            // 1つの弾の角度
            float rad = rad0 + Mathf.PI * 2 * i / way;

            // その弾の発射角度
            Vector3 eulerAngles;
            eulerAngles = CalcEulerAngles(rad);

            // 発射された弾の現在の位置
            Vector3 pos = transform.position;
            pos += new Vector3(distance * Mathf.Cos(rad), 0, distance * Mathf.Sin(rad));

            // 弾を撃つ
            //ShotBullet(0, 0, pos, eulerAngles);
        }
    }


    protected override void Awake()
    {
        CalcRealShotNumDelegate[] calcRealShotNum = { ShotNum };
        CalcLaunchTimeDelegate[] calcLaunchTime = { LaunchTime };
        ShotBulletsDelegate[] shotBullets = { ShotBullets };

        base.Awake(calcRealShotNum, calcLaunchTime, shotBullets);
    }
}