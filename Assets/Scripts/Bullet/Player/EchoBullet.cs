using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
    [SerializeField]
    private bool m_IsRoot;

    private int index;

    public int GetIndex()
    {
        return index;
    }

    public override void OnHitCharacter(CharaController chara)
    {
        var controller = (EchoController)GetBulletOwner();

        if (!m_IsRoot)
        {
            index = EchoBulletIndexGenerater.GenerateBulletIndex();

            if(index % controller.GetMaxHitCount() != 0)
            {
                controller.ShotWaveBullet(chara.transform.localPosition);
            }
        }
        else
        {
            controller.ShotWaveBullet(chara.transform.localPosition);
        }
    }
}
