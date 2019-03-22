using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepingBullet : DanmakuAbstract
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

    // 連続発射帯の数(way数)
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

        // 発射時の角度
        float pastAngle = angleSpeed * launchTime;
        pastAngle = Modulo2PI(pastAngle);

        // 発射後の移動距離
        float distance = bulletSpeed * dTime;

        for (int shotRadsIndex = 0; shotRadsIndex < shotRads.Length; shotRadsIndex++)
        {

            // このwayの中心角度
            float centerWayRad = pastAngle + shotRads[shotRadsIndex];

            // このwayの発射角度
            float launchRad = Random.Range(centerWayRad - ampRad, centerWayRad + ampRad);

            // 弾の位置を敵本体の位置にする
            Vector3 pos = transform.position;

            // 弾源の位置に移動する
            pos += new Vector3(circleRadius * Mathf.Cos(pastAngle), 0, circleRadius * Mathf.Sin(pastAngle));

            // 弾を経過した時間だけ進ませる
            pos += new Vector3(distance * Mathf.Cos(launchRad), 0, distance * Mathf.Sin(launchRad));

            // 弾の角度
            Vector3 eulerAngles = CalcEulerAngles(launchRad);

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(this,0,0,0, pos, eulerAngles,transform.localScale);
            BulletController.ShotBullet(bulletShotParam);

        }
    }
}