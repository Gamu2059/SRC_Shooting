#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
    [SerializeField]
    private bool m_IsRoot;

    [SerializeField]
    private int m_Index;

    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private float m_AnimInterval;

    [SerializeField]
    private float m_TargetScale;

    [SerializeField]
    private float m_AnimLerp;

    private float m_AnimTimeCount;

    public override void OnStart()
    {
        base.OnStart();

        m_AnimTimeCount = 0;
        m_SpriteRenderer.transform.localScale = Vector3.one;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_AnimTimeCount += Time.deltaTime;
        if (m_AnimTimeCount >= m_AnimInterval)
        {
            m_AnimTimeCount = 0;
            m_SpriteRenderer.transform.localScale = Vector3.one;
            return;
        }

        float scale = m_SpriteRenderer.transform.localScale.x;
        float nextScale = (m_TargetScale - scale) * m_AnimLerp + scale;
        m_SpriteRenderer.transform.localScale = Vector3.one * nextScale;

        if (!m_IsRoot && GetScale().x <= 0)
        {
            DestroyBullet();
        }
    }

    //public override void HitChara(CharaController targetChara, ColliderData attackData, ColliderData targetData)
    //{
    //    if (m_IsRoot)
    //    {
    //        m_Index = EchoBulletIndexGenerater.Instance.GenerateBulletIndex();
    //    }

    //    if (EchoBulletIndexGenerater.Instance.IsRegisteredChara(m_Index, targetChara))
    //    {
    //        return;
    //    }

    //    EchoBulletIndexGenerater.Instance.RegisterHitChara(m_Index, targetChara);
    //    var controller = (EchoController)GetBulletOwner();
    //    controller.ShotWaveBullet(m_Index, targetChara.transform.localPosition);
    //    DestroyBullet();
    //}

    public void SetIndex(int n)
    {
        m_Index = n;
    }

    public int GetRootIndex()
    {
        return m_Index;
    }
}
