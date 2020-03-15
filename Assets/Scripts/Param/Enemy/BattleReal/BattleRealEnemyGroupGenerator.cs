#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyGroupGenerator", fileName = "param.battle_real_enemy_group_generator.asset")]
public class BattleRealEnemyGroupGenerator : ScriptableObject
{
    [Serializable]
    public class Content
    {
        [SerializeField, Tooltip("敵グループの生成条件")]
        private EventTriggerRootCondition m_Condition;
        public EventTriggerRootCondition Condition => m_Condition;

        [SerializeField]
        private BattleRealEnemyGroupGenerateParamSet m_GroupGenerateParamSet;
        public BattleRealEnemyGroupGenerateParamSet GroupGenerateParamSet => m_GroupGenerateParamSet;
    }

    [SerializeField]
    private Content[] m_Contents;
    public Content[] Contents => m_Contents;
}
