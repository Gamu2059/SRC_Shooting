using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのEffectManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Manager/BattleRealEffect", fileName = "param.battle_real_effect.asset")]
public class BattleRealEffectManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("弾消しエフェクト")]
    private EffectParamSet m_BulletRemoveEffect;
    public EffectParamSet BulletRemoveEffect => m_BulletRemoveEffect;
}
