#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// エフェクト再生オプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Option/PlayEffect", fileName = "play_effect.sequence_option.asset")]
public class BattleRealSequencePlayEffectFunc : SequenceOptionFunc
{
    [SerializeField]
    private EffectParamSet[] m_Effects;

    [SerializeField]
    private SequentialEffectParamSet[] m_SequentialEffects;

    public override void Call(Transform transform = null)
    {
        if (m_Effects != null)
        {
            foreach (var e in m_Effects)
            {
                BattleRealEffectManager.Instance.CreateEffect(e, transform);
            }
        }

        if (m_SequentialEffects != null)
        {
            foreach (var e in m_SequentialEffects)
            {
                BattleRealEffectManager.Instance.RegisterSequentialEffect(e, transform);
            }
        }
    }
}
