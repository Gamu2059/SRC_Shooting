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

    public void Awake()
    {
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
    }

    public override Bullet ShotBullet(int bulletIndex = 0, int bulletParamIndex = 0)
    {
        return base.ShotBullet(bulletIndex, bulletParamIndex);

        /*if (shotDelay >= m_ShotInterval)
        {
            Bullet b = GetOriginalBullet(0);

            for(int i=0; i< m_MainShotPosition.Length; i++)
            {
                Bullet bullet = GetPoolBullet(0);
                bullet.ShotBullet(this, m_MainShotPosition[i].position, m_MainShotPosition[i].eulerAngles, b.transform.localScale, bulletIndex, m_BulletParams[0], 1);
            }
            shotDelay = 0;
        }*/
    }

    public override void ShotBomb(int bombIndex = 0)
    {
        base.ShotBomb(bombIndex);
    }

    private void UpdateShotInterval(int level) {

    }
}
