#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kakukasoku : BattleRealEnemyController
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

    // 弾の加速度の大きさ
    [SerializeField]
    private float m_BulletAccelaration;

    // 位置
    [SerializeField]
    private Vector3 m_BasePos;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;


    // Start is called before the first frame update
    private void Awake()
    {
        m_ShotTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 角度を進める
        m_AngleSpeed += m_BulletAccelaration * Time.deltaTime;
        m_NowRad += m_AngleSpeed;
        m_NowRad %= Mathf.PI * 2;

        // 角度を反映させる
        Vector3 angle = transform.eulerAngles;
        angle.y = -(m_NowRad * Mathf.Rad2Deg) + 90;
        transform.eulerAngles = angle;

        m_ShotTime -= Time.deltaTime;

        if (m_ShotTime < 0f)
        {
            // 現在は、本来撃たれた時刻からこれだけ経っている。
            float dTime = -m_ShotTime;
            m_ShotTime += m_ShotInterval;

            // 発射時の角速度
            float pastSpeed = m_AngleSpeed - m_BulletAccelaration * dTime;

            // 発射時から現在への角度差
            //float dRad = (m_AngleSpeed * m_AngleSpeed - pastSpeed * pastSpeed) / (m_BulletAccelaration * 2);
            //float dRad = pastSpeed * dTime + m_BulletAccelaration * dTime * dTime / 2;
            float dRad = m_AngleSpeed * dTime - m_BulletAccelaration * dTime * dTime / 2;

            // 発射時の角度
            float pastRad = m_NowRad - dRad;

            // 弾が撃たれてから弾が進んだ距離
            float distance = m_BulletSpeed * dTime;

            // 現在の敵の位置
            Vector3 nowPos = new Vector3(m_BasePos.x, m_BasePos.y, m_BasePos.z);

            // 現在の弾の位置に敵を移動させる
            Vector3 pos = new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad)) + m_BasePos;
            transform.position = pos;

            // 現在の敵の向き
            Vector3 nowAngle = transform.eulerAngles;

            // 敵の向きを発射時のものにする
            Vector3 pastAngle = transform.eulerAngles;
            pastAngle.y = -(pastRad * Mathf.Rad2Deg) + 90;
            transform.eulerAngles = pastAngle;

            // 弾を撃つ
            //ShotBullet(0, 0);

            // 敵の位置を現在のものに戻す
            transform.position = nowPos;

            // 敵の向きを現在のものに戻す
            transform.eulerAngles = nowAngle;
        }
    }
}