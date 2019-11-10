#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalWave : BattleRealEnemyController
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

    // 発射地点の円の半径
    [SerializeField]
    private float circleRadius;

    // 角速度
    [SerializeField]
    private float m_AngleSpeed;

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

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            // 本来撃たれた時刻での角度
            float pastRad = m_NowRad - m_AngleSpeed * dTime;

            for (int i = 0; i < way; i++)
            {
                // この弾の速度の向き
                float wayRad = Mathf.PI * 2 * i / way;

                // その弾の発射角度
                Vector3 tempAngle = transform.eulerAngles;
                tempAngle.y = -(wayRad * Mathf.Rad2Deg) + 90;

                // 発射された弾の現在の位置
                Vector3 pos = m_BasePos;
                pos += new Vector3(circleRadius * Mathf.Cos(pastRad), 0, circleRadius * Mathf.Sin(pastRad));
                pos += new Vector3(distance * Mathf.Cos(wayRad), 0, distance * Mathf.Sin(wayRad));

                // 弾を撃つ
                //ShotBullet(0, 0, pos, tempAngle);
            }

            ShotOrNot();
        }
    }
}
