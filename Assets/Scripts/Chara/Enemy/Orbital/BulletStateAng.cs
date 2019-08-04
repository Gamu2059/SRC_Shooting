using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 弾一つひとつが持っている情報。角度あり。
[System.Serializable]
public class BulletStateAng : BulletStatusBase
{
    // 弾の角度
    [SerializeField]
    protected float m_Angle;
}
