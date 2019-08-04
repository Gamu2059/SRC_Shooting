using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialOrbitalAbstract : EnemyController
{

    // 実際の今までの発射回数
    [SerializeField]
    protected int m_RealShotNum;

    // 実際の今までの発射回数
    [SerializeField]
    protected int m_BulletNum;

    // 弾
    [SerializeField]
    protected BulletController[] m_Bullet;

    // 弾の発射されてからの経過時間
    [SerializeField]
    protected float[] m_BulletLifeTime;

    // 弾の最大生存時間
    [SerializeField]
    protected float m_BulletMaxLifeTime;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        // 弾とその経過時間の配列
        m_Bullet = new BulletController[m_BulletNum];
        m_BulletLifeTime = new float[m_BulletNum];

        // 弾とその経過時間の配列に代入する
        for (int i = 0; i < m_BulletNum; i++)
        {
            m_Bullet[i] = BulletController.ShotBulletWithoutBulletParam(this, true);
            m_BulletLifeTime[i] = 0;
        }
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        // 弾の発射からの経過時間を進める
        for (int i = 0; i < m_BulletNum; i++)
        {
            if (m_BulletLifeTime[i] <= m_BulletMaxLifeTime)
            {
                m_BulletLifeTime[i] += Time.deltaTime;

                // 弾の位置
                Vector3 position = Vector3.zero;

                // 位置を代入
                position = SetPosition(i);

                m_Bullet[i].SetPosition(position);

                if (m_BulletMaxLifeTime < m_BulletLifeTime[i])
                {
                    m_Bullet[i].DestroyBullet();
                }
            }
        }


        // 理想的な発射回数
        float properShotNum = Mathf.FloorToInt(CalcNowShotNum());

        // 発射されていたか
        if (m_RealShotNum < properShotNum)
        {

            // 実際の今までの発射回数を1増やす
            m_RealShotNum++;

            // 弾のインデックス
            int index = m_RealShotNum % m_BulletNum;

            // 弾を生成する
            m_Bullet[index] = BulletController.ShotBulletWithoutBulletParam(this, true);

            // 弾の発射からの経過時間を新しくする
            m_BulletLifeTime[index] = Time.time - CalcLaunchTime();

            ShotBullets(index, CalcLaunchTime(), Time.time - CalcLaunchTime());
        }
    }


    // 弾の位置を決める
    protected abstract Vector3 SetPosition(int i);

    // 現在のあるべき発射回数を計算する(小数)
    protected abstract float CalcNowShotNum();

    // 発射時刻を計算する
    protected abstract float CalcLaunchTime();

    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected abstract void ShotBullets(int index,float launchTime, float dTime);
}