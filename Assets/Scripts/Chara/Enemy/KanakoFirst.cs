#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanakoFirst : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 発射位置
    [SerializeField]
    private Vector3 relativeLaunchPos;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected override float CalcNowShotNum()
    {
        return Time.time / m_ShotInterval;
    }


    // 発射時刻を計算する
    protected override float CalcLaunchTime()
    {
        return m_ShotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected override void ShotBullets(float launchTime, float dTime)
    {

        // 発射後の移動距離
        float distance = m_BulletSpeed * dTime;

        // 弾の発射角度
        float launchRad = Random.Range(0, Mathf.PI * 2);

        // 弾の位置を敵本体の位置にする
        Vector3 pos = transform.position;

        // 弾の位置を発射時の位置にする
        pos += relativeLaunchPos;

        // 弾を経過した時間だけ進ませる
        pos += new Vector3(distance * Mathf.Cos(launchRad), 0, distance * Mathf.Sin(launchRad));

        // 弾の角度
        Vector3 eulerAngles = CalcEulerAngles(launchRad);

        // 弾を撃つ
        BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
        BulletController.ShotBullet(bulletShotParam);
    }
}