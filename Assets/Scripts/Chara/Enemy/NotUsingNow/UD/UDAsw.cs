using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDAsw : DanmakuCountAbstract
{

    [SerializeField, Tooltip("発射間隔")]
    private float m_ShotInterval;

    [SerializeField, Tooltip("way数")]
    private int m_Way;

    [SerializeField, Tooltip("角速度")]
    private float m_AngleSpeed;

    [SerializeField, Tooltip("弾源円半径の位相速度")]
    private float m_CircleRadiusPhaseSpeed;

    [SerializeField, Tooltip("発射地点の円の半径の最大値")]
    private float m_CircleRadiusMax;

    [SerializeField, Tooltip("弾速")]
    private float m_BulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    //public override void UpdateSelf()
    //{

    //}


    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_ShotInterval;
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_ShotInterval * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(EnemyController enemyController, float launchTime, float dTime)
    {

        float pastRad = m_AngleSpeed * launchTime;
        pastRad = Modulo2PI(pastRad);

        float distance = m_BulletSpeed * dTime;

        float circleRadius = m_CircleRadiusMax * (1 - Mathf.Cos(m_CircleRadiusPhaseSpeed * launchTime)) / 2;

        for (int i = 0; i < m_Way; i++)
        {
            // way数による角度のズレ
            float wayRad = pastRad + Mathf.PI * 2 * i / m_Way;

            // 発射された弾の位置
            Vector3 pos = enemyController.transform.position;
            pos += new Vector3(circleRadius * Mathf.Cos(pastRad - Mathf.PI / 2), 0, circleRadius * Mathf.Sin(pastRad - Mathf.PI / 2));
            pos += new Vector3(distance * Mathf.Cos(wayRad), 0, distance * Mathf.Sin(wayRad));

            Vector3 eulerAngles;

            eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles, wayRad);

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(enemyController, m_BulletIndex[0], m_BulletParamIndex[0], 0, pos, eulerAngles, enemyController.transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }
}
