using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoController : PlayerController
{

    [Header("Echo専用 ショットに関するパラメータ")]
    [SerializeField, Range( 0f, 1f )]
	private float m_ShotInterval;

	private float initialShotInterval;

	[SerializeField]
	private float m_ShotIntervalDecrease;

	private float shotDelay;

	[SerializeField]
	private Transform[] m_MainShotPosition;

    [Header("Echo専用 衝撃波に関するパラメータ")]
    [SerializeField]
    private int m_RadiateDirection;

    [SerializeField]
    private float m_WaveRad;

    [SerializeField]
    private float m_RotateOffset;

    [SerializeField]
    private int m_MaxHitCount;

    public int GetMaxHitCount()
    {
        return m_MaxHitCount;
    }

    protected override void Awake()
	{
		base.Awake();
		OnAwake();
	}

	protected override void OnAwake()
	{
        initialShotInterval = m_ShotInterval;
        base.OnAwake();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		shotDelay += Time.deltaTime;
		UpdateShotLevel( GetLevel() );
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

    public void ShotWaveBullet(Vector3 centerLocalPosition)
    {
        for (int i=0; i< m_RadiateDirection; i++)
        {
            float yAngle = (2 * Mathf.PI / m_RadiateDirection) * (i % 2 == 0 ? i : -i) * Mathf.Rad2Deg;
            var shotParam = new BulletShotParam(this);
            shotParam.Position = new Vector3(centerLocalPosition.x + m_WaveRad * Mathf.Sin(Time.deltaTime), 0, centerLocalPosition.z + m_WaveRad * Mathf.Cos(Time.deltaTime));
            shotParam.Rotation = new Vector3(0, yAngle + m_RotateOffset, 0);
            shotParam.BulletIndex = 1;
            shotParam.OrbitalIndex = 3;
            BulletController.ShotBullet(shotParam);
        }
    } 
         
}
