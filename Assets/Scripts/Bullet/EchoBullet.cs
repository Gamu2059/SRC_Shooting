using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : Bullet
{
    [SerializeField]
    private EchoController m_Parent;

    public override void OnHitCharacter(CharaControllerBase chara)
    {
        m_Parent.ReadyShotDiffusionBullet(chara);
        base.OnHitCharacter(chara);
    }

    public void SetShooter(EchoController echoController)
    {
        m_Parent = echoController;
    }
}
