#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceTsujo : BattleRealEnemyController
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

    // 角速度
    [SerializeField]
    private float m_AngleSpeed;

    // 位置
    [SerializeField]
    private Vector3 m_BasePos;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;

    // 発射地点の円の半径
    [SerializeField]
    private float circleRadius;

    // way数
    [SerializeField]
    private int way;


    // Start is called before the first frame update
    private void Awake()
    {
        m_ShotTime = 0;
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

            // 本来撃たれた時刻での角度
            float pastRad = m_NowRad - m_AngleSpeed * dTime;

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            for (int i = 0; i < way; i++)
            {

                // way数による角度のズレ
                float wayRad = pastRad + Mathf.PI * 2 * i / way;

                // 発射された弾の位置
                Vector3 pos = m_BasePos;
                pos += new Vector3(circleRadius * Mathf.Cos(pastRad - Mathf.PI / 2), 0, circleRadius * Mathf.Sin(pastRad - Mathf.PI / 2));
                pos += new Vector3(distance * Mathf.Cos(wayRad), 0, distance * Mathf.Sin(wayRad));

                // 敵の向きを発射時のものにする
                Vector3 pastAngle = transform.eulerAngles;
                pastAngle.y = -(wayRad * Mathf.Rad2Deg) + 90;

                // 弾を撃つ
                //ShotBullet(0, 0, pos, pastAngle);

            }

            ShotOrNot();
        }
    }
}