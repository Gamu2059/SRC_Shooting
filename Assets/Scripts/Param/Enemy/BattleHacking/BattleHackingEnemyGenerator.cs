using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Enemy/EnemyGenerator", fileName = "param.battle_hacking_enemy_generator.asset")]
public class BattleHackingEnemyGenerator : ScriptableObject
{
    [Serializable]
    public class Content
    {
        [SerializeField, Tooltip("敵の生成時間")]
        private float m_GenerateTime;
        public float GenerateTime => m_GenerateTime;

        [SerializeField, Tooltip("敵の生成パラメータのセット")]
        private BattleHackingEnemyGenerateParamSet m_GenerateParamSet;
        public BattleHackingEnemyGenerateParamSet GenerateParamSet => m_GenerateParamSet;

        [SerializeField, Tooltip("敵の振る舞いパラメータのセット")]
        private BattleHackingEnemyBehaviorParamSet m_BehaviorParamSet;
        public BattleHackingEnemyBehaviorParamSet BehaviorParamSet => m_BehaviorParamSet;
    }

    [SerializeField]
    private string m_GeneratorLabel;
    public string GeneratorLabel => m_GeneratorLabel;

    [SerializeField]
    private Content[] m_Contents;
    public Content[] Contents => m_Contents;
}
