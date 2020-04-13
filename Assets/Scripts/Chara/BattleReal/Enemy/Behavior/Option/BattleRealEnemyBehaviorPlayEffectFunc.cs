#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// エフェクト再生オプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Option/PlayEffect", fileName = "param.behavior_option.asset")]
public class BattleRealEnemyBehaviorPlayEffectFunc : BattleRealEnemyBehaviorOptionFuncBase
{
    [SerializeField]
    private EffectParamSet[] m_Effects;

    [SerializeField]
    private SequentialEffectParamSet[] m_SequentialEffects;

    public override void Call(BattleRealEnemyBase enemy)
    {
        if (m_Effects != null)
        {
            foreach (var e in m_Effects)
            {
                BattleRealEffectManager.Instance.CreateEffect(e, enemy.transform);
            }
        }

        if (m_SequentialEffects != null)
        {
            foreach (var e in m_SequentialEffects)
            {
                BattleRealEffectManager.Instance.RegisterSequentialEffect(e, enemy.transform);
            }
        }
    }
}
