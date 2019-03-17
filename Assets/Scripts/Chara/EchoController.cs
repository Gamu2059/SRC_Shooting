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
    private bool m_CanRadShot;

    private Vector3 latestPosition;

	protected override void Awake()
	{
		base.Awake();
		initialShotInterval = m_ShotInterval;
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
        RadiateShot();
	}

	public override void ShotBullet()
	{
		if( shotDelay >= m_ShotInterval )
		{
			for( int i = 0; i < m_MainShotPosition.Length; i++ )
			{
				var shotParam = new BulletShotParam( this );
				shotParam.Position = m_MainShotPosition[i].transform.GetMoveObjectHolderBasePosition();
                shotParam.OrbitalIndex = 2;
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
		}
		else if( level >= 2 )
		{
			m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease;
		}
		else
		{
			m_ShotInterval = initialShotInterval;
		}
	}

    public void ReadyRadiateShot(Vector3 position)
    {
        latestPosition = position;
        m_CanRadShot = true;
    }

    private void RadiateShot()
    {
        if (!m_CanRadShot)
        {
            return;
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                float angleRad = (Mathf.PI / 4) * i;
                float yAngle = (Mathf.PI / 4) * i * Mathf.Rad2Deg;

                var shotParam = new BulletShotParam(this);
                shotParam.Position = latestPosition;
                shotParam.Rotation = new Vector3(0, yAngle, 0);
                shotParam.BulletIndex = 1;
                shotParam.OrbitalIndex = 3;
                BulletController.ShotBullet(shotParam);
            }

            m_CanRadShot = false;
        }
    }
}
