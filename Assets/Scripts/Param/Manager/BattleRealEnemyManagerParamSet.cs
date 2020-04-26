#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// リアルモードのEnemyManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Manager/BattleRealEnemy", fileName = "param.battle_real_enemy.asset")]
public class BattleRealEnemyManagerParamSet : ScriptableObject
{
    #region Define

    [Serializable]
    private class DifficultySet
    {
        public E_DIFFICULTY Difficulty;
        public BattleRealEnemyGroupGenerator Generator;
    }

    #endregion

    [SerializeField, Tooltip("左下のオフセットフィールド")]
    private Vector2 m_MinOffsetFieldPosition;
    public Vector2 MinOffsetFieldPosition => m_MinOffsetFieldPosition;

    [SerializeField, Tooltip("右上のオフセットフィールド")]
    private Vector2 m_MaxOffsetFieldPosition;
    public Vector2 MaxOffsetFieldPosition => m_MaxOffsetFieldPosition;

    [SerializeField, Tooltip("難易度を参照して、難易度別にジェネレータを取得するかどうか")]
    private bool m_ReferenceDifficultyForGenerator;

    [SerializeField, Tooltip("敵グループのジェネレータ")]
    private BattleRealEnemyGroupGenerator m_Generator;
    public BattleRealEnemyGroupGenerator Generator => m_Generator;

    [SerializeField]
    private List<DifficultySet> m_DifficultyReferencedGenerators;

    /// <summary>
    /// 敵グループのジェネレータを取得する。
    /// </summary>
    public BattleRealEnemyGroupGenerator GetGenerator()
    {
        if (!m_ReferenceDifficultyForGenerator)
        {
            return m_Generator;
        }

        if (DataManager.Instance == null || m_DifficultyReferencedGenerators == null)
        {
            return m_Generator;
        }

        var difficulty = DataManager.Instance.Difficulty;
        var foundGenerator = m_DifficultyReferencedGenerators.Find(g => g.Difficulty == difficulty);
        return foundGenerator != null ? foundGenerator.Generator : m_Generator;
    }
}