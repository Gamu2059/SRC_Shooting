using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DanmakusAbstract : EnemyController
{

    // 実際の今までの発射回数
    [SerializeField]
    protected int[] realShotNum;

    // デリゲートを定義する
    protected delegate float CalcRealShotNum();
    protected delegate float CalcLaunchTime(float realShotNum);
    protected delegate void ShotBullets(float launchTime, float dTime);

    // デリゲートの配列
    protected CalcRealShotNum[] calcRealShotNum;
    protected CalcLaunchTime[] calcLaunchTime;
    protected ShotBullets[] shotBullets;


    // Start is called before the first frame update
    protected void Awake(CalcRealShotNum[] calcRealShotNums, CalcLaunchTime[] calcLaunchTimes, ShotBullets[] shotBulletss)
    {
        calcRealShotNum = calcRealShotNums;
        calcLaunchTime = calcLaunchTimes;
        shotBullets = shotBulletss;

        realShotNum = new int[shotBullets.Length];
    }

    // Update is called once per frame
    protected void Update()
    {

        // 本体の位置とオイラー角を更新する
        UpdateSelf();

        // それぞれの弾幕について処理をする
        for (int i = 0; i < shotBullets.Length; i++)
        {
            // 現在のあるべき発射回数
            int properShotNum = Mathf.FloorToInt(calcRealShotNum[i]());

            // 発射されるべき回数分、弾を発射する
            while (realShotNum[i] < properShotNum)
            {
                // 発射する弾の番号にする
                realShotNum[i]++;

                // 発射時刻
                float launchTime = calcLaunchTime[i](realShotNum[i]);

                // 発射からの経過時間
                float dTime = Time.time - launchTime;

                // 弾を撃つ
                shotBullets[i](launchTime, dTime);
            }
        }
    }


    // 本体の位置とオイラー角を更新する
    abstract protected void UpdateSelf();


    // 角度からオイラー角を計算する
    protected Vector3 CalcEulerAngles(float rad)
    {
        Vector3 angle = transform.eulerAngles;
        angle.y = -(rad * Mathf.Rad2Deg) + 90;
        return angle;
    }


    // 2πで割った余りにする
    protected float Modulo2PI(float rad)
    {
        rad %= Mathf.PI * 2;
        return rad;
    }


    // オイラー角から角度を計算する
    protected float CalcRad()
    {
        Vector3 angle = transform.eulerAngles;
        return (90 - angle.y) * Mathf.Deg2Rad;
    }
}
