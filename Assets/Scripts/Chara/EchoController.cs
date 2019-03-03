using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoController : PlayerController
{
    [SerializeField, Range(0f, 1f)]
    private float m_ShotInterval;

    private float initialShotInterval;

    [SerializeField]
    private float m_ShotIntervalDecrease;

    private float shotDelay;

    [SerializeField]
    private Transform[] m_MainShotPosition;

    protected override void Awake()
    {
        base.Awake();
        
        initialShotInterval = m_ShotInterval;
        OnAwake();
        
    }

    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        shotDelay += Time.deltaTime;
        UpdateShotInterval(GetLevel());
    }

    public override Bullet ShotBullet(int bulletIndex = 0, int bulletParamIndex = 0)
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
                Bullet bullet = GetPoolBullet(bulletIndex);
                bullet.ShotBullet(this, m_MainShotPosition[i].position, m_MainShotPosition[i].eulerAngles, mainBullet.transform.localScale, bulletIndex, bulletParam, 0);
            }

            shotDelay = 0;
        }

        return null;
    }

    public override void ShotBomb(int bombIndex = 0)
    {
        base.ShotBomb(bombIndex);
    }

    private void UpdateShotInterval(int level) {
        if (level >= 3)
        {
            m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease * 2;
        }
        else if (level >= 2)
        {
            m_ShotInterval = initialShotInterval - m_ShotIntervalDecrease;
        }
        else
        {
            m_ShotInterval = initialShotInterval;
        }
    }


}
