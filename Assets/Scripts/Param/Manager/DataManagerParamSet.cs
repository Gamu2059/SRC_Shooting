using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/Common/DataManagerParamSet", fileName = "param.data.asset")]
public class DataManagerParamSet : ScriptableObject
{
    [SerializeField]
    private BattleConstantParam m_BattleConstantParam;
    public BattleConstantParam BattleConstantParam => m_BattleConstantParam;

    [SerializeField]
    private BattleAchievementParamSet m_BattleAchievementParamSet;
    public BattleAchievementParamSet BattleAchievementParamSet => m_BattleAchievementParamSet;

    [SerializeField]
    private BattleRankParamSet m_BattleRankParamSet;
    public BattleRankParamSet BattleRankParamSet => m_BattleRankParamSet;

    [SerializeField]
    private BattleParamSetHolder m_BattleParamSetHolder;
    public BattleParamSetHolder BattleParamSetHolder => m_BattleParamSetHolder;
}
