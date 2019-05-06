using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerBullet : BulletController
{
    public override void HitChara(CharaController targetChara, ColliderData attackData, ColliderData targetData)
    {
        base.HitChara(targetChara, attackData, targetData);
        DestroyBullet();
        BattleManager.Instance.TransitionBattleCommand();
    }
}
