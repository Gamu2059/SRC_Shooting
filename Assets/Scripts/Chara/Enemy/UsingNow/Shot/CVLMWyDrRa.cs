using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等速直線運動する、半径のあるway弾を発射するときのクラス。
/// </summary>
public class CVLMWyDrRa : CVLM
{

    /// <summary>
    /// way数。
    /// </summary>
    public int m_Way;

    /// <summary>
    /// 隣り合う弾同士の発射角度差
    /// </summary>
    public float m_DRad;

    /// <summary>
    /// 弾源の円の半径。
    /// </summary>
    public float m_Radius;


    /// <summary>
    /// コンストラクタ。
    /// </summary>
    public CVLMWyDrRa(CVLM cVLMShotParam, int way, float dRad, float radius)
        : base(cVLMShotParam)
    {
        m_Way = way;
        m_DRad = dRad;
        m_Radius = radius;
    }


    /// <summary>
    /// 弾を撃つ。
    /// </summary>
    public override void Shoot()
    {

        for (int i = -(m_Way - 1); i <= m_Way - 1; i += 2)
        {
            // 1つの弾の角度
            float wayRad = Mathf.PI * 2 * i * m_DRad / 2;

            Vector3 wayPos = Calc.RThetaToVec3(m_Radius, m_VelocityRad + wayRad);

            ShootDpoDra(wayPos, wayRad);
        }
    }
}