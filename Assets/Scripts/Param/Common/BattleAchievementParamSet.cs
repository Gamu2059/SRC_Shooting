using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのAchievementのパラメータセット。
/// </summary>
[Serializable]
public class BattleAchievementParamSet
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
    private int m_TargetLevel;
    public int TargetLevel => m_TargetLevel;

    [SerializeField, Tooltip("達成条件とする最大チェイン数")]
    private int m_TargetMaxChain;
    public int TargetMaxChain => m_TargetMaxChain;

    [SerializeField, Tooltip("達成条件とする弾消し数")]
    private int m_TargetBulletRemove;
    public int TargetBulletRemove => m_TargetBulletRemove;

    [SerializeField, Tooltip("達成条件とする秘密アイテム取得数")]
    private int m_TargetSecretItem;
    public int TargetSecretItem => m_TargetSecretItem;

    [SerializeField, Tooltip("達成条件とするボス救出数")]
    private int m_TargetRescue;
    public int TargetRescue => m_TargetRescue;
}
