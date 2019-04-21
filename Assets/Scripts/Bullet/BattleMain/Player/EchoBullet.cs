using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
	[SerializeField]
	private EchoController m_Parent;

	[SerializeField]
	private int m_HitCount;

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

	public void SetShooter( EchoController echoController, int count )
	{
		m_Parent = echoController;
		m_HitCount = count;
	}

	public void InitializeBullet( EchoController echoController )
	{
		m_Parent = echoController;
		m_HitCount = 0;
	}

}
