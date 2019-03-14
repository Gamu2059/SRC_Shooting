using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineCurve : EnemyController
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
                // way数による角度
                float wayRad = Mathf.PI * i / way + Mathf.PI / way / 2;

                // 正弦波のどの位置か(1次元に投影した時の位置)
                float phase = Mathf.Cos(pastRad - wayRad);

                // 発射された弾の現在の位置
                Vector3 pos = m_BasePos;
                pos += new Vector3(circleRadius * phase * Mathf.Cos(wayRad), 0, circleRadius * phase * Mathf.Sin(wayRad));
                pos += new Vector3(distance * Mathf.Cos(wayRad + Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad + Mathf.PI / 2));

                // その弾の発射角度
                Vector3 tempAngle = transform.eulerAngles;
                tempAngle.y = -((wayRad + Mathf.PI / 2) * Mathf.Rad2Deg) + 90;

                // 弾を撃つ
                //ShotBullet(0, 0, pos, tempAngle);

                // 発射された弾の現在の位置
                pos = m_BasePos;
                pos += new Vector3(circleRadius * phase * Mathf.Cos(wayRad), 0, circleRadius * phase * Mathf.Sin(wayRad));
                pos += new Vector3(distance * Mathf.Cos(wayRad - Mathf.PI / 2), 0, distance * Mathf.Sin(wayRad - Mathf.PI / 2));

                // その弾の発射角度
                tempAngle = transform.eulerAngles;
                tempAngle.y = -((wayRad - Mathf.PI / 2) * Mathf.Rad2Deg) + 90;

                // 弾を撃つ
                //ShotBullet(0, 0, pos, tempAngle);
            }

            ShotOrNot();
        }
    }
}
