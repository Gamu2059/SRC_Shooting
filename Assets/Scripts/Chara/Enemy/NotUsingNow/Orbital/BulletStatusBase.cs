using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 弾一つひとつが持っている情報。
[System.Serializable]
public class BulletStatusBase : System.Object
{
    // 弾
    [SerializeField]
    protected BulletController m_Bullet;

    // 弾の発射されてからの経過時間
    [SerializeField]
    protected float m_LifeTime;
}
