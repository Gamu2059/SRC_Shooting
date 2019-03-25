using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
    [SerializeField]
    private bool m_IsRoot;

    public override void OnHitCharacter(CharaController chara)
    {
        if (this.m_IsRoot)
        {
            var controller = (EchoController)GetBulletOwner();
            controller.ShotWaveBullet(chara.transform.localPosition);
        }       
    }
}
