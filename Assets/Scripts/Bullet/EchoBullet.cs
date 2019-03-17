using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : BulletController
{
    [SerializeField]
    private int m_HitCount;

    [SerializeField]
    private float m_Interval;

    [SerializeField]
    private bool m_IsRoot;

    private float delay;
    
    public override void OnHitCharacter(CharaController chara)
    {
        base.OnHitCharacter(chara);
    }

    public override void OnUpdate()
    {
        
        delay += Time.deltaTime;

        if (delay >= m_Interval && m_IsRoot)
        {
            EchoController echo = (EchoController)GetBulletOwner();
            echo.ReadyRadiateShot(this.GetPosition());
            delay = 0;
        }

        base.OnUpdate();
    }

}
