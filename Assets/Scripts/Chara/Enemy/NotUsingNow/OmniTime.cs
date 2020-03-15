#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniTime : BattleRealEnemyBase
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 今まで発射した回数
    [SerializeField]
    private int shotNum;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;

    // way数
    [SerializeField]
    private int way;

    void Update()
    {

        // 現在のあるべき発射回数
        int nowShotNum = Mathf.FloorToInt(Time.time / m_ShotInterval);

        // 弾の発射
        while (shotNum < nowShotNum)
        {
            // 発射する弾の番号にする
            shotNum++;

            // 発射時刻
            float launchTime = m_ShotInterval * shotNum;

            ShotBBullet(launchTime);
        }
    }


    // 現在、与えられた時刻からどれだけ経っているか
    float CalcDTime(float time)
    {
        return Time.time - time;
    }


    // 角度からオイラー角を計算する
    Vector3 CalcEulerAngles(float rad)
    {
        Vector3 angle = transform.eulerAngles;
        angle.y = -(rad * Mathf.Rad2Deg) + 90;
        return angle;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    void ShotBulletPosAndEuler(float launchTime, float dTime)
    {

        float distance = m_BulletSpeed * dTime;

        // ランダムな角度
        float rad0 = Random.Range(0, Mathf.PI * 2);

        for (int i = 0; i < way; i++)
        {
            // 1つの弾の角度
            float rad = rad0 + Mathf.PI * 2 * i / way;

            // その弾の発射角度
            Vector3 eulerAngles;
            eulerAngles = CalcEulerAngles(rad);

            // 発射された弾の現在の位置
            Vector3 pos = transform.position;
            pos += new Vector3(distance * Mathf.Cos(rad), 0, distance * Mathf.Sin(rad));

            // 弾を撃つ
            //ShotBullet(0, 0, pos, eulerAngles);
        }
    }


    // 発射からの経過時間を計算して弾を撃つ
    void ShotBBullet(float launchTime)
    {
        // 現在は、本来撃たれた時刻からこれだけ経っている
        float dTime = CalcDTime(launchTime);

        ShotBulletPosAndEuler(launchTime, dTime);
    }
}
