using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoBullet : Bullet
{
    [SerializeField]
    private int m_HitCount;

    [SerializeField]
    private float m_Interval;

    [SerializeField]
    private bool m_IsRoot;

    private float delay;
    
    public override void OnHitCharacter(CharaControllerBase chara)
    {

        base.OnHitCharacter(chara);
    }

    private void Update()
    {
        delay += Time.deltaTime;

        if(delay >= m_Interval && m_IsRoot)
        {
            EchoController echo = (EchoController)GetBulletOwner();
            echo.ReadyRadiateShot(this.GetPosition());
            delay = 0;            
        }
    }

}
