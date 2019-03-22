using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射方向が等角速度で変化する弾幕
/// </summary>
public class OmegaDanmaku : AbstractBezierMove
{
    // 発射間隔
    [SerializeField, Tooltip("発射の時間間隔")]
    private float m_ShotInterval;

    // 角度の初期値
    [SerializeField, Tooltip("発射角度の初期値")]
    private float m_InitialAngle;

    // 角速度
    [SerializeField, Tooltip("角速度")]
    private float m_AngleSpeed;

    // 角度の大きさの半分
    [SerializeField, Tooltip("発射角度のブレの大きさの半分")]
    private float ampRad;

    // way数
    [SerializeField, Tooltip("way数")]
    private int m_Way;

    // 発射地点の円の半径
    [SerializeField, Tooltip("発射地点の円の半径")]
    private float m_CircleRadius;

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
        // 弾の位置を発射時の位置にする
        Vector3 pos = transform.position;

        // 発射時の角度
        float pastAngle = m_InitialAngle + m_AngleSpeed * launchTime;
        pastAngle = Modulo2PI(pastAngle);

        // 発射後の移動距離
        float distance = m_BulletSpeed * dTime;

        for (int i = 0; i < m_Way; i++)
        {

            // way数による角度のズレ
            float wayRad = Mathf.PI * 2 * i / m_Way;

            // 弾の発射角度のランダムなズレ
            float randomRad = Random.Range(-ampRad,ampRad);

            // 発射角度
            float Launchangle = pastAngle + wayRad + randomRad;

            // 位置のランダムなブレの成分
            Vector2 randomPosition = Random.insideUnitCircle * m_CircleRadius;

            // ランダムに位置をずらす
            pos += new Vector3(randomPosition.x, 0, randomPosition.y);

            // 弾を経過した時間だけ進ませる
            pos += new Vector3(distance * Mathf.Cos(Launchangle), 0, distance * Mathf.Sin(Launchangle));

            // 弾の角度
            Vector3 eulerAngles = CalcEulerAngles(Launchangle);

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
