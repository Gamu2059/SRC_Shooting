using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDJik : DanmakuCountAbstract
{

    private enum INT
    {
        何番目の弾か,
        way数,
    }


    private enum FLOAT
    {
        発射間隔,
        wayごとの角度差,
        弾速,
    }


    private enum VECTOR3
    {
        発射位置,
    }


    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_Float[(int)FLOAT.発射間隔];
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_Float[(int)FLOAT.発射間隔] * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleRealEnemyBase enemyController, float launchTime, float dTime)
    {
        var player = BattleRealPlayerManager.Instance.Player;
        Vector3 relativePosition = player.transform.position - (enemyController.transform.position + m_Vector3[(int)VECTOR3.発射位置]);
        relativePosition.Normalize();

        float relativeRad = Mathf.Atan2(relativePosition.z, relativePosition.x);

        // way数の分弾を撃つ
        for (int aWay = -m_Int[(int)INT.way数] + 1; aWay < m_Int[(int)INT.way数]; aWay++)
        {
            // このwayで撃つ角度
            float wayRad = relativeRad + m_Float[(int)FLOAT.wayごとの角度差] * aWay;

            // 発射された弾の位置
            Vector3 pos = enemyController.transform.position;
            pos += m_Vector3[(int)VECTOR3.発射位置];

            ShotTouchokuBullet(enemyController, m_Int[(int)INT.何番目の弾か], pos, wayRad, m_Float[(int)FLOAT.弾速], dTime);
        }
    }
}




//[SerializeField, Tooltip("発射位置")]
//private Vector3 m_LaunchPosition;

//[SerializeField, Tooltip("発射間隔")]
//private float m_ShotInterval;

//[SerializeField, Tooltip("way数")]
//private int m_Way;

//[SerializeField, Tooltip("1wayごとの角度差")]
//private float m_RadAWay;

//[SerializeField, Tooltip("弾速")]
//private float m_BulletSpeed;


//m_ShotInterval = uDParameters.m_FloatParameters[0];


//Vector3 relativeVector = new Vector3(0, 0, -10) - enemyController.transform.position;

//Vector3 eulerAngles;

// 撃つ角度を決定する
//eulerAngles = CalcEulerAngles(enemyController.transform.eulerAngles, wayRad);

// 弾速の数だけ弾を撃つ
// 速度差のある弾を撃つプログラムの名残があります

//float distance = m_Float[(int)FLOAT.弾速] * dTime;

//pos += distance * new Vector3(Mathf.Cos(wayRad), 0, Mathf.Sin(wayRad));

// 弾を撃つ
//BulletShotParam bulletShotParam = new BulletShotParam(enemyController, m_Int[(int)INT.何番目の弾か], m_Int[(int)INT.何番目の弾パラメータか], 0, pos, eulerAngles, enemyController.transform.localScale);
//BulletController.ShotBullet(bulletShotParam);