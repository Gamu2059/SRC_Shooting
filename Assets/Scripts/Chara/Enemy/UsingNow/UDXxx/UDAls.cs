using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDAls : DanmakuCountAbstract2
{

    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_Float[(int)Als.FLOAT.shotInterval];
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_Float[(int)Als.FLOAT.shotInterval] * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleHackingBossBehavior enemyController, float launchTime, float dTime)
    {

        Vector3 posRandomZure;

        if (m_Float[(int)Als.FLOAT.shotBlurRadius] == 0)
        {
            posRandomZure = RandomCircleInsideToV3(m_Float[(int)Als.FLOAT.shotBlurRadius]);
        }
        else
        {
            posRandomZure = Vector3.zero;
        }


        // これ以降で使うフィールドは、bool値によらず必ず使うものだけを使う。

        // 発射時刻による角度ズレ
        float bigRad = m_Float[(int)Als.FLOAT.angleSpeed] * m_RealShotNum * m_Float[(int)Als.FLOAT.shotInterval];

        for (int i = 0; i < m_Int[(int)Als.INT.way]; i++)
        {

            // way数による角度のズレ
            float wayRad = Mathf.PI * 2 * i / m_Int[(int)Als.INT.way];

            // 発射された弾の現在の位置
            Vector3 pos;
            pos = enemyController.GetEnemy().transform.position;
            pos += m_Vector3[(int)Als.VECTOR3.shotAvePosition];
            pos += RThetaToVector3(m_Float[(int)Als.FLOAT.bulletSourceRadius], bigRad);
            pos += posRandomZure;

            // 発射角度
            float shotAngle = bigRad + Mathf.PI / 2 + wayRad;

            ShotTouchokuBullet(enemyController, m_Int[(int)Als.INT.bulletIndex], pos, shotAngle, m_Float[(int)Als.FLOAT.bulletSpeed], dTime);
        }
    }
}