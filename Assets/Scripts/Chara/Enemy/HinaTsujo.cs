using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinaTsujo : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 角速度
    [SerializeField]
    private float m_AngleSpeed;

    // way数
    [SerializeField]
    private int way;

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
        // 弾の位置
        Vector3 pos;

        for (int i = 0; i < way; i++)
        {

            // 弾の位置を発射時の位置にする
            pos = transform.position;

            // 発射時の角度
            float pastRad = m_AngleSpeed * m_ShotInterval * realShotNum;
            pastRad = Modulo2PI(pastRad);

            // 発射後の移動距離
            float distance = m_BulletSpeed * dTime;

            // 弾を経過した時間だけ進ませる
            pos += new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad));

            // 弾の角度
            Vector3 eulerAngles = CalcEulerAngles(pastRad);

            // 弾を撃つ
            //ShotBullet(0, 0, pos, eulerAngles);
        }
    }
}