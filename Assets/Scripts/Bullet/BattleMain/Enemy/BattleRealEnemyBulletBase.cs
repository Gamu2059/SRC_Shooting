using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealEnemyBulletBase : BulletController
{
    [SerializeField]
    protected bool m_IsDestoryOnHitOther;
    public bool IsDestoryOnHitOther => m_IsDestoryOnHitOther;

    protected override void OnEnterHitChara(HitSufferData<CharaController> hitData)
    {
        base.OnEnterHitChara(hitData);

        if (IsDestoryOnHitOther && !hitData.SufferCollider.Transform.IsTriggerCollider)
        {
            DestroyBullet();
        }
    }

    protected override void OnEnterHitBullet(HitSufferData<BulletController> hitData)
    {
        base.OnEnterHitBullet(hitData);

        if (IsDestoryOnHitOther && !hitData.SufferCollider.Transform.IsTriggerCollider)
        {
            DestroyBullet();
        }
    }
}
