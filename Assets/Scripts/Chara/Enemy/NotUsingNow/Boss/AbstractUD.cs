using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbstractUD : System.Object
{

    // 何番目の弾か
    [SerializeField]
    protected int m_BulletIndex;

    // 何番目の弾パラメータか
    [SerializeField]
    protected int m_BulletParamIndex;

    // 実際の今までの発射回数
    [SerializeField]
    protected int realShotNum;


    [SerializeField, Tooltip("現在の素弾幕内の時刻")]
    protected float m_Time;

    [SerializeField, Tooltip("形態内での開始時刻")]
    public float m_BeginTime;

    [SerializeField, Tooltip("形態内での終了時刻")]
    public float m_FinishTime;


    // 状態
    public enum STATE
    {
        OUT,
        GET_IN,
        IN,
        GET_OUT,
    }

    // 状態
    public STATE m_State;


    public void Judge(float time)
    {
        // 今、内か外か
        bool isIn = m_BeginTime <= time && time <= m_FinishTime;

        // 中に入った直後の場合
        if (m_State == STATE.OUT && isIn)
        {
            m_State = STATE.GET_IN;
        }
        // 外に出た直後の場合
        else if (m_State == STATE.IN && !isIn)
        {
            m_State = STATE.GET_OUT;
        }
        // 前フレームと状態が変わらない場合
        else if (isIn)
        {
            m_State = STATE.IN;
        }
        else
        {
            m_State = STATE.OUT;
        }
    }


    // Update is called once per frame
    public void Updates(BattleRealEnemyController enemyController, float time)
    {
        Judge(time);

        int properShotNum;

        switch (m_State)
        {
            case STATE.IN:

                // 経過時間を進める
                m_Time += Time.deltaTime;

                // 現在のあるべき発射回数
                properShotNum = Mathf.FloorToInt(CalcNowShotNum());

                // 発射されるべき回数分、弾を発射する
                while (realShotNum < properShotNum)
                {
                    // 発射する弾の番号にする
                    realShotNum++;

                    // 発射時刻
                    float launchTime = CalcLaunchTime();

                    // 発射からの経過時間
                    float dTime = m_Time - launchTime;

                    // 弾を撃つ
                    ShotBullets(enemyController, launchTime, dTime);
                }

                break;

            case STATE.OUT:
                break;

            case STATE.GET_IN:

                // 経過時間を進める
                m_Time += time - m_BeginTime;

                // 現在のあるべき発射回数
                properShotNum = Mathf.FloorToInt(CalcNowShotNum());

                // 発射されるべき回数分、弾を発射する
                while (realShotNum < properShotNum)
                {
                    // 発射する弾の番号にする
                    realShotNum++;

                    // 発射時刻
                    float launchTime = CalcLaunchTime();

                    // 発射からの経過時間
                    float dTime = m_Time - launchTime;

                    // 弾を撃つ
                    ShotBullets(enemyController, launchTime, dTime);
                }

                break;

            case STATE.GET_OUT:

                // 最後まで時間を進める
                m_Time = m_FinishTime - m_BeginTime;

                // 現在のあるべき発射回数
                properShotNum = Mathf.FloorToInt(CalcNowShotNum());

                // 発射されるべき回数分、弾を発射する
                while (realShotNum < properShotNum)
                {
                    // 発射する弾の番号にする
                    realShotNum++;

                    // 発射時刻
                    float launchTime = CalcLaunchTime();

                    // 発射からの経過時間
                    float dTime = m_Time - launchTime;

                    // 弾を撃つ
                    ShotBullets(enemyController, launchTime, dTime);
                }

                m_Time = 0;
                realShotNum = 0;

                break;
        }
    }


    // 現在のあるべき発射回数を計算する(小数)
    public abstract float CalcNowShotNum();


    // 発射時刻を計算する
    public abstract float CalcLaunchTime();


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public abstract void ShotBullets(BattleRealEnemyController enemyController, float launchTime, float dTime);


    // 角度からオイラー角を計算する
    public Vector3 CalcEulerAngles(Vector3 eulerAngles, float rad)
    {
        Vector3 angle = eulerAngles;
        angle.y = -(rad * Mathf.Rad2Deg) + 90;
        return angle;
    }


    // 2πで割った余りにする
    public float Modulo2PI(float rad)
    {
        rad %= Mathf.PI * 2;
        return rad;
    }


    // オイラー角から角度を計算する
    public float CalcRad(Vector3 eulerAngles)
    {
        Vector3 angle = eulerAngles;
        return (90 - angle.y) * Mathf.Deg2Rad;
    }
}
