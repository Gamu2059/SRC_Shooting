using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherController : PlayerController
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ShotBullet( int bulletIndex = 0)
    { 
        Bullet bullet = GetPoolBullet(0);
        bullet.transform.position = transform.position;
        bullet.transform.eulerAngles = Vector3.up * m_NowRad * Mathf.Rad2Deg;
        bullet.transform.Translate(m_ShotPosOffset * Mathf.Cos(m_NowRad), 0, m_ShotPosOffset * Mathf.Sin(m_NowRad));
        bullet.ShotBullet();
    }
}
