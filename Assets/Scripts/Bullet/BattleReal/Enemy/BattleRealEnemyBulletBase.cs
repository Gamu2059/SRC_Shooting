using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealEnemyBulletBase : BulletController
{
    [SerializeField]
    protected bool m_IsDestoryOnHitOther;
    public bool IsDestoryOnHitOther => m_IsDestoryOnHitOther;

    public override void OnInitialize()
    {
        base.OnInitialize();
        AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot01Se);
    }

    protected override void OnEnterHitChara(HitSufferData<BattleRealCharaController> hitData)
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
