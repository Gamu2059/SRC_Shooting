using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoController : PlayerController
{
	[SerializeField, Range( 0f, 1f )]
	private float m_ShotInterval;

	private float initialShotInterval;

	[SerializeField]
	private float m_ShotIntervalDecrease;

	private float shotDelay;

	[SerializeField]
	private Transform[] m_MainShotPosition;

	[SerializeField]
	private float m_DiffusionRadius;

	[SerializeField]
	private bool m_CanShotWave;

	[SerializeField]
	private CharaController m_LatestHitCharacter;

	[SerializeField]
	private int m_LatestHitCount;

	[SerializeField]
	private int m_MaxHitCount;

	private int initialMaxHitCount;

	[SerializeField]
	private int m_MaxHitCountIncrease;

	protected override void Awake()
	{
		base.Awake();
		initialShotInterval = m_ShotInterval;
		initialMaxHitCount = m_MaxHitCount;
		OnAwake();
	}

	protected override void OnAwake()
	{
		base.OnAwake();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		shotDelay += Time.deltaTime;
		UpdateShotLevel( GetLevel() );
		ShotDiffusinBullet();
	}

	public override void ShotBullet()
	{
		if( shotDelay >= m_ShotInterval )
		{
			for( int i = 0; i < m_MainShotPosition.Length; i++ )
			{
				var shotParam = new BulletShotParam( this );
				shotParam.Position = m_MainShotPosition[i].position;
				BulletController.ShotBullet( shotParam );
				//EchoBullet bullet = (EchoBullet)GetPoolBullet(bulletIndex);
				//bullet.InitializeBullet(this);
				//bullet.ShotBullet(this, m_MainShotPosition[i].position, m_MainShotPosition[i].eulerAngles, mainBullet.transform.localScale, bulletIndex, bulletParam, -1);
			}

			shotDelay = 0;
		}
	}

	public override void ShotBomb()
	{
		base.ShotBomb();
	}

	private void UpdateShotLevel( int level )
	{

		if( level >= 3 )
		{
			m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease * 2;
			m_MaxHitCount = initialMaxHitCount + m_MaxHitCountIncrease * 2;
		}
		else if( level >= 2 )
		{
			m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease;
			m_MaxHitCount = initialMaxHitCount + m_MaxHitCountIncrease;
		}
		else
		{
			m_ShotInterval = initialShotInterval;
			m_MaxHitCount = initialMaxHitCount;
		}
	}

	public void ReadyShotDiffusionBullet( CharaController chara, int count )
	{
		if( !m_CanShotWave )
		{
			m_CanShotWave = true;
		}

		m_LatestHitCharacter = chara;
		m_LatestHitCount = count;
	}

	private void ShotDiffusinBullet( int bulletIndex = 1, int bulletParamIndex = 0 )
	{
		if( m_CanShotWave )
		{
			if( m_LatestHitCharacter == null )
			{
				return;
			}

			BulletParam bulletParam = GetBulletParam( bulletParamIndex );

			for( int i = 0; i < 4; i++ )
			{
				float angleRad = ( Mathf.PI / 2 ) * i;
				float yAngle = 90f * Direction4( i );

				var shotParam = new BulletShotParam();
				shotParam.Position = new Vector3( Mathf.Cos( angleRad ), 0, Mathf.Sin( angleRad ) ) * m_DiffusionRadius;
				shotParam.Rotation = m_LatestHitCharacter.transform.eulerAngles + new Vector3( 0, yAngle, 0 );
				BulletController.ShotBullet( shotParam );
				//EchoBullet bullet = (EchoBullet)GetPoolBullet(bulletIndex);
				//bullet.SetShooter(this, ++m_LatestHitCount);
				//bullet.ShotBullet(this, m_LatestHitCharacter.transform.position + m_DiffusionRadius * new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)), m_LatestHitCharacter.transform.eulerAngles + new Vector3(0, yAngle, 0), bulletPrefab.transform.localScale, bulletIndex, bulletParam, -1);
			}

			m_CanShotWave = false;
		}
	}

	private int Direction4( int i )
	{
		if( i < 1 )
		{
			return -i;

		}
		else if( i < 2 )
		{
			return 3 * i - 4;
		}
		else
		{
			return - i + 4;
		}
	}

	public int GetMaxHitCount()
	{
		return m_MaxHitCount;
	}
}
