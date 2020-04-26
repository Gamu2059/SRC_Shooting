#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// リアルモードのAchievementのパラメータセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleCommon/BattleAchievement", fileName = "param.battle_achievement.asset")]
public class BattleAchievementParamSet : ScriptableObject
{
    [Serializable]
    private class AchievementParamOnDifficulty
    {
        [SerializeField]
        private E_DIFFICULTY m_Difficulty;
        public E_DIFFICULTY Difficulty => m_Difficulty;

        [SerializeField]
        private BattleAchievementParam m_Param;
        public BattleAchievementParam Param => m_Param;
    }

    [Serializable]
    private class AchievementParamOnChapter
    {
        [SerializeField]
        private E_CHAPTER m_Chapter;
        public E_CHAPTER Chapter => m_Chapter;

        [SerializeField]
        private List<AchievementParamOnDifficulty> m_Params;

        public BattleAchievementParam GetAchievementParam(E_DIFFICULTY difficulty)
        {
            if (m_Params == null)
            {
                return null;
            }

            var param = m_Params.Find(p => p.Difficulty == difficulty);
            if (param == null)
            {
                return null;
            }

            return param.Param;
        }
    }

    [SerializeField]
    private List<AchievementParamOnChapter> m_Params;

    public BattleAchievementParam GetAchievementParam(E_CHAPTER chapter, E_DIFFICULTY difficulty)
    {
        if (m_Params == null)
        {
            return null;
        }

        var param = m_Params.Find(p => p.Chapter == chapter);
        if (param == null)
        {
            return null;
        }

        return param.GetAchievementParam(difficulty);
    }
}

/// <summary>
/// リアルモードのAchievementのパラメータ。
/// </summary>
[Serializable]
public class BattleAchievementParam
{
    [SerializeField, Tooltip("達成条件とするレベル")]
    private ulong m_TargetLevel;
    public ulong TargetLevel => m_TargetLevel;

    [SerializeField, Tooltip("達成条件とする最大チェイン数")]
    private ulong m_TargetMaxChain;
    public ulong TargetMaxChain => m_TargetMaxChain;

    [SerializeField, Tooltip("達成条件とする弾消し数")]
    private ulong m_TargetBulletRemove;
    public ulong TargetBulletRemove => m_TargetBulletRemove;

    [SerializeField, Tooltip("達成条件とする秘密アイテム取得数")]
    private ulong m_TargetSecretItem;
    public ulong TargetSecretItem => m_TargetSecretItem;

    [SerializeField, Tooltip("達成条件とするボス救出数")]
    private ulong m_TargetRescue;
    public ulong TargetRescue => m_TargetRescue;
}
