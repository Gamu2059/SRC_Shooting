using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherController : PlayerController
{
    [SerializeField]
    private float m_ShotInterval;

    private float shotDelay;

    [SerializeField]
    private Transform[] m_MainShotPosition;

    [SerializeField]
    private GameObject[] m_SubShotLv1;

    private bool m_SubShotLv1CanShot;

    [SerializeField]
    private GameObject[] m_SubShotLv2;

    private bool m_SubShotLv2CanShot;

    [SerializeField, Range(1, 3)]
    private int m_ShotLevel;

    [SerializeField]
    private float m_SubShotLv2Radius;

    [SerializeField]
    private float m_SubShotLv2Speed;

    private float m_SubShotLv2AngleRad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shotDelay += Time.deltaTime;
        UpdateSubShot();
    }

    public override void ShotBullet( int bulletIndex = 0)
    { 
        if(shotDelay >= m_ShotInterval)
        {
            // メインショット
            for (int i = 0; i < m_MainShotPosition.Length; i++)
            {
                Bullet bullet = GetPoolBullet(0);
                bullet.ShotBullet();
                bullet.transform.position = m_MainShotPosition[i].position;
            }
            // サブLv1
            if (m_SubShotLv1CanShot)
            {
                for (int i = 0; i < m_SubShotLv1.Length; i++)
                {
                    Bullet bullet = GetPoolBullet(0);
                    bullet.ShotBullet();
                    bullet.transform.position = m_SubShotLv1[i].GetComponent<Transform>().position;
                }
            }
            // サブLv2
            if (m_SubShotLv2CanShot)
            {
                for (int i = 0; i < m_SubShotLv2.Length; i++)
                {
                    Bullet bullet = GetPoolBullet(0);
                    bullet.ShotBullet();
                    bullet.transform.position = m_SubShotLv2[i].GetComponent<Transform>().position;

                }
            }
            shotDelay = 0;
        }
    }

    private void UpdateSubShot()
    {
        if (m_ShotLevel >= 3)
        {
            m_SubShotLv1CanShot = true;
            m_SubShotLv2CanShot = true;
        }
        else if (m_ShotLevel >= 2)
        {
            m_SubShotLv2CanShot = false;
            m_SubShotLv1CanShot = true;
        }
        else
        {
            m_SubShotLv1CanShot = false;
            m_SubShotLv2CanShot = false;
        }

        for (int i = 0; i < m_SubShotLv1.Length; i++)
        {
            m_SubShotLv1[i].SetActive(m_SubShotLv1CanShot);
        }

        for (int i = 0; i < m_SubShotLv2.Length; i++)
        {
            m_SubShotLv2[i].SetActive(m_SubShotLv2CanShot);
        }



        m_SubShotLv2AngleRad += m_SubShotLv2Speed * Time.deltaTime;
        m_SubShotLv2AngleRad %= Mathf.PI * 2;
        float unitAngle = Mathf.PI * 2 / m_SubShotLv2.Length;

        if (m_SubShotLv2CanShot)
        {
            for (int i = 0; i < m_SubShotLv2.Length; i++)
            {
                float angle = unitAngle * i + m_SubShotLv2AngleRad;
                float x = m_SubShotLv2Radius * Mathf.Cos(-angle);
                float z = m_SubShotLv2Radius * Mathf.Sin(-angle);
                m_SubShotLv2[i].GetComponent<Transform>().localPosition = new Vector3(x, 0, z);
                m_SubShotLv2[i].GetComponent<Transform>().LookAt(transform);
            }
        }
    }
}
