using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Level", fileName = "param.hacking_level.asset")]
public class BattleHackingLevelParamSet : ScriptableObject
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

    [SerializeField, Tooltip("分かりやすいように名前を付けられる。特にゲーム中に使用するものではない。")]
    private string m_GeneratorLabel;
    public string GeneratorLabel => m_GeneratorLabel;

    [SerializeField, Tooltip("ゲーム開始時のプレイヤーの最初の位置")]
    private Vector2 m_InitAppearViewportPosition;
    public Vector2 InitAppearViewportPosition => m_InitAppearViewportPosition;

    [SerializeField, Tooltip("敵の生成パラメータ")]
    private Content[] m_EnemyContents;
    public Content[] EnemyContents => m_EnemyContents;
}
