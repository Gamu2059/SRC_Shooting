using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDAswShort : AbstractUD
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // way数
    [SerializeField]
    private int way;

    // 角速度
    [SerializeField]
    private float angleSpeed;

    // 弾源円半径の位相速度
    [SerializeField]
    private float circleRadiusPhaseSpeed;

    // 発射地点の円の半径の最大値
    [SerializeField]
    private float circleRadiusMax;

    // 弾の速さ
    [SerializeField]
    private float bulletSpeed = 10;


    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum()
    {
        return m_Time / shotInterval;
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return shotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(EnemyController enemyController, float launchTime, float dTime)
    {

        float pastRad = angleSpeed * launchTime;
        pastRad = Modulo2PI(pastRad);

        float distance = bulletSpeed * dTime;

        float circleRadius = circleRadiusMax * (1 - Mathf.Cos(circleRadiusPhaseSpeed * launchTime)) / 2;

        for (int i = 0; i < way; i++)
        {
            // way数による角度のズレ
            float wayRad = pastRad + Mathf.PI * 2 * i / way;

            // 発射された弾の位置
            Vector3 pos = enemyController.transform.position;
            pos += new Vector3(circleRadius * Mathf.Cos(pastRad - Mathf.PI / 2), 0, circleRadius * Mathf.Sin(pastRad - Mathf.PI / 2));
            pos += new Vector3(distance * Mathf.Cos(wayRad), 0, distance * Mathf.Sin(wayRad));

            Vector3 eulerAngles;

            eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles,wayRad);

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(enemyController, 0, 0, 0, pos, eulerAngles, enemyController.transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }
}
