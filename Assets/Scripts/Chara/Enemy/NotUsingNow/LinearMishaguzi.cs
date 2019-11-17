#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMishaguzi : BattleRealEnemyController
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 次撃つまでの時間
    [SerializeField]
    private float m_ShotTime;

    // 現在の向いている角度
    [SerializeField]
    private float m_NowRad;

    // 位置
    [SerializeField]
    private Vector3 m_BasePos;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;

    // way数
    [SerializeField]
    private int way;

    // 発射地点までの距離
    [SerializeField]
    private float dist;

    protected override void Awake()
    {
        m_ShotTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        m_ShotTime -= Time.deltaTime;

        ShotOrNot();
    }

    private void ShotOrNot()
    {

        if (m_ShotTime < 0f)
        {

            // 現在は、本来撃たれた時刻からこれだけ経っている。
            float dTime = -m_ShotTime;
            m_ShotTime += m_ShotInterval;

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            // ランダムな角度
            float rad0 = Random.Range(0, Mathf.PI);

            for (int i = 0; i < way; i++)
            {
                // 1つの弾の角度
                float rad = rad0 + Mathf.PI * 2 * i / way;

                // 本来の発射地点
                Vector3 launchPos = m_BasePos + new Vector3(dist * Mathf.Cos(rad), 0, dist * Mathf.Sin(rad));

                // 発射された弾の現在の位置
                Vector3 pos = launchPos + new Vector3(distance * Mathf.Cos(rad + Mathf.PI / 2), 0, distance * Mathf.Sin(rad + Mathf.PI / 2));

                // その弾の発射角度
                Vector3 tempAngle = transform.eulerAngles;
                tempAngle.y = -((rad + Mathf.PI / 2) * Mathf.Rad2Deg) + 90;

                // 弾を撃つ
                //ShotBullet(0, 0, pos, tempAngle);
                // 弾を撃つ
                BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, tempAngle, transform.localScale);
                BulletController.ShotBullet(bulletShotParam);

                // 発射された弾の現在の位置
                pos = launchPos + new Vector3(distance * Mathf.Cos(rad - Mathf.PI / 2), 0, distance * Mathf.Sin(rad - Mathf.PI / 2));

                // その弾の発射角度
                tempAngle.y = -((rad - Mathf.PI / 2) * Mathf.Rad2Deg) + 90;

                // 弾を撃つ
                //ShotBullet(0, 0, pos, tempAngle);
                // 弾を撃つ
                bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, tempAngle, transform.localScale);
                BulletController.ShotBullet(bulletShotParam);
            }

            ShotOrNot();
        }
    }
}
