using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全方位にランダムに弾を出す弾幕
/// </summary>
public class OmniDirectionalDanmaku : AbstractBezierMove
{
    // 発射間隔
    [SerializeField, Tooltip("発射の時間間隔")]
    private float m_ShotInterval;

    // way数
    [SerializeField, Tooltip("way数")]
    private int m_Way;

    // 弾の速さ
    [SerializeField, Tooltip("弾の速さ")]
    private float m_BulletSpeed;


    // 現在のあるべき発射回数を計算する(小数)
    protected float ShotNum()
    {
        return m_Time / m_ShotInterval;
    }


    // 発射時刻を計算する
    protected float LaunchTime(float realShotNum)
    {
        return m_ShotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected void ShotBullet(float launchTime, float dTime)
    {
        float distance = m_BulletSpeed * dTime;

        // ランダムな角度
        float rad0 = Random.Range(0, Mathf.PI * 2);

        for (int i = 0; i < m_Way; i++)
        {
            // 1つの弾の角度
            float rad = rad0 + Mathf.PI * 2 * i / m_Way;

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


    protected override ProperShotNumDelegate[] GetProperShotNumDelegate()
    {
        ProperShotNumDelegate[] calcRealShotNums = { ShotNum };

        return calcRealShotNums;
    }


    protected override LaunchTimeDelegate[] GetLaunchTimeDelegate()
    {
        LaunchTimeDelegate[] calcLaunchTimes = { LaunchTime };

        return calcLaunchTimes;
    }


    protected override ShotBulletsDelegate[] GetShotBulletsDelegate()
    {
        ShotBulletsDelegate[] shotBullets = { ShotBullet };

        return shotBullets;
    }
}
