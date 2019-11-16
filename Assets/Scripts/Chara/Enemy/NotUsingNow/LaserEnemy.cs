#pragma warning disable 0649

using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class LaserEnemy : BattleRealEnemyController

{



    [SerializeField]

    private Vector3 m_BasePos;



    [SerializeField]

    private float m_Amplitude;



    [SerializeField]

    private float m_PeriodSpeed;



    [SerializeField]

    private Vector3 m_Axis;



    [SerializeField]

    private float m_ShotInterval;



    private float m_ShotTime;



    private float m_NowRad;



    public override void OnStart()

    {

        base.OnStart();

    }



    public override void OnUpdate()

    {

        base.OnUpdate();



        m_NowRad += m_PeriodSpeed * Time.deltaTime;

        m_NowRad %= Mathf.PI * 2f;



        Vector3 pos = new Vector3(Mathf.Cos(m_NowRad), 0, Mathf.Sin(m_NowRad)) * m_Amplitude;

        pos.x *= m_Axis.x;

        pos.y *= m_Axis.y;

        pos.z *= m_Axis.z;

        transform.position = pos + m_BasePos;



        if (m_ShotTime < 0f)

        {

            m_ShotTime = m_ShotInterval;

            int index = Random.Range(0, GetBulletPrefabsCount());



            var shotParam = new BulletShotParam(this, index);

            var laser = BulletController.ShotBullet(shotParam);



            // index == 1 の時は、軌道を変えて撃ってみる

            if (index > 0)

            {

                BulletParam bulletParam = GetBulletParam(0);

                laser.ChangeOrbital(bulletParam.ConditionalOrbitalParams[0]);

            }

        }

        else

        {

            m_ShotTime -= Time.deltaTime;

        }

    }

}