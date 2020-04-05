#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのボスの攻撃やダウン時のパラメータの規定クラス。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Enemy/BehaviorUnitParam", fileName = "param.behavior_unit.asset")]
public class BattleHackingBossBehaviorUnitParamSet : ScriptableObject
{
    [SerializeField]
    private string m_BehaviorClass;
    public string BehaviorClass => m_BehaviorClass;

    // ボスや攻撃に関わらず共通に使いそうなので、以下に書いておく。
    [SerializeField, Tooltip("")]
    public HackingBossPhase m_HackingBossPhase;
}
