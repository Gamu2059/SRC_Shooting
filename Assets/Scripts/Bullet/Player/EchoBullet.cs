using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
    [SerializeField]
    private bool m_IsRoot;

    static int index;

    public override void OnHitCharacter(CharaController chara)
    {
        var controller = (EchoController)GetBulletOwner();
        if (m_IsRoot)
        {
            index = EchoBulletIndexGenerater.GenerateBulletIndex();
        }
        controller.ShotWaveBullet(index, chara.transform.position);

        Debug.Log(string.Format("index={0}", index));
    }

    public static void setIndex(int n)
    {
        index = n;
    }
}
