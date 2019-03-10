using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : Bullet
{
    /*
    [SerializeField]
    private EchoController m_Parent;
    */
    [SerializeField]
    private int m_HitCount;
    

    public override void OnHitCharacter(CharaControllerBase chara)
    {
        Debug.Log(string.Format("Hit! @ {0}", chara.transform.position));
        EchoController echoController = (EchoController)this.GetBulletOwner();
        echoController.ReadyShotDiffusionBullet(chara, ++m_HitCount);
        base.OnHitCharacter(chara);
    }

    /*
    public void SetShooter(EchoController echoController, int count)
    {
        m_Parent = echoController;
        m_HitCount = count;
    }

    public void InitializeBullet(EchoController echoController)
    {
        m_Parent = echoController;
        m_HitCount = 0;
    }
    */

}
