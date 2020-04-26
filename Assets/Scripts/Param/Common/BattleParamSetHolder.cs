#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 難易度とチャプターごとにバトルパラメータセットを格納するもの。
/// </summary>
[Serializable]
public class BattleParamSetHolder
{
    [Serializable]
    private class BattleParamSetOnDifficulty
    {
        [SerializeField]
        private E_DIFFICULTY m_Difficulty;
        public E_DIFFICULTY Difficulty => m_Difficulty;

        [SerializeField]
        private BattleParamSet m_Param;
        public BattleParamSet Param => m_Param;
    }

    [Serializable]
    private class BattleParamSetOnChapter
    {
        [SerializeField]
        private E_CHAPTER m_Chapter;
        public E_CHAPTER Chapter => m_Chapter;

        [SerializeField]
        private List<BattleParamSetOnDifficulty> m_Params;

        public BattleParamSet GetBattleParamSet(E_DIFFICULTY difficulty)
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
    private List<BattleParamSetOnChapter> m_Params;

    public BattleParamSet GetBattleParamSet(E_CHAPTER chapter, E_DIFFICULTY difficulty)
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

        return param.GetBattleParamSet(difficulty);
    }
}
