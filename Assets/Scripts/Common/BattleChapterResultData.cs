using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チャプターのリザルトデータ
/// </summary>
public class BattleChapterResultData
{
    public E_GAME_MODE GameMode;
    public E_CHAPTER Chapter;
    public E_DIFFICULTY Difficulty;
    public bool IsClear;
    public int Level;
    public int MaxChain;
    public int RemoveBullet;
    public int SecretItem;
    public int BossDefeat;
    public int BossRescue;
    public ulong Score;
    public ulong ScoreInBonus;
    public E_GAME_RANK Rank;
}
