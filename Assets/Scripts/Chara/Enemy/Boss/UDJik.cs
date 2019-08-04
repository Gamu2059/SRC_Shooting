using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDJik : DanmmakuAbstractObject
{

    [SerializeField, Tooltip("発射間隔")]
    private float m_ShotInterval;

    [SerializeField, Tooltip("way数")]
    private int m_Way;

    [SerializeField, Tooltip("1wayごとの角度差")]
    private float m_RadAWay;

    [SerializeField, Tooltip("弾速")]
    private float[] m_BulletSpeeds;


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


        //Vector3 relativeVector = PlayerCharaManager.Instance.GetCurrentController().transform.position - transform.position;
        Vector3 relativeVector = new Vector3(0, 0, -10) - enemyController.transform.position;
        relativeVector.Normalize();

        float relativeRad = Mathf.Atan2(relativeVector.z, relativeVector.x);

        Vector3 eulerAngles;

        // way数の分弾を撃つ
        for (int aWay = -m_Way + 1; aWay < m_Way; aWay++)
        {

            // このwayで撃つ角度
            float wayRad = relativeRad + m_RadAWay * aWay;

            // 撃つ角度を決定する
            eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles, wayRad);

            // 弾速の数だけ弾を撃つ
            for (int bulletSpeedIndex = 0; bulletSpeedIndex < m_BulletSpeeds.Length; bulletSpeedIndex++)
            {

                float distance = m_BulletSpeeds[bulletSpeedIndex] * dTime;

                // 発射された弾の位置
                Vector3 pos = enemyController.transform.position;
                pos += distance * new Vector3(Mathf.Cos(wayRad), 0, Mathf.Cos(wayRad));

                // 弾を撃つ
                BulletShotParam bulletShotParam = new BulletShotParam(enemyController, 0, bulletSpeedIndex, 0, pos, eulerAngles, enemyController.transform.localScale);
                BulletController.ShotBullet(bulletShotParam);
            }
        }
    }
}
