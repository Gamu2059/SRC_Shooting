using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinkoOrbitalTest : EnemyController
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

    // 実際の今までの発射回数
    [SerializeField]
    private int m_RealShotNum;

    // 実際の今までの発射回数
    [SerializeField]
    private int m_BulletNum;

    // 弾
    [SerializeField]
    private BulletController[] m_Bullet;

    // 弾の発射されてからの経過時間
    [SerializeField]
    private float[] m_BulletLifeTime;

    // 弾の角度
    [SerializeField]
    private float[] m_BulletAngle;

    // 弾の最大生存時間
    [SerializeField]
    private float m_BulletMaxLifeTime;


    // Start is called before the first frame update
    void Start()
    {
        // 弾とその経過時間の配列
        m_Bullet = new BulletController[m_BulletNum];
        m_BulletLifeTime = new float[m_BulletNum];
        m_BulletAngle = new float[m_BulletNum];

        for (int i = 0; i < m_BulletNum; i++)
        {
            m_Bullet[i] = BulletController.ShotBulletWithoutBulletParam(this, true);
            m_BulletLifeTime[i] = 0;
            m_BulletAngle[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
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
        float properShotNum = Mathf.FloorToInt(Time.time / m_ShotInterval);

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
            m_BulletLifeTime[index] = Time.time - m_ShotInterval * m_RealShotNum;

            // 弾の発射角度
            m_BulletAngle[index] = m_AngleSpeed * m_ShotInterval * m_RealShotNum;
        }
    }


    // 弾の位置を決める
    private Vector3 SetPosition(int i)
    {
        return m_BulletSpeed * m_BulletLifeTime[i] *
            new Vector3(Mathf.Cos(m_BulletAngle[i] - m_BulletLifeTime[i] * 0.5f), 0, Mathf.Sin(m_BulletAngle[i] - m_BulletLifeTime[i] * 0.5f));

        //if (m_BulletLifeTime[i] < 0.4f)
        //{
        //    return 20 * m_BulletLifeTime[i] * new Vector3(Mathf.Cos(m_BulletAngle[i]), 0, Mathf.Sin(m_BulletAngle[i]));
        //}
        //else
        //{
        //    return ((20 - 8) * 0.4f + 8 * m_BulletLifeTime[i]) * new Vector3(Mathf.Cos(m_BulletAngle[i]), 0, Mathf.Sin(m_BulletAngle[i]));
        //}
    }
}


//// 弾1
//[SerializeField]
//private BulletController m_Bullet1;

//// 弾2
//[SerializeField]
//private BulletController m_Bullet2;

//// 弾3
//[SerializeField]
//private BulletController m_Bullet3;

//// 弾1の発射されてからの経過時間
//[SerializeField]
//private float m_BulletLifeTime1;

//// 弾2の発射されてからの経過時間
//[SerializeField]
//private float m_BulletLifeTime2;

//// 弾3の発射されてからの経過時間
//[SerializeField]
//private float m_BulletLifeTime3;


//// 弾0を生成
//m_Bullet1 = BulletController.ShotBulletWithoutBulletParam(this, true);

//// 弾1を生成
//m_Bullet2 = BulletController.ShotBulletWithoutBulletParam(this, true);

//// 弾3を生成
//m_Bullet3 = BulletController.ShotBulletWithoutBulletParam(this, true);

// 弾0の位置を設定
//m_Bullet0.SetPosition(new Vector3(5,0,0));

// 弾1の位置を設定
//m_Bullet1.SetPosition(new Vector3(10, 0, 0));


//// 発射回数の偶奇はどちらか
//if (m_RealShotNum % 3 == 1)
//{
//    // 弾を削除する
//    m_Bullet1.DestroyBullet();

//    // 弾を生成する
//    m_Bullet1 = BulletController.ShotBulletWithoutBulletParam(this, true);

//    // 弾の発射からの経過時間を0にする
//    m_BulletLifeTime1 = 0;
//}
//else if (m_RealShotNum % 3 == 2)
//{
//    // 弾を削除する
//    m_Bullet2.DestroyBullet();

//    // 弾を生成する
//    m_Bullet2 = BulletController.ShotBulletWithoutBulletParam(this, true);

//    // 弾の発射からの経過時間を0にする
//    m_BulletLifeTime2 = 0;
//}
//else
//{
//    // 弾を削除する
//    m_Bullet3.DestroyBullet();

//    // 弾を生成する
//    m_Bullet3 = BulletController.ShotBulletWithoutBulletParam(this, true);

//    // 弾の発射からの経過時間を0にする
//    m_BulletLifeTime3 = 0;
//}


// 弾の発射からの経過時間を進める
//m_BulletLifeTime1 += Time.deltaTime;
//m_BulletLifeTime2 += Time.deltaTime;
//m_BulletLifeTime3 += Time.deltaTime;

// 弾の位置を更新する
//m_Bullet1.SetPosition(new Vector3(m_BulletLifeTime1, 0, 0));
//m_Bullet2.SetPosition(new Vector3(m_BulletLifeTime2, 0, 0));
//m_Bullet3.SetPosition(new Vector3(m_BulletLifeTime3, 0, 0));




// 角速度なし
//position = m_BulletSpeed * m_BulletLifeTime[i] * new Vector3(1,0,0);



// 弾を削除する
//m_Bullet[index].DestroyBullet();



//[SerializeField]
//private float m_BulletIndex;


//[SerializeField]
//private float m_Time;

//[SerializeField]
//private float m_ShotInterval_m_RealShotNum;

//[SerializeField]
//private float m_C;


//m_BulletIndex = index;

//m_Time = Time.time;
//m_ShotInterval_m_RealShotNum = m_ShotInterval * m_RealShotNum;
//m_C = Time.time - m_ShotInterval * m_RealShotNum;

//m_BulletLifeTime[index] = 0;
