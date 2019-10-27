using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDWap : DanmakuCountAbstract
{

    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_Float[(int)Wap.FLOAT.発射間隔];
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_Float[(int)Wap.FLOAT.発射間隔] * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleRealEnemyController enemyController, float launchTime, float dTime)
    {
        Vector3 posRandomZure;

        if (m_Float[(int)Wap.FLOAT.発射位置のブレ範囲の円の半径] != 0)
        {
            posRandomZure = RandomCircleInsideToV3(m_Float[(int)Wap.FLOAT.発射位置のブレ範囲の円の半径]);
        }
        else
        {
            posRandomZure = Vector3.zero;
        }


        //// 角度の増分
        //float deltaAngle = m_Float[(int)Wap.FLOAT.角速度] * m_Float[(int)Wap.FLOAT.発射間隔];
        //deltaAngle %= Mathf.PI * 2;
        //deltaAngle += m_Float[(int)Wap.FLOAT.角加速度] * m_Float[(int)Wap.FLOAT.発射間隔] * m_Float[(int)Wap.FLOAT.発射間隔] / 2;
        //deltaAngle %= Mathf.PI * 2;

        // 前回の角速度
        float pastAngleSpeed = m_Float[(int)Wap.FLOAT.角速度];

        // 角速度を現在のものにする
        m_Float[(int)Wap.FLOAT.角速度] += m_Float[(int)Wap.FLOAT.角加速度] * m_Float[(int)Wap.FLOAT.発射間隔];

        // 角度の増分
        float deltaAngle = (pastAngleSpeed + m_Float[(int)Wap.FLOAT.角速度]) / 2 * m_Float[(int)Wap.FLOAT.発射間隔];
        deltaAngle %= Mathf.PI * 2;

        m_Float[(int)Wap.FLOAT.角速度] %= Mathf.PI * 2 / m_Float[(int)Wap.FLOAT.発射間隔];

        // 角度を現在のものにする
        m_Float[(int)Wap.FLOAT.角度] += deltaAngle;
        m_Float[(int)Wap.FLOAT.角度] %= Mathf.PI * 2;

        Debug.Log((m_Float[(int)Wap.FLOAT.角加速度],m_Float[(int)Wap.FLOAT.角速度], m_Float[(int)Wap.FLOAT.角度]));

        for (int i = 0; i < m_Int[(int)Wap.INT.way]; i++)
        {
            // 1つの弾の角度
            float rad = m_Float[(int)Wap.FLOAT.角度] + Mathf.PI * 2 * i / m_Int[(int)Wap.INT.way];

            // 発射された弾の現在の位置
            Vector3 pos;
            pos = enemyController.transform.position;
            pos += m_Vector3[(int)Wap.VECTOR3.発射平均位置];
            pos += posRandomZure;
            pos += RThetaToVector3(m_Float[(int)Wap.FLOAT.弾源円半径], rad);

            ShotTouchokuBullet(enemyController, m_Int[(int)Wap.INT.bulletIndex], pos, rad, m_Float[(int)Wap.FLOAT.弾速], dTime);
        }
    }
}




//// 発射角度
//float angle = m_Float[(int)Wap.FLOAT.角加速度] * launchTime * launchTime / 2;
//angle = angle % Mathf.PI * 2;