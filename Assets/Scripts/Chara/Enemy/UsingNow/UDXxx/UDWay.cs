using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDWay : DanmakuCountAbstract
{

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


    // 現在のあるべき発射回数を計算する(小数)
    public override float CalcNowShotNum(float time)
    {
        return time / m_Float[(int)Way.FLOAT.発射間隔];
    }


    // 発射時刻を計算する
    public override float CalcLaunchTime()
    {
        return m_Float[(int)Way.FLOAT.発射間隔] * m_RealShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(EnemyController enemyController, float launchTime, float dTime)
    {

        Vector3 avePosition;

        if (m_Bool[(int)Way.BOOL.発射平均位置を指定するかどうか])
        {
            avePosition = m_Vector3[(int)Way.VECTOR3.発射平均位置];
        }
        else
        {
            avePosition = Vector3.zero;
        }


        Vector3 posRandomZure;

        if (m_Bool[(int)Way.BOOL.発射中心位置を円内にブレさせるかどうか])
        {
            posRandomZure = RandomCircleInsideToV3(m_Float[(int)Way.FLOAT.発射位置のブレ範囲の円の半径]);
        }
        else
        {
            posRandomZure = Vector3.zero;
        }


        // これ以降で使うフィールドは、bool値によらず必ず使うものだけを使う。

        float centerRad = V3ToRelativeRad(enemyController.transform.position + m_Vector3[(int)Way.VECTOR3.発射平均位置] + posRandomZure,
            PlayerCharaManager.Instance.GetCurrentController().transform.position);

        for (int i = -(m_Int[(int)Way.INT.way数] - 1); i <= m_Int[(int)Way.INT.way数] - 1; i += 2)
        {
            // 1つの弾の角度
            float rad = centerRad + Mathf.PI * 2 * i * m_Float[(int)Way.FLOAT.角度差] / 2;

            // 発射された弾の現在の位置
            Vector3 pos;
            pos = enemyController.transform.position;
            pos += avePosition;
            pos += posRandomZure;
            pos += RThetaToVector3(m_Float[(int)Way.FLOAT.弾源円半径], rad);

            ShotTouchokuBullet(enemyController, m_Int[(int)Way.INT.何番目の弾か], pos, rad, m_Float[(int)Way.FLOAT.弾速], dTime);
        }
    }
}