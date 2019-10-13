using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDLis : DanmakuCountAbstract
{

    [SerializeField, Tooltip("発射間隔")]
    public float m_ShotInterval;

    [SerializeField, Tooltip("way数")]
    public int m_Way;

    [SerializeField, Tooltip("x軸方向の振幅")]
    public float m_AmpX;

    [SerializeField, Tooltip("x軸方向の角振動数")]
    public float m_AngFreqX;

    [SerializeField, Tooltip("y軸方向の振幅")]
    public float m_AmpY;

    [SerializeField, Tooltip("y軸方向の角振動数")]
    public float m_AngFreqY;

    [SerializeField, Tooltip("弾速")]
    public float m_BulletSpeed = 10;


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
    public override void ShotBullets(BattleRealEnemyController enemyController,float launchTime, float dTime)
    {


        // リサージュの中心から見た発射位置
        Vector3 relativeLaunchPos = new Vector3(m_AmpX * Mathf.Sin(m_AngFreqX * launchTime), 0, m_AmpY * Mathf.Sin(m_AngFreqY * launchTime));

        // 発射後の移動距離
        float distance = m_BulletSpeed * dTime;

        // 発射角度
        float frontLaunchRad = Mathf.Atan2(m_AmpY * m_AngFreqY * Mathf.Cos(m_AngFreqY * launchTime), m_AmpX * m_AngFreqX * Mathf.Cos(m_AngFreqX * launchTime));

        for (int i = 0; i < m_Way; i++)
        {
            // way数による角度のズレ
            float wayRad = frontLaunchRad + Mathf.PI * 2 * i / m_Way;

            // 発射された弾の位置
            Vector3 pos = enemyController.transform.position;
            pos += relativeLaunchPos;
            pos += new Vector3(distance * Mathf.Cos(wayRad), 0, distance * Mathf.Sin(wayRad));

            Vector3 eulerAngles;

            eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles, wayRad);

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(enemyController, m_BulletIndex[0], m_BulletParamIndex[0], 0, pos, eulerAngles, enemyController.transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }
}
