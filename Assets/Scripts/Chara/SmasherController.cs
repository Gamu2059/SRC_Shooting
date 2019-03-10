using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherController : PlayerController
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
	private Transform[] m_SubShotLv1Position;

	private bool m_SubShotLv1CanShot;

	[SerializeField]
	private Transform[] m_SubShotLv2Position;

	private bool m_SubShotLv2CanShot;

	[SerializeField]
	private float m_SubShotLv2Radius;

	[SerializeField]
	private float m_SubShotLv2Speed;

	private float m_SubShotLv2AngleDeg;

	[SerializeField]
	private bool m_IsSpinTurn;


	//遊び用
	//[SerializeField]
	private float m_SubShotLv2MaxSpeed = 25f;

	//[SerializeField]
	private float step = 1f;

	protected void Awake()
	{
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

		// 遊び用
		//UpdateSpeed();
		//UpdateSubShot();
	}

	public override Bullet ShotBullet( int bulletIndex = 0, int bulletParamIndex = 0 )
	{
		return base.ShotBullet( bulletIndex, bulletParamIndex );
		/*
		if(shotDelay >= m_ShotInterval)
		{
		    Bullet Main = GetOriginalBullet(0);
		    Bullet Sub2 = GetOriginalBullet(1);

		     for (int i = 0; i < m_MainShotPosition.Length; i++)
		    {
		        Bullet bullet = GetPoolBullet(0);
		        bullet.ShotBullet(this, m_MainShotPosition[i].position, m_MainShotPosition[i].eulerAngles,  Main.transform.localScale, bulletIndex, m_BulletParams[0], 0);
		    }

		    if (m_SubShotLv1CanShot)
		    {
		        for (int i = 0; i < m_SubShotLv1Position.Length; i++)
		        {
		            Bullet bullet = GetPoolBullet(0);
		            bullet.ShotBullet(this, m_SubShotLv1Position[i].position, m_SubShotLv1Position[i].eulerAngles, Main.transform.localScale, bulletIndex, m_BulletParams[0], 0);
		        }
		    }

		    if (m_SubShotLv2CanShot)
		    {
		        for (int i = 0; i < m_SubShotLv2Position.Length; i++)
		        {
		            Bullet bullet = GetPoolBullet(1);
		            bullet.ShotBullet(this, m_SubShotLv2Position[i].position, Vector3.zero, Sub2.transform.localScale, bulletIndex, m_BulletParams[0], 4);


		            //Bullet bulletR = GetPoolBullet(1);
		            //Bullet bulletL = GetPoolBullet(1);
		            //bulletR.ShotBullet(this, m_SubShotLv2Position[i].position + new Vector3(0.5f, 0, 0), Vector3.zero, Sub2.transform.localScale, bulletIndex, m_BulletParams[0], 4);
		            //bulletL.ShotBullet(this, m_SubShotLv2Position[i].position + new Vector3(-0.5f, 0, 0), Vector3.zero, Sub2.transform.localScale, bulletIndex, m_BulletParams[0], 4);


		            //bullet.ShotBullet(this, m_SubShotLv2[i].transform.position, m_SubShotLv2[i].transform.eulerAngles, b.transform.localScale, bulletIndex, m_BulletParams[0], 0);
		        }
		    }

		    shotDelay = 0;
		 */

	}
}

/*
private void UpdateSpeed()
{

    //速度が直線的に変化するバージョン
    //m_SubShotLv2Speed -= step * Time.deltaTime;
    //if(Mathf.Abs(m_SubShotLv2Speed) >= m_SubShotLv2MaxSpeed)
   // {
   //     m_SubShotLv2Speed = (m_SubShotLv2Speed < 0 ? -m_SubShotLv2MaxSpeed : m_SubShotLv2MaxSpeed);
   //     step = -step;
    //}


    // 速度が正弦波に従って変化するバージョン
    m_SubShotLv2Speed = m_SubShotLv2MaxSpeed * Mathf.Sin(Time.time * step);
}
 */



/*
    public override void UpdateShotLevel(int level)
    {
        if (level >= 3)
        {
            m_SubShotLv1CanShot = true;
            m_SubShotLv2CanShot = true;
            m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease * 2;
        }
        else if (level >= 2)
        {
            m_SubShotLv2CanShot = false;
            m_SubShotLv1CanShot = true;
            m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease * 2;
        }
        else
        {
            m_SubShotLv1CanShot = false;
            m_SubShotLv2CanShot = false;
            m_ShotInterval = initialShotInterval;
        }

        for (int i = 0; i < m_SubShotLv1Position.Length; i++)
        {
            m_SubShotLv1Position[i].gameObject.SetActive(m_SubShotLv1CanShot);
        }

        for (int i = 0; i < m_SubShotLv2Position.Length; i++)
        {
            m_SubShotLv2Position[i].gameObject.SetActive(m_SubShotLv2CanShot);
        }
    }
 */


/*
private void UpdateSubShot()
{
    if (m_SubShotLv2CanShot)
    {
        m_SubShotLv2AngleDeg += m_SubShotLv2Speed * Time.deltaTime;
        m_SubShotLv2AngleDeg %= Mathf.PI * 2;
        float unitAngle = Mathf.PI * 2 / m_SubShotLv2Position.Length;
        for (int i = 0; i < m_SubShotLv2Position.Length; i++)
        {
            float angle = unitAngle * i + m_SubShotLv2AngleDeg;

            if (m_IsSpinTurn)
            {
                angle *= (i % 2 == 0 ? -1 : 1);
            }

            float x = m_SubShotLv2Radius * Mathf.Cos(-angle);
            float z = m_SubShotLv2Radius * Mathf.Sin(-angle);

            m_SubShotLv2Position[i].GetComponent<Transform>().localPosition = new Vector3(x, 0, z);
            m_SubShotLv2Position[i].GetComponent<Transform>().LookAt(transform);
        }
    }
}
 */

