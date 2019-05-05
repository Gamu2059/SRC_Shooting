using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
    [SerializeField]
    private bool m_IsRoot;

    [SerializeField]
    private int m_Index;

    public override void HitChara(CharaController targetChara, ColliderData attackData, ColliderData targetData)
    {
        if (m_IsRoot)
        {
            m_Index = EchoBulletIndexGenerater.Instance.GenerateBulletIndex();
        }

        if (EchoBulletIndexGenerater.Instance.IsRegisteredChara(m_Index, targetChara))
        {
            return;
        } else
        {
            EchoBulletIndexGenerater.Instance.RegisterHitChara(m_Index, targetChara);
        }

        var controller = (EchoController)GetBulletOwner();
        controller.ShotWaveBullet(m_Index, targetChara.transform.localPosition);
        DestroyBullet();
        Debug.LogFormat("echo bullet name : {0}", GetBulletGroupId());
    }

    public void SetIndex(int n)
    {
        m_Index = n;
    }

    public int GetRootIndex()
    {
        return m_Index;
    }
}
