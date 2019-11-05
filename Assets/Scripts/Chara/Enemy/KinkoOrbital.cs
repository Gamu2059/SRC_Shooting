using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinkoOrbital : BattleRealEnemyController
{
    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 角速度
    [SerializeField]
    private float m_AngleSpeed;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed;

    // 弾の総数
    [SerializeField]
    private int m_BulletNum;


    // 弾の配列
    [SerializeField]
    private BulletController[] m_bullet;

    // それぞれの弾の経過時間
    [SerializeField]
    private float[] m_bulletLifeTime;

    // それぞれの弾の大まかな進む角度
    [SerializeField]
    private float[] m_bulletAngle;

    // 次に発射する弾のインデックス
    [SerializeField]
    private int m_nextBulletIndex;


    // 実際の今までの発射回数
    [SerializeField]
    private int realShotNum;


    private void Start()
    {
        m_bullet         = new BulletController[m_BulletNum];
        m_bulletLifeTime = new            float[m_BulletNum];
        m_bulletAngle    = new            float[m_BulletNum];

        for(int i = 0;i < m_BulletNum; i++)
        {
            m_bullet[i] = BulletController.ShotBulletWithoutBulletParam(this,false);
            m_bulletLifeTime[i] = 0;
            m_bulletAngle[i] = 0;
        }
    }


    private void Update()
    {
        // 弾の状態を更新する
        for (int i = 0; i < m_BulletNum; i++)
        {
            // 弾の経過時間進める
            m_bulletLifeTime[i] += Time.deltaTime;

            // 位置
            Vector3 position = transform.position;
            position += m_BulletSpeed * m_bulletLifeTime[i] * new Vector3(Mathf.Cos(m_bulletAngle[i]), 0, Mathf.Sin(m_bulletAngle[i]));

            // 位置を代入
            m_bullet[i].SetPosition(position);
        }

        // 現在のあるべき発射回数
        int properShotNum = Mathf.FloorToInt(Time.time / m_ShotInterval);

        // 発射されるべき回数分、弾を発射する
        while (realShotNum < properShotNum)
        {
            // 発射する弾の番号にする
            realShotNum++;

            // 弾のインデックス
            //int index = realShotNum % m_BulletNum;

            // 弾の角度
            m_bulletAngle[m_nextBulletIndex] = m_AngleSpeed * m_ShotInterval * realShotNum;

            // 弾の経過時間
            m_bulletLifeTime[m_nextBulletIndex] = Time.time - m_ShotInterval* realShotNum;

            m_bullet[m_nextBulletIndex].SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);

            m_nextBulletIndex++;
            m_nextBulletIndex %= m_BulletNum;
        }
    }
}