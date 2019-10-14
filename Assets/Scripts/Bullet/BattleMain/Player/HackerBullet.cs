using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerBullet : BulletController
{
    protected override void OnEnterHitChara(HitSufferData<CharaController> hitData)
    {
        base.OnEnterHitChara(hitData);
        DestroyBullet();
    }
}
