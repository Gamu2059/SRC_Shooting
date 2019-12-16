using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルの結果を保持する。
/// </summary>
public class BattleResultData
{
    public double Score { get; private set; }

    public double LifeBonusScore { get; private set; }

    public double PerfectHackingBonusScore { get; private set; }

    public double TotalScore { get; private set; }

    private double LifeBonus;
    private double PerfectHackingBonus;

    public BattleResultData(BattleRealPlayerLevelParamSet playerLevelParamSet)
    {
        var commonData = playerLevelParamSet.CommonDefData;
        LifeBonus = commonData.LifeBonus;
        PerfectHackingBonus = commonData.PerfectHackingBonus;
    }

    /// <summary>
    /// バトルの結果からスコアを計算する。
    /// </summary>
    public void ClacScore(BattleData battleData)
    {
        if (battleData == null)
        {
            Score = 0;
            LifeBonusScore = 0;
            PerfectHackingBonusScore = 0;
            TotalScore = 0;
            return;
        }

        Score = battleData.Score;
        LifeBonusScore = battleData.PlayerLife * LifeBonus;
        PerfectHackingBonusScore = battleData.IsPerfectHacking() ? PerfectHackingBonus : 0;
        TotalScore = Score + LifeBonusScore + PerfectHackingBonusScore;
    }
}
