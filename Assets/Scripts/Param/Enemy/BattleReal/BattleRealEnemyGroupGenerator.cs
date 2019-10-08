using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleRealEnemy/EnemyGroupGenerator", fileName = "param.battle_real_enemy_group_generator")]
public class BattleRealEnemyGroupGenerator : ScriptableObject
{
    [SerializeField]
    private BattleRealEnemyGroupGenerateParamSet[] m_GroupGenerateParamSets;
    public BattleRealEnemyGroupGenerateParamSet[] GroupGenerateParamSets => m_GroupGenerateParamSets;
}
