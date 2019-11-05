using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等速直線運動する、半径のある全方位弾を等間隔に速度差をつけて発射するときのクラス。
/// </summary>
public class CVLMWaRaSp : CVLM
{

    /// <summary>
    /// way数。
    /// </summary>
    public int m_Way;

    /// <summary>
    /// 弾源の円の半径。
    /// </summary>
    public float m_Radius;

    /// <summary>
    /// 速度の異なる弾の数。
    /// </summary>
    public int m_SpeedNum;

    /// <summary>
    /// 速度差。
    /// </summary>
    public float m_DSpeed;


    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public CVLMWaRaSp(CVLM cVLMShotParam, int way, float radius,int speedNum,float dSpeed)
        : base(cVLMShotParam)
    {
        m_Way = way;
        m_Radius = radius;
        m_SpeedNum = speedNum;
        m_DSpeed = dSpeed;
    }


    /// <summary>
    /// 弾を撃つ。（ローカル関数を使っている）
    /// </summary>
    public override void Shoot()
    {
        if (m_SpeedNum >= 2)
        {
            for (int i = -(m_SpeedNum - 1); i <= m_SpeedNum - 1; i += 2)
            {
                float bulletSpeed = m_Speed + i * m_DSpeed / 2;

                Shoot2(i * m_DSpeed / 2);
            }
        }
        else
        {
            Shoot2(0);
        }


        void Shoot2(float dSpeed)
        {
            for (int i = 0; i < m_Way; i++)
            {
                // wayによる角度差
                float wayRad = Mathf.PI * 2 * i / m_Way;

                Vector3 wayPos = Calc.RThetaToVector3(m_Radius, m_VelocityRad + wayRad);

                ShootDpoDraDsp(wayPos, wayRad, dSpeed);
            }
        }
    }
}
