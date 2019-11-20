#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniOrbital : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // way数
    [SerializeField]
    private int m_way;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed;

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
    private int m_nextBulletIndex = 0;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected override float CalcNowShotNum()
    {
        return Time.time / m_ShotInterval;
    }


    // 発射時刻を計算する
    protected override float CalcLaunchTime()
    {
        return m_ShotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected override void ShotBullets(float launchTime, float dTime)
    {

        float distance = m_BulletSpeed * dTime;

        // ランダムな角度
        float randomAngle = Random.Range(0, Mathf.PI * 2);

        for (int i = 0; i < m_way; i++)
        {
            // wayによる角度変動
            float wayRad = Mathf.PI * 2 * i / m_way;

            // 1つの弾の発射角度
            float launchAngle = randomAngle + wayRad;

            // その弾の発射角度
            Vector3 eulerAngles;
            eulerAngles = CalcEulerAngles(launchAngle);

            // 発射された弾の現在の位置
            Vector3 pos = transform.position;
            pos += new Vector3(distance * Mathf.Cos(launchAngle), 0, distance * Mathf.Sin(launchAngle));

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
            m_bullet[m_nextBulletIndex] = BulletController.ShotBulletWithoutBulletParam(bulletShotParam,true);

            // 弾のパラメータを入力する
            m_bulletLifeTime[m_nextBulletIndex] = Time.time - launchTime;
            m_bulletAngle[m_nextBulletIndex] = launchAngle;

            // 発射する弾のインデックスを進める
            m_nextBulletIndex++;
        }
    }


    protected override void Update()
    {
        base.Update();

        // 弾の状態を更新する
        for (int i = 0; i < m_bullet.Length; i++)
        {
            // 弾の経過時間進める
            m_bulletLifeTime[i] += Time.deltaTime;

            // 位置
            Vector3 position = transform.position;
            position += m_BulletSpeed * m_bulletLifeTime[i] * new Vector3(Mathf.Cos(m_bulletAngle[i]), 0, Mathf.Sin(m_bulletAngle[i]));

            // 位置を代入
            m_bullet[i].SetPosition(position);
        }
    }
}