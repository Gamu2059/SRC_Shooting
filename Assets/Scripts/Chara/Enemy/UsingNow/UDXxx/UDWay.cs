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

        CVLMWyDrRa cVLMShotParam = new CVLMWyDrRa(
            new CVLM(
                enemyController,
                m_Int[(int)Way.INT.bulletIndex],
                Vector3.zero,
                0,
                m_Float[(int)Way.FLOAT.bulletSpeed],
                dTime
                ),
                m_Int[(int)Way.INT.way],
                m_Float[(int)Way.FLOAT.dAngle],
                m_Float[(int)Way.FLOAT.bulletSourceRadius]
                );

        cVLMShotParam.PlusPosition(enemyController.GetEnemy().transform.position);
        cVLMShotParam.PlusPosition(m_Vector3[(int)Way.VECTOR3.shotAvePosition]);
        cVLMShotParam.PlusPosition(Calc.RandomCircleInsideToV3AndZero(m_Float[(int)Way.FLOAT.shotBlurRadius]));

        cVLMShotParam.PlusAngle(V3ToRelativeAngle(cVLMShotParam.Position, Calc.GetPlayerPosition()));

        cVLMShotParam.Shoot();
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


//for (int i = -(m_Int[(int)Way.INT.way] - 1); i <= m_Int[(int)Way.INT.way] - 1; i += 2)
//{
//    // 1つの弾の角度
//    float rad = centerRad + Mathf.PI * 2 * i * m_Float[(int)Way.FLOAT.dAngle] / 2;


//cVLMShotParam.PlusPosition(RThetaToVector3(m_Float[(int)Way.FLOAT.bulletSourceRadius], rad));


//}


//Vector3 posRandomZure;

//        if (m_Float[(int)Way.FLOAT.shotBlurRadius] != 0)
//        {
//            posRandomZure = RandomCircleInsideToV3(m_Float[(int)Way.FLOAT.shotBlurRadius]);
//        }
//        else
//        {
//            posRandomZure = Vector3.zero;
//        }


//        float centerRad = V3ToRelativeAngle(enemyController.GetEnemy().transform.position + m_Vector3[(int)Way.VECTOR3.shotAvePosition] + posRandomZure,
//            BattleHackingPlayerManager.Instance.Player.transform.position);