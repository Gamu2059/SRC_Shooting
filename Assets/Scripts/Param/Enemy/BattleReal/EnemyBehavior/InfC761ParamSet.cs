using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/INF-C-761/Behavior", fileName = "param.inf_c_761.asset")]
public class InfC761ParamSet : BattleRealEnemyBehaviorParamSet
{
    [SerializeField]
    private BattleRealBossBehaviorParamSet[] m_AttackParamSets;
    public BattleRealBossBehaviorParamSet[] AttackParamSets => m_AttackParamSets;

    [SerializeField]
    private BattleRealBossBehaviorParamSet[] m_DownParamSets;
    public BattleRealBossBehaviorParamSet[] DownParamSets => m_DownParamSets;

    [SerializeField]
    private float m_DownHp;
    public float DownHp => m_DownHp;

    [SerializeField]
    private List<float> m_ChangeAttackHpRates;
    public List<float> ChangeAttackHpRates => m_ChangeAttackHpRates;

    [SerializeField]
    private ItemCreateParam m_ItemParam;
    public ItemCreateParam ItemParam => m_ItemParam;
}
