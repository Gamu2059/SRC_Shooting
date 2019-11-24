using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDSnc : RegularIntervalUDAbstract
{

    /// <summary>
    /// 発射間隔を取得する。
    /// </summary>
    public override float GetShotInterval()
    {
        return m_Float[(int)Idr.FLOAT.shotInterval];
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleHackingBossBehavior enemyController, float launchTime, float dTime)
    {

        m_Float[(int)Snc.FLOAT.sourceAngle] += m_Float[(int)Snc.FLOAT.sourceAngleSpeed] * m_Float[(int)Snc.FLOAT.shotInterval];
        m_Float[(int)Snc.FLOAT.sourceAngle] %= Mathf.PI * 2;

        CVLMWa cVLMWa = new CVLMWa(
            new CVLM(
                enemyController,
                m_Int[(int)Idr.INT.bulletIndex],
                Vector3.zero,
                m_Float[(int)Idr.FLOAT.angle],
                m_Float[(int)Idr.FLOAT.bulletSpeed],
                dTime
                ),
            m_Int[(int)Idr.INT.way]
            );

        cVLMWa.PlusPosition(enemyController.GetEnemy().transform.position);
        cVLMWa.PlusPosition(m_Vector3[(int)Idr.VECTOR3.shotAvePosition]);
        cVLMWa.PlusPosition(Calc.RThetaToVec3(m_Float[(int)Idr.FLOAT.bulletSourceRadius], m_Float[(int)Idr.FLOAT.sourceAngle]));

        cVLMWa.Shoot();
    }
}