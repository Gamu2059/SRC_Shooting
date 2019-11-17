#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IeaouCross : BattleRealEnemyController
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

    // 発射地点の円の半径
    [SerializeField]
    private float circleRadius;

    // 位相
    [SerializeField]
    private float phase;

    // 位相速度
    [SerializeField]
    private float phaseSpeed;


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

        // 位相を進める
        phase += phaseSpeed * Time.deltaTime;
        phase %= Mathf.PI;

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

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            // 位置のランダムなブレの成分
            Vector2 randomPos = Random.insideUnitCircle * circleRadius;

            // 発射地点の領域を楕円にする
            if (pastPhase < Mathf.PI / 4 || Mathf.PI * 3 / 4 < pastPhase)
            {
                randomPos.x *= 1 - Mathf.Cos(pastPhase * 2);
            }
            else
            {
                randomPos.y *= -(1 - Mathf.Cos(pastPhase * 2));
            }

            // 発射時の弾の位置
            Vector3 pastPos = m_BasePos;
            pastPos += new Vector3(randomPos.x, 0, randomPos.y);

            // 敵の向き
            Vector3 pastAngle = transform.eulerAngles;

            //Vector3 pos;

            Shot( pastPhase           , distance, pastPos, pastAngle);
            Shot( pastPhase + Mathf.PI, distance, pastPos, pastAngle);
            Shot(-pastPhase + Mathf.PI, distance, pastPos, pastAngle);
            Shot(-pastPhase           , distance, pastPos, pastAngle);

            ShotOrNot();
        }
    }

    // 弾を撃つ
    private void Shot(float pastPhase,float distance, Vector3 pastPos, Vector3 pastAngle)
    {

        // 発射された弾の現在の位置
        Vector3 pos = pastPos + new Vector3(distance * Mathf.Cos(pastPhase), 0, distance * Mathf.Sin(pastPhase));

        // 敵の向きを発射時のものにする
        pastAngle.y = -(pastPhase * Mathf.Rad2Deg) + 90;

        // 弾を撃つ
        //ShotBullet(0, 0, pos, pastAngle);

    }
}