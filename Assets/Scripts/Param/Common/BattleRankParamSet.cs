using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ゲームの総合評価の付け方の指標となるパラメータセット。
/// </summary>
[Serializable]
public class BattleRankParamSet
{
    [Serializable]
    private class BattleRankParamOnDifficulty
    {
        [SerializeField]
        private E_DIFFICULTY m_Difficulty;
        public E_DIFFICULTY Difficulty => m_Difficulty;

        [SerializeField]
        private BattleRankParam m_Param;
        public BattleRankParam Param => m_Param;
    }

    [Serializable]
    private class BattleRankParamOnChapter
    {
        [SerializeField]
        private E_CHAPTER m_Chapter;
        public E_CHAPTER Chapter => m_Chapter;

        [SerializeField]
        private List<BattleRankParamOnDifficulty> m_Params;

        public BattleRankParam GetRankParam(E_DIFFICULTY difficulty)
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
    private List<BattleRankParamOnChapter> m_Params;

    public BattleRankParam GetRankParam(E_CHAPTER chapter, E_DIFFICULTY difficulty)
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

        return param.GetRankParam(difficulty);
    }
}

[Serializable]
public class BattleRankParam
{
    [SerializeField]
    private ulong m_DRankScore;

    [SerializeField]
    private ulong m_CRankScore;

    [SerializeField]
    private ulong m_BRankScore;

    [SerializeField]
    private ulong m_ARankScore;

    [SerializeField]
    private ulong m_SRankScore;

    [SerializeField]
    private ulong m_SSRankScore;

    [SerializeField]
    private ulong m_AchievementBonusScore;
    public ulong AchievementBonusScore => m_AchievementBonusScore;

    public E_GAME_RANK GetRank(ulong score)
    {
        if (score < m_DRankScore)
        {
            return E_GAME_RANK.E;
        }
        else if (score < m_CRankScore)
        {
            return E_GAME_RANK.D;
        }
        else if (score < m_BRankScore)
        {
            return E_GAME_RANK.C;
        }
        else if (score < m_ARankScore)
        {
            return E_GAME_RANK.B;
        }
        else if (score < m_SRankScore)
        {
            return E_GAME_RANK.A;
        }
        else if (score < m_SSRankScore)
        {
            return E_GAME_RANK.S;
        }

        return E_GAME_RANK.SS;
    }
}
