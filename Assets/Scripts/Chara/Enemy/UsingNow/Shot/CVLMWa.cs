using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 等速直線運動する、全方位弾を発射するときのクラス。
/// </summary>
public class CVLMWa : CVLM
{

    /// <summary>
    /// way数。
    /// </summary>
    public int m_Way;


    /// <summary>
    /// インスタンスを生成する。
    /// </summary>
    public CVLMWa(CVLM cVLMShotParam, int way)
        : base(cVLMShotParam)
    {
        m_Way = way;
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

            ShootDra(wayRad);
        }
    }
}
