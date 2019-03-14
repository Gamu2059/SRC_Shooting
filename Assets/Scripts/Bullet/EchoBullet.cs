using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
	[SerializeField]
	private EchoController m_Parent;

	[SerializeField]
	private int m_HitCount;

	public override void OnHitCharacter( CharaControllerBase chara )
	{
		if( m_HitCount < m_Parent.GetMaxHitCount() )
		{
			m_Parent.ReadyShotDiffusionBullet( chara, m_HitCount );
		}
		else
		{
			m_HitCount = 0;
		}

		base.OnHitCharacter( chara );
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
