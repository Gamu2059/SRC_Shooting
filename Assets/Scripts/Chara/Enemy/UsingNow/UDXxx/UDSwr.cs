using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDSwr : RegularIntervalUDAbstract
{

    /// <summary>
    /// 発射間隔を取得する。
    /// </summary>
    public override float GetShotInterval()
    {
        return m_Float[(int)Omn.FLOAT.shotInterval];
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    public override void ShotBullets(BattleHackingBossBehavior enemyController, float launchTime, float dTime)
    {

        // intとか基本型をメソッドで書き換えたい
        // パラメータ自体を書き換える時はこうするしかないか
        m_Float[(int)Swr.FLOAT.angle] = Calc.PlusRad(m_Float[(int)Swr.FLOAT.angle], m_Float[(int)Swr.FLOAT.angleSpeed]);


        CVLMWaRa cVLMWaRaSp = new CVLMWaRa(
        new CVLM(
            enemyController,
            m_Int[(int)Swr.INT.bulletIndex],
            Vector3.zero,
            m_Float[(int)Swr.FLOAT.angle],
            m_Float[(int)Swr.FLOAT.bulletSpeed],
            dTime
            ),
        m_Int[(int)Swr.INT.way],
        m_Float[(int)Swr.FLOAT.bulletSourceRadius]
        );

        cVLMWaRaSp.PlusPosition(enemyController.GetEnemy().transform.position);
        cVLMWaRaSp.PlusPosition(m_Vector3[(int)Swr.VECTOR3.発射平均位置]);
        cVLMWaRaSp.PlusPosition(Calc.RandomCircleInsideToV3AndZero(m_Float[(int)Swr.FLOAT.発射位置のブレ範囲の円の半径]));

        cVLMWaRaSp.Shoot();
    }
}




//Vector3 posRandomZure;

//if (m_Float[(int)Swr.FLOAT.発射位置のブレ範囲の円の半径] != 0)
//{
//    posRandomZure = RandomCircleInsideToV3(m_Float[(int)Swr.FLOAT.発射位置のブレ範囲の円の半径]);
//}
//else
//{
//    posRandomZure = Vector3.zero;
//}


// 発射角度
//float angle = m_Float[(int)Swr.FLOAT.角速度] * m_RealShotNum;
////float angle = m_Float[(int)Swr.FLOAT.角速度] * m_RealShotNum * m_Float[(int)Swr.FLOAT.発射間隔];
//angle = angle % Mathf.PI * 2;


//for (int i = 0; i < m_Int[(int)Swr.INT.way]; i++)
//{
//    // 1つの弾の角度
//    float rad = m_Float[(int)Swr.FLOAT.angle] + Mathf.PI * 2 * i / m_Int[(int)Swr.INT.way];


//}


//m_Float[(int)Swr.FLOAT.angle] += m_Float[(int)Swr.FLOAT.角速度];
//m_Float[(int)Swr.FLOAT.angle] %= Calc.TWO_PI;