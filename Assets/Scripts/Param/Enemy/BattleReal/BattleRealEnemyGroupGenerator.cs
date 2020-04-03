#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/EnemyGroupGenerator", fileName = "param.generator.asset")]
public class BattleRealEnemyGroupGenerator : ScriptableObject
{
    [Serializable]
    public class Content
    {
        [SerializeField, Tooltip("データとして使用はしませんが、データの目印になるものです。")]
        private string m_Commnet;

        [SerializeField, Tooltip("敵グループの生成条件")]
        private EventTriggerRootCondition m_Condition;
        public EventTriggerRootCondition Condition => m_Condition;

        [SerializeField]
        private BattleRealEnemyGroupParam m_Param;
        public BattleRealEnemyGroupParam Param => m_Param;
    }

    [SerializeField]
    private Content[] m_Contents;
    public Content[] Contents => m_Contents;
}
