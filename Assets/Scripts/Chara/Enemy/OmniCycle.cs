#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniCycle : BattleRealEnemyController
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
    private float distMax;

    // 位相
    [SerializeField]
    private float phase;

    // 位相速度
    [SerializeField]
    private float phaseSpeed;

    // 左右どちらなのか
    [SerializeField]
    private bool right;

    // Start is called before the first frame update
    private void Awake()
    {
        m_ShotTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // 位相を進める
        phase += phaseSpeed * Time.deltaTime;

        // 左右を入れ替えるかどうか
        if (phase >= Mathf.PI * 2)
        {
            right = !right;
            phase -= Mathf.PI * 2;
        }

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

            // 発射時の位相
            float pastPhase = phase - phaseSpeed * dTime;

            // 発射時の左右
            bool pastRight = right;

            // 発射時の左右を入れ替えるかどうか
            if (pastPhase < 0)
            {
                pastRight = !pastRight;
                pastPhase += Mathf.PI * 2;
            }

            // 発射時の円の半径
            float dist = distMax * (- Mathf.Cos(pastPhase) + 1) / 2;

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
                Vector3 pos = m_BasePos;

                // 発射位置が左右どちらかに移動する
                if (pastRight)
                {
                    pos += new Vector3(dist * Mathf.Cos(rad + Mathf.PI / 2), 0, dist * Mathf.Sin(rad + Mathf.PI / 2));
                }
                else
                {
                    pos += new Vector3(dist * Mathf.Cos(rad - Mathf.PI / 2), 0, dist * Mathf.Sin(rad - Mathf.PI / 2));
                }

                // 発射された弾の現在の位置
                pos += new Vector3(distance * Mathf.Cos(rad), 0, distance * Mathf.Sin(rad));

                // 弾を撃つ
                //ShotBullet(0, 0, pos, tempAngle);
            }

            ShotOrNot();
        }
    }
}
