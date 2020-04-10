using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チャプターのリザルトデータ
/// </summary>
public class BattleChapterResultData
{
    /// <summary>
    /// ハッキング成功回数
    /// </summary>
    public int HackingSuccessCount { get; private set; }

    /// <summary>
    /// ハッキング挑戦回数
    /// </summary>
    public int HackingTryCount { get; private set; }

    /// <summary>
    /// チャプター全体を通して必要な最小のハッキング挑戦回数<br/>
    /// ある意味では定数
    /// </summary>
    public int MinHackingTryNum { get; private set; }

    /// <summary>
    /// チャプター終了時のレベル
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// 隠しアイテム獲得数
    /// </summary>
    public int SecretItemGetCount { get; private set; }

    /// <summary>
    /// チャプター終了までに出した最高チェイン数
    /// </summary>
    public int MaxChainCount { get; private set; }

    /// <summary>
    /// チャプター終了までに消した敵弾の合計数
    /// </summary>
    public int EnemyBulletCancelCount { get; private set; }

    /// <summary>
    /// チャプターボード達成数
    /// </summary>
    public int AchievementCount { get; private set; }

    /// <summary>
    /// チャプターで獲得したスコア<br/>
    /// クリア時のボーナスも加味される
    /// </summary>
    public ulong Score { get; private set; }

    /// <summary>
    /// チャプターでのランク
    /// </summary>
    public int Rank { get; private set; }
}
