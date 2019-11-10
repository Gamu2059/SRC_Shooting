#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jikinerai : DanmakusAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // way数
    [SerializeField]
    private int way;

    // 1wayごとの角度差
    [SerializeField]
    private float radAWay;

    // 弾の速さ
    [SerializeField]
    private float[] bulletSpeeds;


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

        //Vector3 relativeVector = PlayerCharaManager.Instance.GetCurrentController().transform.position - transform.position;
        Vector3 relativeVector = new Vector3(0,0,-10) - transform.position;
        relativeVector.Normalize();

        float relativeRad = Mathf.Atan2(relativeVector.z, relativeVector.x);

        Vector3 eulerAngles;

        // way数の分弾を撃つ
        for (int aWay = -way + 1; aWay < way; aWay++)
        {

            // このwayで撃つ角度
            float wayRad = relativeRad + radAWay * aWay;

            // 撃つ角度を決定する
            eulerAngles = CalcEulerAngles(wayRad);

            // 弾速の数だけ弾を撃つ
            for (int bulletSpeedIndex = 0; bulletSpeedIndex < bulletSpeeds.Length; bulletSpeedIndex++)
            {

                float distance = bulletSpeeds[bulletSpeedIndex] * dTime;

                // 発射された弾の位置
                Vector3 pos = transform.position;
                pos += distance * new Vector3(Mathf.Cos(wayRad),0, Mathf.Cos(wayRad));

                // 弾を撃つ
                BulletShotParam bulletShotParam = new BulletShotParam(this, 0, bulletSpeedIndex, 0, pos, eulerAngles, transform.localScale);
                BulletController.ShotBullet(bulletShotParam);
            }
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