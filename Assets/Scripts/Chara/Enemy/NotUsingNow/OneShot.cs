﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShot : BattleRealEnemyBase
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 次撃つまでの時間
    [SerializeField]
    private float m_ShotTime;

    // 位置
    [SerializeField]
    private Vector3 m_BasePos;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;

    // 現在の向いている角度
    [SerializeField]
    private float m_NowRad;

    // 本体の速度
    [SerializeField]
    private Vector3 m_BaseVelocity;

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

        // 位置を反映させる
        m_BasePos += m_BaseVelocity * Time.deltaTime;
        transform.position = m_BasePos;

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

            // 発射時の敵の位置
            Vector3 pastPos = m_BasePos - m_BaseVelocity * dTime;

            // 発射された弾の位置
            Vector3 pos = new Vector3(distance * Mathf.Cos(m_NowRad), 0, distance * Mathf.Sin(m_NowRad)) + pastPos;

            // 弾を撃つ
            //ShotBullet(0, 0, pos, transform.eulerAngles);

            ShotOrNot();
        }
    }
}