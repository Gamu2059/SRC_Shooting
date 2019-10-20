using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleReal", fileName = "param.battle_real.asset")]
public class BattleRealParamSet : ScriptableObject
{
    [Header("Param Set")]

    [SerializeField]
    private BattleRealPlayerManagerParamSet m_PlayerManagerParamSet;
    public BattleRealPlayerManagerParamSet PlayerManagerParamSet => m_PlayerManagerParamSet;

    [SerializeField]
    private BattleRealEnemyGroupManagerParamSet m_EnemyGroupManagerParamSet;
    public BattleRealEnemyGroupManagerParamSet EnemyGroupManagerParamSet => m_EnemyGroupManagerParamSet;

    [SerializeField]
    private BattleRealEnemyManagerParamSet m_EnemyManagerParamSet;
    public BattleRealEnemyManagerParamSet EnemyManagerParamSet => m_EnemyManagerParamSet;

    [SerializeField]
    private BattleRealBulletManagerParamSet m_BulletManagerParamSet;
    public BattleRealBulletManagerParamSet BulletManagerParamSet => m_BulletManagerParamSet;

    [SerializeField]
    private BattleRealItemManagerParamSet m_ItemManagerParamSet;
    public BattleRealItemManagerParamSet ItemManagerParamSet => m_ItemManagerParamSet;

    [SerializeField]
    private BattleRealEventTriggerParamSet m_EventTriggerParamSet;
    public BattleRealEventTriggerParamSet EventTriggerParamSet => m_EventTriggerParamSet;

    [Header("Boss Change")]

    [SerializeField]
    private ControlBgmParam m_ControlBgmParam;
    public ControlBgmParam ControlBgmParam => m_ControlBgmParam;
}
