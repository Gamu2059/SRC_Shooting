using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
    [SerializeField]
    private bool m_IsRoot;

    static int index;

    public override void HitChara(CharaController targetChara, ColliderData attackData, ColliderData targetData)
    {
        var controller = (EchoController)GetBulletOwner();
        if (m_IsRoot)
        {
            index = EchoBulletIndexGenerater.GenerateBulletIndex();
        }
        controller.ShotWaveBullet(index, targetChara.transform.localPosition);
    }

    public static void SetIndex(int n)
    {
        index = n;
    }

    public int GetRootIndex()
    {
        return index;
    }
}
