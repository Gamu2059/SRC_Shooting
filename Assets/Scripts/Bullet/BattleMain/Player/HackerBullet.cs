using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerBullet : BulletController
{
    private float m_NowDownDamage;
    
    public float GetNowDownDamage()
    {
        return m_NowDownDamage;
    }

    public void SetNowDownDamage(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowDownDamage = relative == E_RELATIVE.ABSOLUTE ? value : m_NowDownDamage + value;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_IsLookMoveDir = false;
    }

    protected override void OnEnterHitChara(HitSufferData<CharaController> hitData)
    {
        base.OnEnterHitChara(hitData);

        var colliderType = hitData.SufferCollider.Transform.ColliderType;
        switch (colliderType)
        {
            case E_COLLIDER_TYPE.CRITICAL:
                DestroyBullet();
                break;
        }
    }
}
