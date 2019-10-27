using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDSwr : DanmakuCountAbstract
{

    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_Float[(int)Swr.FLOAT.発射間隔];
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_Float[(int)Swr.FLOAT.発射間隔] * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleRealEnemyController enemyController, float launchTime, float dTime)
    {
        Vector3 posRandomZure;

        if (m_Float[(int)Swr.FLOAT.発射位置のブレ範囲の円の半径] != 0)
        {
            posRandomZure = RandomCircleInsideToV3(m_Float[(int)Swr.FLOAT.発射位置のブレ範囲の円の半径]);
        }
        else
        {
            posRandomZure = Vector3.zero;
        }


        // 発射角度
        float angle = m_Float[(int)Swr.FLOAT.角速度] * launchTime;
        //float angle = m_Float[(int)Swr.FLOAT.角速度] * m_RealShotNum * m_Float[(int)Swr.FLOAT.発射間隔];
        angle = angle % Mathf.PI * 2;

        for (int i = 0; i < m_Int[(int)Swr.INT.way]; i++)
        {
            // 1つの弾の角度
            float rad = angle + Mathf.PI * 2 * i / m_Int[(int)Swr.INT.way];

            // 発射された弾の現在の位置
            Vector3 pos;
            pos = enemyController.transform.position;
            pos += m_Vector3[(int)Swr.VECTOR3.発射平均位置];
            pos += posRandomZure;
            pos += RThetaToVector3(m_Float[(int)Swr.FLOAT.弾源円半径], rad);

            ShotTouchokuBullet(enemyController, m_Int[(int)Swr.INT.bulletIndex], pos, rad, m_Float[(int)Swr.FLOAT.弾速], dTime);
        }
    }
}