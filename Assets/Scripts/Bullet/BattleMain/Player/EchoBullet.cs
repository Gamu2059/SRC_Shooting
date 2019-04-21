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
        if (m_HitCount < m_Parent.GetMaxHitCount())
        {
            m_Parent.ReadyShotDiffusionBullet(targetChara, m_HitCount);
        }
        else
        {
            m_HitCount = 0;
        }

        base.HitChara(targetChara, attackData, targetData);
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
