using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinkoOrbital : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 角速度
    [SerializeField]
    private float m_AngleSpeed;

    // 発射位置
    [SerializeField]
    private Vector3 m_RelativeLaunchPos;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {
        // 角度を進める
        float rad = m_AngleSpeed * Time.time;
        rad = Modulo2PI(rad);

        // 角度を反映させる
        transform.eulerAngles = CalcEulerAngles(rad);
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
        // 弾の位置を発射時の位置にする
        Vector3 pos = transform.position;

        // 発射時の角度
        float pastRad = m_AngleSpeed * m_ShotInterval * realShotNum;
        pastRad = Modulo2PI(pastRad);

        // 発射後の移動距離
        float distance = m_BulletSpeed * dTime;

        // 弾の位置を発射時の位置にする
        pos += m_RelativeLaunchPos;

        // 弾を経過した時間だけ進ませる
        pos += new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad));

        // 弾の角度
        Vector3 eulerAngles = CalcEulerAngles(pastRad);

        // 弾を撃つ
        BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
        BulletController.ShotBullet(bulletShotParam);
    }
}