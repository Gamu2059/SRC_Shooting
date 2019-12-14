using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRealPlayerDownTriangle : BattleCommonEffectController
{
    [SerializeField]
    private Transform m_Base;

    [SerializeField]
    private SpriteRenderer m_Triangle;

    [SerializeField]
    private AnimationCurve m_AlphaCurve;

    private Transform m_Player;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_Player = BattleRealPlayerManager.Instance.Player.transform;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_Player == null || Owner == null)
        {
            DestroyEffect(true);
            return;
        }

        var playerPos = m_Player.position;
        var targetPos = Owner.position;
        var delta = targetPos - playerPos;
        transform.forward = delta;
        transform.position = m_Player.position;

        var dist = delta.ToVector2XZ().magnitude;
        var color = m_Triangle.color;
        color.a = m_AlphaCurve.Evaluate(dist);
        m_Triangle.color = color;
    }
}
