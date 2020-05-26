using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// チャプターごとのリザルトとストーリーを通してのリザルトを保持するクラス<br/>
/// このクラスのインスタンスさえあればレコードを作成できるようになっている
/// </summary>
public class BattleResultData
{
    #region Field

    private E_GAME_MODE m_GameMode;

    private E_DIFFICULTY m_Difficulty;

    /// <summary>
    /// チャプターごとのリザルト
    /// </summary>
    public Dictionary<E_CHAPTER, BattleChapterResultData> ChapterResultDict { get; private set; }
    
    /// <summary>
    /// ストーリーのリザルト
    /// </summary>
    public BattleStoryResultData StoryResult { get; private set; }

    #endregion

    public BattleResultData(E_GAME_MODE gameMode, E_DIFFICULTY difficulty)
    {
        ChapterResultDict = new Dictionary<E_CHAPTER, BattleChapterResultData>();
        StoryResult = null;
        m_GameMode = gameMode;
        m_Difficulty = difficulty;
    }

    public void OnFinalize()
    {
        ChapterResultDict?.Clear();
        ChapterResultDict = null;
        StoryResult = null;
    }

    /// <summary>
    /// チャプターのリザルトを記録する
    /// </summary>
    public void AddChapterResult(E_CHAPTER chapter, BattleData battleData, BattleRankParam rankParam, bool isClear)
    {
        var scoreInBonus = GetScoreInBonus(battleData, rankParam);
        var data = new BattleChapterResultData
        {
            GameMode = m_GameMode,
            Chapter = chapter,
            Difficulty = m_Difficulty,
            IsClear = isClear,
            Level = battleData.LevelInChapter.Value,
            MaxChain = battleData.MaxChainInChapter.Value,
            RemoveBullet = battleData.BulletRemoveInChapter.Value,
            SecretItem = battleData.SecretItemInChapter.Value,
            BossDefeat = battleData.BossDefeatCountInChapter.Value,
            BossRescue = battleData.BossRescueCountInChapter.Value,
            Score = battleData.ScoreInChapter.Value,
            ScoreInBonus = scoreInBonus,
            Rank = rankParam.GetRank(scoreInBonus),
        };

        ChapterResultDict.Add(chapter, data);
    }

    private ulong GetScoreInBonus(BattleData battleData, BattleRankParam rankParam)
    {
        var score = battleData.ScoreInChapter.Value;
        
        if (battleData.IsAchieveLevel())
        {
            score += rankParam.AchievementBonusScore;
        }
        if (battleData.IsAchieveMaxChain())
        {
            score += rankParam.AchievementBonusScore;
        }
        if (battleData.IsAchieveBulletRemove())
        {
            score += rankParam.AchievementBonusScore;
        }
        if (battleData.IsAchieveSecretItem())
        {
            score += rankParam.AchievementBonusScore;
        }
        if (battleData.IsAchieveRescue())
        {
            score += rankParam.AchievementBonusScore;
        }

        return score;
    }

    /// <summary>
    /// ストーリーを通してのリザルトを記録する
    /// </summary>
    public void AddStoryResult()
    {

    }

    public BattleChapterResultData GetChapterResult(E_CHAPTER chapter)
    {
        if (ChapterResultDict != null && ChapterResultDict.TryGetValue(chapter, out var data))
        {
            return data;
        }

        return null;
    }
}
