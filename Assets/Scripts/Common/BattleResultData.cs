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
        var levelBonus = CalcBonusScore(battleData, rankParam, E_ACHIEVEMENT_TYPE.LEVEL);
        var maxChainBonus = CalcBonusScore(battleData, rankParam, E_ACHIEVEMENT_TYPE.MAX_CHAIN);
        var bulletRemoveBonus = CalcBonusScore(battleData, rankParam, E_ACHIEVEMENT_TYPE.BULLET_REMOVE);
        var secretItemBonus = CalcBonusScore(battleData, rankParam, E_ACHIEVEMENT_TYPE.SECRET_ITEM);
        var rescueBonus = CalcBonusScore(battleData, rankParam, E_ACHIEVEMENT_TYPE.RESCUE);
        var totalBonus = CalcTotalScore(battleData.ScoreInChapter.Value, levelBonus, maxChainBonus, bulletRemoveBonus, secretItemBonus, rescueBonus);
        
        var data = new BattleChapterResultData
        {
            GameMode = m_GameMode,
            Chapter = chapter,
            Difficulty = m_Difficulty,
            IsClear = isClear,
            Level = battleData.LevelInChapter.Value,
            MaxChain = battleData.MaxChainInChapter.Value,
            BulletRemove = battleData.BulletRemoveInChapter.Value,
            SecretItem = battleData.SecretItemInChapter.Value,
            BossDefeat = battleData.BossDefeatCountInChapter.Value,
            BossRescue = battleData.BossRescueCountInChapter.Value,
            Score = battleData.ScoreInChapter.Value,
            LevelBonusScore = levelBonus,
            MaxChainBonusScore = maxChainBonus,
            BulletRemoveBonusScore = bulletRemoveBonus,
            SecretItemBonusScore = secretItemBonus,
            HackingCompleteBonusScore = rescueBonus,
            TotalScore = totalBonus,
            Rank = rankParam.GetRank(totalBonus),
        };

        Debug.LogFormat("Chapter result {0}", totalBonus);
        ChapterResultDict.Add(chapter, data);
    }

    private ulong CalcBonusScore(BattleData battleData, BattleRankParam rankParam, E_ACHIEVEMENT_TYPE type)
    {
        if (battleData == null || rankParam == null)
        {
            return 0;
        }

        if (!battleData.IsAchieve(type))
        {
            return 0;
        }

        var current = (double)battleData.GetAchievementCurrentValue(type);
        var target = battleData.GetAchievementTargetValue(type);
        if (target < 1)
        {
            return 0;
        }

        return (ulong) Math.Round((current / target) * rankParam.AchievementBonusScore);
    }

    private ulong CalcTotalScore(params ulong[] scores)
    {
        ulong value = 0;
        foreach (var i in scores)
        {
            value = Math.Min(value + i, ulong.MaxValue);
        }

        return value;
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

    /// <summary>
    /// 指定したチャプターのリザルトをローカルストレージに保存する
    /// </summary>
    public void SaveChapterResult(E_CHAPTER chapter)
    {
        var data = GetChapterResult(chapter);
        if (data == null)
        {
            return;
        }

        Debug.Log("SaveChapterResult Total Score : " + data.TotalScore);
        var record = new PlayerRecord("", data.TotalScore, data.Chapter, DateTime.Now);
        PlayerRecordManager.Instance.AddChapterModeRecord(data.Chapter, data.Difficulty, record);
    }
}
