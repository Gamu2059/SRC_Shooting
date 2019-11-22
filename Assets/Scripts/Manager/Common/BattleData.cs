using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数ステージを跨ぐようなバトルデータを保持する。
/// </summary>
public class BattleData
{
    /// <summary>
    /// ゲームモード
    /// </summary>
    public E_GAME_MODE GameMode { get; private set; }

    /// <summary>
    /// ステージ
    /// </summary>
    public E_STAGE Stage { get; private set; }

    /// <summary>
    /// 残機
    /// </summary>
    public int PlayerLife { get; private set; }

    /// <summary>
    /// 同じ挑戦条件のベストスコア
    /// </summary>
    public double BestScore { get; private set; }

    /// <summary>
    /// 現在スコア
    /// </summary>
    public double Score { get; private set; }

    /// <summary>
    /// 現在レベル
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// 現在EXP
    /// </summary>
    public int Exp { get; private set; }

    /// <summary>
    /// チャージしきったエネルギーの数
    /// </summary>
    public int EnergyCount { get; private set; }

    /// <summary>
    /// チャージ中のエネルギー
    /// </summary>
    public float EnergyCharge { get; private set; }

    /// <summary>
    /// ハッキング成功回数
    /// </summary>
    public int HackingSucceedCount { get; private set; }

    private BattleRealPlayerLevelParamSet m_PlayerLevelParamSet;

    public BattleData(BattleRealPlayerLevelParamSet playerLevelParamSet)
    {
        m_PlayerLevelParamSet = playerLevelParamSet;

        GameMode = E_GAME_MODE.STORY;
        Stage = E_STAGE.NORMAL_1;
    }

    public void ResetAll()
    {

    }

    #region Player Life

    public void ResetPlayerLife()
    {
        // パラメータで設定できるようにしたい
        PlayerLife = 3;
    }

    public void IncreasePlayerLife()
    {
        PlayerLife++;
    }

    public void DecreasePlayerLife()
    {
        PlayerLife = Mathf.Max(PlayerLife -1 , 0);
    }

    #endregion

    #region Score

    public void ResetScore()
    {
        Score = 0;
    }

    public void AddScore(double score)
    {
        Score += score;
    }

    #endregion

    #region BestScore

    public void ResetBestScore()
    {
        BestScore = PlayerRecordManager.Instance.GetTopRecord().m_FinalScore;
    }

    public void UpdateBestScore(double score)
    {
        BestScore = score;
    }

    #endregion

    #region Level

    public void ResetLevel()
    {
        Level = 0;
    }

    #endregion

    #region Exp

    public void ResetExp()
    {
        Exp = 0;
    }

    public void AddExp(int exp)
    {
        Exp += exp;

        var currentExp = Exp;
        var currentLevel = Level;

        var levelNum = m_PlayerLevelParamSet.PlayerLevels.Length;

        // スコア増加(レベルMAXの時)
        if (currentLevel == levelNum - 1)
        {
            AddScore(exp);
            return;
        }

        currentExp += exp;
        var expParamSet = m_PlayerLevelParamSet.PlayerLevels[currentLevel];

        if (currentExp >= expParamSet.NecessaryExpToLevelUpNextLevel)
        {
            Level++;
            currentExp %= expParamSet.NecessaryExpToLevelUpNextLevel;
        }

        Exp = currentExp;
    }

    #endregion

    /// <summary>
    /// エナジーチャージを増やす。
    /// </summary>
    public void AddEnergyCharge(float charge)
    {
        // var currentCharge = m_CurrentBombCharge.Value;
        // currentCharge += charge;

        // if (currentCharge >= m_PlayerState.BombCharge) {
        //     m_CurrentBombNum.Value++;
        //     currentCharge %= m_PlayerState.BombCharge;
        // }
    }

    #region Hacking Succeed Count 

    public void ResetHackingSucceedCount()
    {
        HackingSucceedCount = 0;
    }

    public void IncreaseHackingSucceedCount()
    {
        HackingSucceedCount++;
    }

    #endregion
}
