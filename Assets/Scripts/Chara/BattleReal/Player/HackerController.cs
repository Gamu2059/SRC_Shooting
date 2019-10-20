using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerController : BattleRealPlayerController
{
    [SerializeField]
    private Transform[] m_MainShotPosition;

    [SerializeField, Range(0f, 1f)]
    private float m_ShotInterval;

    /// <summary>
    /// コマンドイベントの再発動にかかるインターバル
    /// </summary>
    [SerializeField]
    private float m_CommandEventInterval;

    private float shotDelay;

    private BulletController m_Laser;

    public float GetCommandEventInterval()
    {
        return m_CommandEventInterval;
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
        shotDelay += Time.deltaTime;
    }

    public override void ShotBullet()
    {
        if (shotDelay >= m_ShotInterval)
        {
            for (int i = 0; i < m_MainShotPosition.Length; i++)
            {
                var shotParam = new BulletShotParam(this);
                shotParam.Position = m_MainShotPosition[i].transform.position;
                BulletController.ShotBullet(shotParam);
            }
            shotDelay = 0;
        }
    }

    public override void ShotLaser() 
    {
        base.ShotLaser();

        if (m_Laser != null && m_Laser.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED)
        {
            return;
        }

        var param = new BulletShotParam(this);
        param.Position = m_MainShotPosition[0].transform.position;
        m_Laser = BulletController.ShotBullet(param, true);
    }
}
