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
        DestroyBullet();
    }
}
