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

    /// <summary>
    /// ゲームモード
    /// </summary>
    public E_GAME_MODE GameMode { get; private set; }

    /// <summary>
    /// 難易度
    /// </summary>
    public E_DIFFICULTY Difficulty { get; private set; }

    /// <summary>
    /// チャプターごとのリザルト
    /// </summary>
    public Dictionary<E_CHAPTER, BattleChapterResultData> ChapterResultDict { get; private set; }
    
    /// <summary>
    /// ストーリーのリザルト
    /// </summary>
    public BattleStoryResultData StoryResult { get; private set; }

    #endregion

    /// <summary>
    /// ゲーム開始時にゲームモードと難易度が確定するので、そこで初めてコンストラクタが呼べるはず。
    /// </summary>
    public BattleResultData(E_GAME_MODE gameMode, E_DIFFICULTY difficulty)
    {
        GameMode = gameMode;
        Difficulty = difficulty;
        ChapterResultDict = new Dictionary<E_CHAPTER, BattleChapterResultData>();
        StoryResult = null;
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
    public void AddChapterResult()
    {

    }

    /// <summary>
    /// ストーリーを通してのリザルトを記録する
    /// </summary>
    public void AddStoryResult()
    {

    }


    [Obsolete]
    public double Score { get; private set; }

    [Obsolete]
    public double LifeBonusScore { get; private set; }

    [Obsolete]
    public double PerfectHackingBonusScore { get; private set; }

    [Obsolete]
    public double TotalScore { get; private set; }
}
