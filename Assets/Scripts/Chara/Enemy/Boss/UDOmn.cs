using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 一定の時間間隔で発射する、角度ランダムの全方位弾
[System.Serializable]
public class UDOmn : DanmmakuAbstractObject
{

    [SerializeField, Tooltip("発射位置")]
    private Vector3 m_LaunchPosition;

    [SerializeField, Tooltip("発射間隔")]
    private float m_ShotInterval;

    [SerializeField, Tooltip("way数")]
    private int m_Way;

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
    public override void ShotBullets(EnemyController enemyController,float launchTime, float dTime)
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
            eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles,rad);

            // 発射された弾の現在の位置
            Vector3 pos = enemyController.transform.position;
            pos += m_LaunchPosition;
            pos += new Vector3(distance * Mathf.Cos(rad), 0, distance * Mathf.Sin(rad));

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(enemyController, m_BulletIndex[0], m_BulletParamIndex[0], 0, pos, eulerAngles, enemyController.transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }
}
