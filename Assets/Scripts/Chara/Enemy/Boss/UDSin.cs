using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDSin : DanmmakuAbstractObject
{

    [SerializeField, Tooltip("発射間隔")]
    private float SineshotInterval;

    [SerializeField, Tooltip("way数")]
    private int Sineway;

    [SerializeField, Tooltip("発射地点の円の半径")]
    private float circleRadius;

    [SerializeField, Tooltip("角速度")]
    private float angleSpeed;

    [SerializeField, Tooltip("弾速")]
    private float SinebulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    //public override void UpdateSelf()
    //{

    //}


    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / SineshotInterval;
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return SineshotInterval * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(EnemyController enemyController, float launchTime, float dTime)
    {

        float pastRad = angleSpeed * launchTime;
        pastRad = Modulo2PI(pastRad);

        float distance = SinebulletSpeed * dTime;

        for (int i = 0; i < Sineway; i++)
        {
            // way数による角度
            float wayRad = Mathf.PI * i / Sineway + Mathf.PI / Sineway / 2;

            // 正弦波のどの位置か(1次元に投影した時の位置)
            float phase = Mathf.Cos(pastRad - wayRad);

            Vector3 pos = enemyController.transform.position;

            pos += new Vector3(circleRadius * phase * Mathf.Cos(wayRad), 0, circleRadius * phase * Mathf.Sin(wayRad));
            pos += new Vector3(distance * Mathf.Cos(wayRad + Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad + Mathf.PI / 2));

            Vector3 eulerAngles;

            eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles,wayRad + Mathf.PI / 2);

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(enemyController, m_BulletIndex[0], m_BulletParamIndex[0], 0, pos, eulerAngles, enemyController.transform.localScale);
            BulletController.ShotBullet(bulletShotParam);

            pos = enemyController.transform.position;
            pos += new Vector3(circleRadius * phase * Mathf.Cos(wayRad), 0, circleRadius * phase * Mathf.Sin(wayRad));
            pos += new Vector3(distance * Mathf.Cos(wayRad - Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad - Mathf.PI / 2));

            eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles,wayRad - Mathf.PI / 2);

            // 弾を撃つ
            bulletShotParam = new BulletShotParam(enemyController, m_BulletIndex[0], m_BulletParamIndex[0], 0, pos, eulerAngles, enemyController.transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }
}
