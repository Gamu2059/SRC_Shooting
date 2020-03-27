#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッカーのボムを制御する
/// </summary>
public class BattleRealPlayerBomb : BulletController
{
    [SerializeField]
    private float m_BaseSize;

    [SerializeField]
    private AnimationCurve m_SizeCurve;

    private Transform m_BombTransform;

    public override void OnInitialize()
    {
        base.OnInitialize();

        var bombCollider = GetCollider().GetColliderTransform(E_COLLIDER_TYPE.PLAYER_BOMB);
        m_BombTransform = bombCollider.Transform;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var nowLifetime = GetNowLifeTime();
        var duration = GetBulletParam() != null ? GetBulletParam().LifeTime : 0;

        if (duration > 0)
        {
            var normalizedTime = nowLifetime / duration;
            if (normalizedTime <= 1)
            {
                var size = m_BaseSize * m_SizeCurve.Evaluate(normalizedTime);
                m_BombTransform.localScale = Vector3.one * size;
            }
        }
    }
}
