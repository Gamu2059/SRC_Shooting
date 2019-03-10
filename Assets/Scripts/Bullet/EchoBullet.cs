using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : Bullet
{
    [SerializeField]
    private int m_HitCount;
    
    public override void OnHitCharacter(CharaControllerBase chara)
    {
        EchoController echoController = (EchoController)this.GetBulletOwner();
        echoController.ReadyShotDiffusionBullet(chara, ++m_HitCount);
        base.OnHitCharacter(chara);
    }

}
