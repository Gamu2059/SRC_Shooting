using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 単純な軌道の弾幕の抽象クラス。
// 発射回数をひたすら数えて、発射するかどうかを判定する。
[System.Serializable]
public abstract class DanmmakuAbstractObject : System.Object
{

    [SerializeField, Tooltip("何番目の弾か")]
    protected int[] m_BulletIndex;

    [SerializeField, Tooltip("何番目の弾パラメータか")]
    protected int[] m_BulletParamIndex;

    // 実際の今までの発射回数
    protected int m_RealShotNum;

    [SerializeField, Tooltip("その弾幕の開始からの経過時間")]
    protected float m_Time;


    // Update is called once per frame
    public void Updates(EnemyController enemyController,float time)
    {

        // 本体の位置とオイラー角を更新する
        //UpdateSelf();

        // 現在のあるべき発射回数
        int properShotNum = Mathf.FloorToInt(CalcNowShotNum(time));

        // 発射されるべき回数分、弾を発射する
        while (m_RealShotNum < properShotNum)
        {
            // 発射する弾の番号にする
            m_RealShotNum++;

            // 発射時刻
            float launchTime = CalcLaunchTime();

            // 発射からの経過時間
            float dTime = time - launchTime;

            // 弾を撃つ
            ShotBullets(enemyController, launchTime, dTime);
        }
    }


    // 本体の位置とオイラー角を更新する
    //public abstract void UpdateSelf();

    // 現在のあるべき発射回数を計算する(小数)
    public abstract float CalcNowShotNum(float time);

    // 発射時刻を計算する
    public abstract float CalcLaunchTime();

    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public abstract void ShotBullets(EnemyController enemyController,float launchTime, float dTime);


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
