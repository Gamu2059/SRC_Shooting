#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinkoOmniDir : BattleRealEnemyBase
{

    // 全方位弾の発射間隔
    [SerializeField]
    private float m_OmniShotInterval;

    // 均衡の発射間隔
    [SerializeField]
    private float m_KinkoShotInterval;

    // 全方位弾の次撃つまでの時間
    [SerializeField]
    private float m_OmniShotTime;

    // 均衡の次撃つまでの時間
    [SerializeField]
    private float m_KinkoShotTime;

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

    // 角速度
    [SerializeField]
    private float m_AngleSpeed;

    protected override void Awake()
    {
        m_OmniShotTime = 0;
        m_KinkoShotTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // 角度を進める
        m_NowRad += m_AngleSpeed * Time.deltaTime;
        m_NowRad %= Mathf.PI * 2;

        // 角度を反映させる
        Vector3 angle = transform.eulerAngles;
        angle.y = -(m_NowRad * Mathf.Rad2Deg) + 90;
        transform.eulerAngles = angle;

        m_OmniShotTime -= Time.deltaTime;
        m_KinkoShotTime -= Time.deltaTime;

        OmniShotOrNot();
        KinkoShotOrNot();
    }

    private void OmniShotOrNot()
    {

        if (m_OmniShotTime < 0f)
        {

            // 現在は、本来撃たれた時刻からこれだけ経っている。
            float dTime = -m_OmniShotTime;
            m_OmniShotTime += m_OmniShotInterval;

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            // ランダムな角度
            float rad0 = Random.Range(0, Mathf.PI);

            for (int i = 0; i < way; i++)
            {
                // 1つの弾の角度
                float rad = rad0 + Mathf.PI * 2 * i / way;

                // その弾の発射角度
                Vector3 tempAngle = transform.eulerAngles;
                tempAngle.y = -(rad * Mathf.Rad2Deg) + 90;

                // 発射された弾の現在の位置
                Vector3 OmniPos = new Vector3(distance * Mathf.Cos(rad), 0, distance * Mathf.Sin(rad)) + m_BasePos;

                // 弾を撃つ
                //ShotBullet(0, 0, OmniPos, tempAngle);
            }

            OmniShotOrNot();
        }
    }

    private void KinkoShotOrNot()
    {

        if (m_KinkoShotTime < 0f)
        {

            // 現在は、本来撃たれた時刻からこれだけ経っている。
            float dTime = -m_KinkoShotTime;
            m_KinkoShotTime += m_KinkoShotInterval;

            // 本来撃たれた時刻での角度
            float pastRad = m_NowRad - m_AngleSpeed * dTime;

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            // 発射された弾の位置
            Vector3 kinkoPos = new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad)) + m_BasePos;

            // 敵の向きを発射時のものにする
            Vector3 pastAngle = transform.eulerAngles;
            pastAngle.y = -(pastRad * Mathf.Rad2Deg) + 90;

            // 弾を撃つ
            //ShotBullet(0, 0, kinkoPos, pastAngle);

            KinkoShotOrNot();
        }
    }
}
