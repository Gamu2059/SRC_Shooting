#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinkoTime : BattleRealEnemyController
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 実際の今までの発射回数
    [SerializeField]
    private int realShotNum;

    // 角速度
    [SerializeField]
    private float m_AngleSpeed;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;

    // Update is called once per frame
    void Update()
    {

        // 本体の位置とオイラー角を更新する
        UpdateSelf();

        // 現在のあるべき発射回数
        int properShotNum = Mathf.FloorToInt(CalcNowShotNum());

        // 発射されるべき回数分、弾を発射する
        while (realShotNum < properShotNum)
        {
            // 発射する弾の番号にする
            realShotNum++;

            // 発射時刻
            float launchTime = CalcLaunchTime();

            // 発射からの経過時間
            float dTime = Time.time -  launchTime;

            // 弾を撃つ
            ShotBullets(launchTime, dTime);
        }
    }


    // 本体の位置とオイラー角を更新する
    void UpdateSelf()
    {
        // 角度を進める
        float rad = m_AngleSpeed * Time.time;
        rad = Modulo2PI(rad);

        // 角度を反映させる
        transform.eulerAngles = CalcEulerAngles(rad);
    }


    // 現在のあるべき発射回数を計算する(小数)
    float CalcNowShotNum()
    {
        return Time.time / m_ShotInterval;
    }


    // 発射時刻を計算する
    float CalcLaunchTime()
    {
        return m_ShotInterval* realShotNum;
    }


    // 角度からオイラー角を計算する
    Vector3 CalcEulerAngles(float rad)
    {
        Vector3 angle = transform.eulerAngles;
        angle.y = -(rad * Mathf.Rad2Deg) + 90;
        return angle;
    }


    // 2πで割った余りにする
    float Modulo2PI(float rad)
    {
        rad %= Mathf.PI * 2;
        return rad;
    }


    // オイラー角から角度を計算する
    float CalcRad()
    {
        Vector3 angle = transform.eulerAngles;
        return (90 - angle.y) * Mathf.Deg2Rad;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    void ShotBullets(float launchTime, float dTime)
    {
        Vector3 pos = transform.position;

        float pastRad = m_AngleSpeed * m_ShotInterval * realShotNum;
        pastRad = Modulo2PI(pastRad);

        float distance = m_BulletSpeed * dTime;

        pos += new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad));

        Vector3 eulerAngles;

        eulerAngles = CalcEulerAngles(pastRad);

        //ShotBullet(0, 0, pos, eulerAngles);
    }


    // これ以下は使っていない


    // 現在、与えられた時刻からどれだけ経っているか
    float CalcDTime(float time)
    {
        return Time.time - time;
    }


    // 弾の位置[発射時刻、発射からの経過時間]
    Vector3 BulletPos(float launchTime, float dTime)
    {
        Vector3 pos = transform.position;

        float pastRad = m_AngleSpeed * m_ShotInterval * realShotNum;
        float distance = m_BulletSpeed * dTime;

        pos += new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad));

        return pos;
    }


    // 弾のオイラー角[発射時刻、発射からの経過時間]
    Vector3 BulletEuler(float launchTime, float dTime)
    {
        Vector3 eulerAngles;

        float pastRad = m_AngleSpeed * m_ShotInterval * realShotNum;
        pastRad = Modulo2PI(pastRad);

        eulerAngles = CalcEulerAngles(pastRad);

        return eulerAngles;
    }


    // 弾の位置とオイラー角[発射時刻、発射からの経過時間]
    (Vector3 position, Vector3 eulerAngles) BulletPosAndEuler(float launchTime, float dTime)
    {
        Vector3 pos = transform.position;

        float pastRad = m_AngleSpeed * m_ShotInterval * realShotNum;
        pastRad = Modulo2PI(pastRad);

        float distance = m_BulletSpeed * dTime;

        pos += new Vector3(distance * Mathf.Cos(pastRad), 0, distance * Mathf.Sin(pastRad));

        Vector3 eulerAngles;

        eulerAngles = CalcEulerAngles(pastRad);

        return (pos, eulerAngles);
    }


    // 弾を撃つ
    void ShotABullet(float launchTime, float dTime)
    {
        Vector3 pos = BulletPos(launchTime, dTime);

        Vector3 pastAngle = BulletEuler(launchTime, dTime);

        // 弾を撃つ
        //ShotBullet(0, 0, pos, pastAngle);
    }


    // 弾を撃つ(Bを展開したもの)
    void ShotCBullet(float launchTime)
    {
        // 現在は、本来撃たれた時刻からこれだけ経っている
        float dTime = CalcDTime(launchTime);

        // 弾を撃つ
        //ShotBullet(0, 0, BulletPos(launchTime, dTime), BulletEuler(launchTime, dTime));
    }


    // タプルを使って弾を撃つ
    void ShotDBullet(float launchTime)
    {
        // 現在は、本来撃たれた時刻からこれだけ経っている
        float dTime = CalcDTime(launchTime);

        (Vector3 position, Vector3 eulerAngles) = BulletPosAndEuler(launchTime, dTime);

        // 弾を撃つ
        //ShotBullet(0, 0, position, eulerAngles);
    }


    // 発射からの経過時間を計算して弾を撃つ
    void ShotBBullet(float launchTime)
    {
        // 現在は、本来撃たれた時刻からこれだけ経っている
        float dTime = CalcDTime(launchTime);

        ShotBullets(launchTime, dTime);
    }
}