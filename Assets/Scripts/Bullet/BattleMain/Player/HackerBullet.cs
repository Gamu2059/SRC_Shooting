using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerBullet : BulletController
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        m_IsLookMoveDir = false;
    }

    protected override void OnEnterHitChara(HitSufferData<CharaController> hitData)
    {
        base.OnEnterHitChara(hitData);

        var colliderType = hitData.SufferCollider.Transform.ColliderType;
        switch (colliderType)
        {
            case E_COLLIDER_TYPE.CRITICAL:
                DestroyBullet();
                break;
        }
    }
}
