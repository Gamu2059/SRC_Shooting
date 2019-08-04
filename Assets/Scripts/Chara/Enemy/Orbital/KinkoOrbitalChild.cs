using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinkoOrbitalChild : SpecialOrbitalAbstract
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

    // 弾の角度
    [SerializeField]
    private float[] m_BulletAngle;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_BulletAngle = new float[m_BulletNum];

        for (int i = 0; i < m_BulletNum; i++)
        {
            m_BulletAngle[i] = 0;
        }
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    // 弾の位置を決める
    protected override Vector3 SetPosition(int i)
    {
        return m_BulletSpeed * m_BulletLifeTime[i] *
            new Vector3(Mathf.Cos(m_BulletAngle[i] - m_BulletLifeTime[i] * 0.5f), 0, Mathf.Sin(m_BulletAngle[i] - m_BulletLifeTime[i] * 0.5f));
    }


    // 現在のあるべき発射回数を計算する(小数)
    protected override float CalcNowShotNum()
    {
        return Time.time / m_ShotInterval;
    }


    // 発射時刻を計算する
    protected override float CalcLaunchTime()
    {
        return m_ShotInterval * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected override void ShotBullets(int index, float launchTime, float dTime)
    {
        // 弾の発射角度
        m_BulletAngle[index] = m_AngleSpeed * m_ShotInterval * m_RealShotNum;
    }
}
