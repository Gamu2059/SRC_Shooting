using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDWay : RegularIntervalUDAbstract
{ 

    /// <summary>
    /// 発射間隔を取得する。
    /// </summary>
    public override float GetShotInterval()
    {
        return m_Float[(int)Way.FLOAT.shotInterval];
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleHackingBossBehavior enemyController, float launchTime, float dTime)
    {
        Vector3 posRandomZure;

        if (m_Float[(int)Way.FLOAT.shotBlurRadius] != 0)
        {
            posRandomZure = RandomCircleInsideToV3(m_Float[(int)Way.FLOAT.shotBlurRadius]);
        }
        else
        {
            posRandomZure = Vector3.zero;
        }


        // これ以降で使うフィールドは、bool値によらず必ず使うものだけを使う。

        float centerRad = V3ToRelativeRad(enemyController.GetEnemy().transform.position + m_Vector3[(int)Way.VECTOR3.shotAvePosition] + posRandomZure,
            BattleHackingPlayerManager.Instance.Player.transform.position);

        for (int i = -(m_Int[(int)Way.INT.way] - 1); i <= m_Int[(int)Way.INT.way] - 1; i += 2)
        {
            // 1つの弾の角度
            float rad = centerRad + Mathf.PI * 2 * i * m_Float[(int)Way.FLOAT.dAngle] / 2;

            CVLM cVLMShotParam = new CVLM(
                enemyController,
                m_Int[(int)Way.INT.bulletIndex],
                enemyController.GetEnemy().transform.position,
                rad,
                m_Float[(int)Way.FLOAT.bulletSpeed],
                dTime
                );

            cVLMShotParam.PlusPosition(m_Vector3[(int)Way.VECTOR3.shotAvePosition]);
            cVLMShotParam.PlusPosition(posRandomZure);
            cVLMShotParam.PlusPosition(RThetaToVector3(m_Float[(int)Way.FLOAT.bulletSourceRadius], rad));

            cVLMShotParam.Shoot();
        }
    }
}




//private enum BOOL
//{
//    発射平均位置を指定するかどうか,
//    発射中心位置を円内にブレさせるかどうか,
//}


//private enum INT
//{
//    何番目の弾か,
//    way数,
//}


//private enum FLOAT
//{
//    発射間隔,
//    弾速,
//    角度差,
//    弾源円半径,
//    発射位置のブレ範囲の円の半径,
//}


//private enum VECTOR3
//{
//    発射平均位置,
//}


//// 現在のあるべき発射回数を計算する(小数)
//public override float CalcNowShotNum(float time)
//{
//    return time / m_Float[(int)Way.FLOAT.shotInterval];
//}


//// 発射時刻を計算する
//public override float CalcLaunchTime()
//{
//    return m_Float[(int)Way.FLOAT.shotInterval] * m_RealShotNum;
//}


//// 発射された弾の現在の位置
//Vector3 pos;
//pos = enemyController.transform.position;
//pos += m_Vector3[(int)Way.VECTOR3.shotAvePosition];
//pos += posRandomZure;
//pos += RThetaToVector3(m_Float[(int)Way.FLOAT.bulletSourceRadius], rad);


//ShotTouchokuBullet(enemyController, m_Int[(int)Way.INT.bulletIndex], pos, rad, m_Float[(int)Way.FLOAT.bulletSpeed], dTime);