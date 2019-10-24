using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/INF-C-761/Hacker1", fileName = "param.inf_c_761_hacker1.asset")]
public class InfC761Hacker1ParamSet : BattleHackingEnemyBehaviorParamSet
{
    [SerializeField]
    private BattleHackingBossBehaviorParamSet[] m_BehaviorParamSets;
    public BattleHackingBossBehaviorParamSet[] BehaviorParamSets => m_BehaviorParamSets;
}
