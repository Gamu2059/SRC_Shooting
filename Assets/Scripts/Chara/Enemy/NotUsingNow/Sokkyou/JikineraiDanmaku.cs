#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機を狙って弾を出す弾幕
/// </summary>
public class JikineraiDanmaku : AbstractBezierMove
{
    
    [SerializeField, Tooltip("発射の時間間隔")]
    private float m_ShotInterval;

    // way数
    [SerializeField, Tooltip("way数")]
    private int m_Way;

    // 1wayごとの角度差
    [SerializeField, Tooltip("1wayごとの角度差")]
    private float radAWay;

    // 弾の速さ
    [SerializeField, Tooltip("弾の速さの配列(BulletParamと対応)")]
    private float[] m_BulletSpeed;


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

        // 敵本体から見た自機の位置
        var player = BattleRealPlayerManager.Instance.Player;
        Vector3 relativeVector = player.transform.position - transform.position;

        // 敵本体から見た自機の角度
        float relativeRad = Mathf.Atan2(relativeVector.z, relativeVector.x);

        Vector3 eulerAngles;

        // way数の分弾を撃つ
        for (int aWay = -m_Way + 1; aWay < m_Way; aWay++)
        {

            // このwayで撃つ角度
            float wayRad = relativeRad + radAWay * aWay;

            // 撃つ角度を決定する
            eulerAngles = CalcEulerAngles(wayRad);

            // 弾速の数だけ弾を撃つ
            for (int bulletSpeedIndex = 0; bulletSpeedIndex < m_BulletSpeed.Length; bulletSpeedIndex++)
            {

                float distance = m_BulletSpeed[bulletSpeedIndex] * dTime;

                // 発射された弾の位置
                Vector3 pos = transform.position;
                pos += distance * new Vector3(Mathf.Cos(wayRad), 0, Mathf.Cos(wayRad));

                // 弾を撃つ
                BulletShotParam bulletShotParam = new BulletShotParam(this, 0, bulletSpeedIndex, 0, pos, eulerAngles, transform.localScale);
                BulletController.ShotBullet(bulletShotParam);
            }
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
