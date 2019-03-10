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


    protected override void Awake()
    {
        initialShotInterval = m_ShotInterval;
        OnAwake();
    }

    public override void OnUpdate()
    {
        shotDelay += Time.deltaTime;
        base.OnUpdate();
        UpdateShotLevel(GetLevel());
        UpdateSubShot();
        
    }

    public override Bullet ShotBullet(int bulletIndex, int bulletParamIndex)
    {
        if(shotDelay >= m_ShotInterval)
        {
            if (bulletIndex < 0 || bulletIndex >= m_BulletPrefabs.Length)
            {
                return null;
            }

            if (bulletParamIndex < 0 || bulletParamIndex >= m_BulletParams.Length)
            {
                return null;
            }

            BulletParam bulletParam = m_BulletParams[bulletParamIndex];

            if (bulletParam == null)
            {
                return null;
            }

            GameObject mainBullet = m_BulletPrefabs[bulletIndex].gameObject;

            for (int i = 0; i < m_MainShotPosition.Length; i++)
            {
                var shotParam = new BulletShotParam();
                shotParam.Position = m_MainShotPosition[i].position;
                Bullet.ShotBullet(shotParam);
                //Bullet bullet = GetPoolBullet(bulletIndex);
                //bullet.ShotBullet(this, m_MainShotPosition[i].position, m_MainShotPosition[i].eulerAngles, mainBullet.transform.localScale, bulletIndex, bulletParam, 0);
            }

            if (m_SubShotLv1CanShot)
            {
                for (int i = 0; i < m_SubShotLv1Position.Length; i++)
                {
                    var shotParam = new BulletShotParam();
                    shotParam.Position = m_SubShotLv1Position[i].position;
                    Bullet.ShotBullet(shotParam);
                    //Bullet bullet = GetPoolBullet(bulletIndex);
                    //bullet.ShotBullet(this, m_SubShotLv1Position[i].position, m_SubShotLv1Position[i].eulerAngles, mainBullet.transform.localScale, bulletIndex, bulletParam, 0);
                }
            }

            if (m_SubShotLv2CanShot)
            {
                GameObject Sub2 = m_BulletPrefabs[1].gameObject;

                for (int i = 0; i < m_SubShotLv2Position.Length; i++)
                {
                    var shotParam = new BulletShotParam();
                    shotParam.Position = m_SubShotLv2Position[i].position;
                    Bullet.ShotBullet(shotParam);
                    //Bullet bullet = GetPoolBullet(bulletIndex + 1);
                    //bullet.ShotBullet(this, m_SubShotLv2Position[i].position, Vector3.zero, Sub2.transform.localScale, bulletIndex, bulletParam, 1);
                }

            }
            shotDelay = 0;
        }
        
        return null;
     
    }

    public void UpdateShotLevel(int level)
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




    private void UpdateSubShot()
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





     
     

