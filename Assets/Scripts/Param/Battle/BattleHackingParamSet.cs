using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleHacking", fileName = "param.battle_hacking.asset")]
public class BattleHackingParamSet : ScriptableObject
{
    [SerializeField]
    private BattleHackingPlayerManagerParamSet m_PlayerManagerParamSet;
    public BattleHackingPlayerManagerParamSet PlayerManagerParamSet => m_PlayerManagerParamSet;

    [SerializeField]
    private BattleHackingBulletManagerParamSet m_BulletManagerParamSet;
    public BattleHackingBulletManagerParamSet BulletManagerParamSet => m_BulletManagerParamSet;
}
