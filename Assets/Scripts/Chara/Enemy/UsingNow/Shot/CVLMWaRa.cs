﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等速直線運動する、半径のある全方位弾を発射するときのクラス。
/// </summary>
public class CVLMWaRa : CVLM
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
    /// コンストラクタ。
    /// </summary>
    public CVLMWaRa(CVLM cVLMShotParam,int way,float radius)
        : base(cVLMShotParam)
    {
        m_Way = way;
        m_Radius = radius;
    }


    /// <summary>
    /// 弾を撃つ。
    /// </summary>
    public override void Shoot()
    {
        for (int i = 0; i < m_Way; i++)
        {
            // wayによる角度差
            float wayRad = Mathf.PI * 2 * i / m_Way;

            Vector3 wayPos = Calc.RThetaToVec3(m_Radius, m_VelocityRad + wayRad);

            ShootDpoDra(wayPos, wayRad);
        }
    }
}




//m_EnemyController = cVLMShotParam.m_EnemyController;
//m_BulletIndex = cVLMShotParam.m_BulletIndex;
//m_Position = cVLMShotParam.m_Position;
//m_VelocityRad = cVLMShotParam.m_VelocityRad;
//m_Speed = cVLMShotParam.m_Speed;
//m_DTime = cVLMShotParam.m_DTime;