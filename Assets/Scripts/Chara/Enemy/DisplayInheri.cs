#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInheri : DanmakusAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

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

        float pastRad = angleSpeed * launchTime;
        pastRad = Modulo2PI(pastRad);

        float distance = bulletSpeed * dTime;

        for (int i = 0; i < way; i++)
        {
            // way数による角度のズレ
            float wayRad = pastRad + Mathf.PI * 2 * i / way;

            // 発射された弾の位置
            Vector3 pos = transform.position;
            pos += new Vector3(circleRadius * Mathf.Cos(pastRad - Mathf.PI / 2), 0, circleRadius * Mathf.Sin(pastRad - Mathf.PI / 2));
            pos += new Vector3(distance * Mathf.Cos(wayRad), 0, distance * Mathf.Sin(wayRad));

            Vector3 eulerAngles;

            eulerAngles = CalcEulerAngles(wayRad);

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