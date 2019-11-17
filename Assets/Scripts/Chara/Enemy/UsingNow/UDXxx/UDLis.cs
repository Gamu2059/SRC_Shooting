using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDLis : DanmakuCountAbstract2
{

    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_Float[(int)Lis.FLOAT.shotInterval];
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_Float[(int)Lis.FLOAT.shotInterval] * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleHackingBossBehavior enemyController, float launchTime, float dTime)
    {

        // 発射された弾の現在の位置
        Vector3 shotPos;
        shotPos = enemyController.GetEnemy().transform.position;
        shotPos += m_Vector3[(int)Lis.VECTOR3.shotAvePosition];
        shotPos += new Vector3(
            m_Float[(int)Lis.FLOAT.ampX] * Mathf.Sin(m_Float[(int)Lis.FLOAT.angFreqX] * launchTime),0,
            m_Float[(int)Lis.FLOAT.ampY] * Mathf.Sin(m_Float[(int)Lis.FLOAT.angFreqY] * launchTime)
            );

        // 発射時刻による角度ズレ
        float bigAngle = Mathf.Atan2(
            m_Float[(int)Lis.FLOAT.ampY] * m_Float[(int)Lis.FLOAT.angFreqY] * Mathf.Cos(m_Float[(int)Lis.FLOAT.angFreqY] * launchTime),
            m_Float[(int)Lis.FLOAT.ampX] * m_Float[(int)Lis.FLOAT.angFreqX] * Mathf.Cos(m_Float[(int)Lis.FLOAT.angFreqX] * launchTime)
            );

        for (int i = 0; i < m_Int[(int)Lis.INT.way]; i++)
        {
            // 発射角度
            float shotAngle = bigAngle + Mathf.PI * 2 * i / m_Int[(int)Lis.INT.way];

            ShotTouchokuBullet(enemyController, m_Int[(int)Lis.INT.bulletIndex], shotPos, shotAngle, m_Float[(int)Lis.FLOAT.bulletSpeed], dTime);
        }
    }
}