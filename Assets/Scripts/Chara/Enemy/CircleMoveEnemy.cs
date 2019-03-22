using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UniRx;



/// <summary>

/// 円運動をしながら定期的に弾を撃ってくる敵。

/// </summary>

public class CircleMoveEnemy : EnemyController

{



    [SerializeField]

    private Material m_Normal;



    [SerializeField]

    private Material m_HitMate;



    [SerializeField]

    private Renderer m_Renderer;



    [SerializeField]

    private Vector3 m_BasePos;



    [SerializeField]

    private float m_Radius;



    [SerializeField]

    private float m_AngleSpeed;



    [SerializeField]

    private float m_ShotInterval;



    [SerializeField]

    private float m_ShotPosOffset;



    [SerializeField]

    private float m_BulletSpeed = 10;



    [SerializeField]

    private float m_DeadDistance = 500;

    [SerializeField]
    private float a;

    [SerializeField]
    private float b;

    [SerializeField]
    private float c;



    private float m_NowRad;



    private float m_ShotTime;



    Timer hitTimer;



    private void Awake()

    {

        a++;

        base.Awake();

        m_ShotTime = 0;



        m_Renderer.material = m_Normal;

    }



    public void Update()

    {

        

        m_NowRad += m_AngleSpeed * Time.deltaTime;

        m_NowRad %= Mathf.PI * 2;



        Vector3 pos = new Vector3(Mathf.Cos(m_NowRad), 0, Mathf.Sin(m_NowRad)) * m_Radius + m_BasePos;

        transform.position = pos;



        Vector3 angle = transform.eulerAngles;

        angle.y = -(m_NowRad * Mathf.Rad2Deg) + 90;

        transform.eulerAngles = angle;



        if (m_ShotTime < 0f)

        {

            m_ShotTime = m_ShotInterval;

            b++;

            BulletController.ShotBullet(this);

            c++;

        }

        else

        {

            m_ShotTime -= Time.deltaTime;

        }

    }



    public override void OnSuffer(BulletController bullet, ColliderData colliderData)

    {

        base.OnSuffer(bullet, colliderData);



        m_Renderer.material = m_HitMate;



        if (hitTimer == null)

        {

            hitTimer = Timer.CreateTimeoutTimer(Timer.E_TIMER_TYPE.UNSCALED_TIMER, 0.1f, () =>

            {

                m_Renderer.material = m_Normal;

                hitTimer = null;

            });

            TimerManager.Instance.RegistTimer(hitTimer);

        }

        else

        {

            TimerManager.Instance.RemoveTimer(hitTimer);

            hitTimer = Timer.CreateTimeoutTimer(Timer.E_TIMER_TYPE.UNSCALED_TIMER, 0.1f, () =>

            {

                m_Renderer.material = m_Normal;

                hitTimer = null;

            });

            TimerManager.Instance.RegistTimer(hitTimer);

        }

    }

}