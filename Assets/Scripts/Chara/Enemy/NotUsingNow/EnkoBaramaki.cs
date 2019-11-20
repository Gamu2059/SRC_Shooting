#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnkoBaramaki : BattleRealEnemyController
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

    // 中心の角度
    [SerializeField]
    private float centerRad;

    // 角度の大きさの半分
    [SerializeField]
    private float ampRad;


    protected override void Awake()
    {
        m_ShotTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

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

            // 弾の発射角度
            float pastRad = Random.Range((centerRad - ampRad) * Mathf.Deg2Rad, (centerRad + ampRad) * Mathf.Deg2Rad);

            // 発射された弾の位置
            Vector3 pos = new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad)) + m_BasePos;

            // 敵の向きを発射時のものにする
            Vector3 pastAngle = transform.eulerAngles;
            pastAngle.y = -(pastRad * Mathf.Rad2Deg) + 90;

            // 弾を撃つ
            //ShotBullet(0, 0, pos, pastAngle);

            ShotOrNot();
        }
    }
}