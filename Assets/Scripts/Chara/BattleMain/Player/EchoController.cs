using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoController : PlayerController
{

    [Header("Echo専用 ショットに関するパラメータ")]
    [SerializeField, Range(0f, 1f)]
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

    private Dictionary<int, int> RootBulletIndex;

    public int GetMaxHitCount()
    {
        return m_MaxHitCount;
    }

    protected override void Awake()
    {
        initialShotInterval = m_ShotInterval;
        RootBulletIndex = new Dictionary<int, int>();
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
        UpdateShotLevel(GetLevel());
    }

    public override void ShotBullet()
    {
        if (shotDelay >= m_ShotInterval)
        {
            for (int i = 0; i < m_MainShotPosition.Length; i++)
            {
                var shotParam = new BulletShotParam(this);
                shotParam.Position = m_MainShotPosition[i].transform.position - transform.parent.position;
                shotParam.OrbitalIndex = 2;
                BulletController.ShotBullet(shotParam);
            }

            shotDelay = 0;
        }
    }

    public override void ShotBomb()
    {

        base.ShotBomb();
    }

    private void UpdateShotLevel(int level)
    {
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

    public void ShotWaveBullet(int index, Vector3 centerLocalPosition)
    {
        if (!IndexExistP(index))
        {
            SetNewIndex(index);
        }
        else
        {
            UpdateIndex(index);
        }

        if (GetRootBulletHitCount(index) < m_MaxHitCount)
        {
            for (int i = 0; i < m_RadiateDirection; i++)
            {
                float yAngle = (2 * Mathf.PI / m_RadiateDirection) * (i % 2 == 0 ? i : -i) * Mathf.Rad2Deg;
                var shotParam = new BulletShotParam(this);
                var currentTime = Time.deltaTime;
                shotParam.Position = new Vector3(centerLocalPosition.x + m_WaveRad * Mathf.Cos(currentTime), centerLocalPosition.y, centerLocalPosition.z + m_WaveRad * Mathf.Sin(currentTime));
                shotParam.Rotation = new Vector3(0, yAngle + m_RotateOffset, 0);
                shotParam.BulletIndex = 1;
                shotParam.OrbitalIndex = 3;
                var bullet = BulletController.ShotBullet(shotParam) as EchoBullet;
                if (bullet != null)
                {
                    bullet.SetIndex(index);
                }
            }
        }
    }

    private bool IndexExistP(int index)
    {
        return RootBulletIndex.ContainsKey(index);
    }

    private void SetNewIndex(int index)
    {
        RootBulletIndex.Add(index, 0);
    }

    private void UpdateIndex(int index)
    {
        RootBulletIndex[index]++;
    }

    private int GetRootBulletHitCount(int index)
    {
        return RootBulletIndex[index];
    }
}
