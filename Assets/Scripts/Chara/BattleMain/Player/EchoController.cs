using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoController : PlayerController
{

    [Header("Echo専用 ショットに関するパラメータ")]
    [SerializeField, Range(0f, 1f)]
    private float[] m_ShotInterval;

    private float ShotInterval;

    //[SerializeField]
    //private float m_ShotIntervalDecrease;

    private float shotDelay;

    [SerializeField]
    private Transform[] m_MainShotPosition;

    [SerializeField]
    private Transform[] m_SubShotLv1Position;

    private bool m_SubShotLv1CanShot;

    [SerializeField]
    private Transform[] m_SubShotLv2Position;

    private bool m_SubShotLv2CanShot;

    [Header("Echo専用 衝撃波に関するパラメータ")]
    [SerializeField]
    private int m_RadiateDirection;

    [SerializeField]
    private float m_WaveRad;

    [SerializeField]
    private float m_RotateOffset;

    [SerializeField]
    private int[] m_MaxHitCount;

    private int maxHitCount;

    private Dictionary<int, int> RootBulletIndex;

    [SerializeField]
    private int m_MaxBombAmount;

    private int currentBombAmount;

    [SerializeField]
    private float m_BombInterval;       // ボムの演出の長さで適宜変えてね

    private float bombDelay;

    public int GetMaxHitCount()
    {
        return maxHitCount;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        ShotInterval = m_ShotInterval[GetLevel()];
        maxHitCount = m_MaxHitCount[GetLevel()];
        RootBulletIndex = new Dictionary<int, int>();
        currentBombAmount = m_MaxBombAmount;
        bombDelay = m_BombInterval;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        shotDelay += Time.deltaTime;
        bombDelay += Time.deltaTime;
        UpdateShotLevel(GetLevel());
    }

    public override void ShotBullet(InputManager.E_INPUT_STATE state)
    {
        if (shotDelay >= ShotInterval)
        {
            for (int i = 0; i < m_MainShotPosition.Length; i++)
            {
                var shotParam = new BulletShotParam(this);
                shotParam.Position = m_MainShotPosition[i].transform.position - transform.parent.position;
                BulletController.ShotBullet(shotParam);
            }

            if (m_SubShotLv1CanShot)
            {
                //Debug.Log("Fire!@Lv1");
                for(int i=0; i < m_SubShotLv1Position.Length; i++)
                {
                    var shotParam = new BulletShotParam(this);
                    shotParam.Position = m_SubShotLv1Position[i].transform.position - transform.parent.position;
                    shotParam.Rotation = m_SubShotLv1Position[i].transform.eulerAngles;
                    BulletController.ShotBullet(shotParam);
                }
            }

            if (m_SubShotLv2CanShot)
            {
                //Debug.Log("Fire!@Lv2");
                for (int i=0; i < m_SubShotLv2Position.Length; i++)
                {
                    var shotParam = new BulletShotParam(this);
                    shotParam.Position = m_SubShotLv2Position[i].transform.position - transform.parent.position;
                    shotParam.Rotation = m_SubShotLv2Position[i].transform.eulerAngles;
                    BulletController.ShotBullet(shotParam);
                }
            }

            shotDelay = 0;
        }
    }

    private void UpdateShotLevel(int level)
    {
        //Debug.Log(level);

        ShotInterval = m_ShotInterval[level - 1];
        maxHitCount = m_MaxHitCount[level - 1];

        if (level >= 3)
        {
            m_SubShotLv1CanShot = true;
            m_SubShotLv2CanShot = true;          
        }
        else if (level >= 2)
        {
            m_SubShotLv1CanShot = true;
            m_SubShotLv2CanShot = false;
        }
        else
        {
            m_SubShotLv1CanShot = false;
            m_SubShotLv2CanShot = false;
        }

        for(int i=0; i <m_SubShotLv1Position.Length; i++)
        {
            m_SubShotLv1Position[i].gameObject.SetActive(m_SubShotLv1CanShot);
        }

        for (int i = 0; i < m_SubShotLv2Position.Length; i++)
        {
            m_SubShotLv2Position[i].gameObject.SetActive(m_SubShotLv2CanShot);
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

        if (GetRootBulletHitCount(index) < maxHitCount)
        {
            for (int i = 0; i < m_RadiateDirection; i++)
            {
                float yAngle = (2 * Mathf.PI / m_RadiateDirection) * (i % 2 == 0 ? i : -i) * Mathf.Rad2Deg;
                var shotParam = new BulletShotParam(this);
                var currentTime = Time.deltaTime;
                shotParam.Position = new Vector3(centerLocalPosition.x + m_WaveRad * Mathf.Cos(currentTime), centerLocalPosition.y, centerLocalPosition.z + m_WaveRad * Mathf.Sin(currentTime));
                shotParam.Rotation = new Vector3(0, yAngle + m_RotateOffset, 0);
                shotParam.BulletIndex = 1;
                shotParam.OrbitalIndex = 0;
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

    public override void ShotBomb(InputManager.E_INPUT_STATE state)
    {
        if (currentBombAmount > 0 && bombDelay >= m_BombInterval)
        {
            currentBombAmount--;
            Debug.Log(string.Format("{0}Bomb!!!@{1}, Last{2}", this.name, this.transform.position, this.currentBombAmount));
            bombDelay = 0;
        }
    }
}
