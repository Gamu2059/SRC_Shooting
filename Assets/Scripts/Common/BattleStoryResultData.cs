using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ストーリーのリザルトデータ
/// </summary>
public class BattleStoryResultData
{
    /// <summary>
    /// ストーリーをクリアしたかどうか
    /// </summary>
    public bool IsClearStory { get; private set; }

    /// <summary>
    /// 隠しチャプター(チャプター6)に到達したかどうか
    /// </summary>
    public bool IsReachedSecretChapter { get; private set; }

    /// <summary>
    /// クリアまでは行かなかったが到達できたチャプター
    /// </summary>
    public E_CHAPTER ReachedChapter { get; private set; }

    /// <summary>
    /// ストーリークリア時の残機数
    /// </summary>
    public int RemainPlayerLife { get; private set; }

    /// <summary>
    /// ストーリー終了までに救出した数
    /// </summary>
    public int RescueCount { get; private set; }

    /// <summary>
    /// ストーリー全体を通してのスコア<br/>
    /// ボーナスも加味される
    /// </summary>
    public ulong TotalScore { get; private set; }

    /// <summary>
    /// ストーリー全体を通してのランク
    /// </summary>
    public int Rank { get; private set; }
}
