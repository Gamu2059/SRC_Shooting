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
    public int BulletRemove;
    public int SecretItem;
    public int BossDefeat;
    public int BossRescue;
    public ulong Score;
    public ulong LevelBonusScore;
    public ulong MaxChainBonusScore;
    public ulong BulletRemoveBonusScore;
    public ulong SecretItemBonusScore;
    public ulong HackingCompleteBonusScore;
    public ulong TotalScore;
    public E_GAME_RANK Rank;

    public ulong GetBonusScore(E_ACHIEVEMENT_TYPE type)
    {
        switch (type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                return LevelBonusScore;
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                return MaxChainBonusScore;
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                return BulletRemoveBonusScore;
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                return SecretItemBonusScore;
            case E_ACHIEVEMENT_TYPE.RESCUE:
                return HackingCompleteBonusScore;
        }

        return 0;
    }
}
