using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DanmakuAbstract : EnemyController
{

    // 実際の今までの発射回数
    [SerializeField]
    protected int realShotNum;


    // Start is called before the first frame update
    protected void Awake()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
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
            float dTime = Time.time - launchTime;

            // 弾を撃つ
            ShotBullets(launchTime, dTime);
        }
    }


    // 本体の位置とオイラー角を更新する
    abstract protected void UpdateSelf();

    // 現在のあるべき発射回数を計算する(小数)
    abstract protected float CalcNowShotNum();

    // 発射時刻を計算する
    abstract protected float CalcLaunchTime();

    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    abstract protected void ShotBullets(float launchTime, float dTime);


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
