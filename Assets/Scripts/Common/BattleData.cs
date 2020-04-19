using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 複数ステージを跨ぐようなバトルデータを保持する。
/// </summary>
public class BattleData
{
    #region Field

    private BattleConstantParam m_ConstantParam;

    /// <summary>
    /// 残機
    /// </summary>
    public int PlayerLife { get; private set; }

    /// <summary>
    /// 同じ挑戦条件のベストスコア
    /// </summary>
    public ulong BestScore { get; private set; }

    /// <summary>
    /// 現在スコア
    /// </summary>
    public ulong Score { get; private set; }

    /// <summary>
    /// 現在レベル
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// 現在EXP
    /// </summary>
    public int Exp { get; private set; }

    /// <summary>
    /// チャージしきったエナジーの数
    /// </summary>
    public int EnergyStock { get; private set; }

    /// <summary>
    /// チャージ中のエナジー
    /// </summary>
    public int EnergyCharge { get; private set; }

    /// <summary>
    /// ハッキング挑戦回数
    /// </summary>
    public int HackingTryCount { get; private set; }

    /// <summary>
    /// ハッキング成功回数
    /// </summary>
    public int HackingSuccessCount { get; private set; }

    /// <summary>
    /// チャプター全体を通して必要な最小のハッキング挑戦回数<br/>
    /// ある意味では定数
    /// </summary>
    public int MinHackingTryNum { get; private set; }

    #endregion

    #region Callback

    public Action IncreaseEnergyStockAction;
    public Action ConsumeEnergyStockAction;

    #endregion

    public BattleData(BattleConstantParam param)
    {
        m_ConstantParam = param;
    }

    public void ResetDataOnChapterStart()
    {
        BestScore = PlayerRecordManager.Instance.GetTopRecord().m_FinalScore;
        Score = 0;
        Level = 0;
        Exp = 0;
        EnergyStock = 0;
        EnergyCharge = 0;
        HackingTryCount = 0;
        HackingSuccessCount = 0;
        MinHackingTryNum = 0;
    }

    public BattleRealPlayerLevelData GetCurrentLevelParam()
    {
        return m_ConstantParam.PlayerLevelDatas[Level];
    }

    #region Player Life

    public void AddPlayerLife(int num)
    {
        if (num < 1)
        {
            return;
        }

        PlayerLife = Mathf.Min(PlayerLife + num, m_ConstantParam.MaxLife);
    }

    public void DecreasePlayerLife()
    {
        // チャプター0は残機が減らない
        if (DataManager.Instance.Chapter == E_CHAPTER.CHAPTER_0)
        {
            return;
        }

        PlayerLife = Mathf.Max(PlayerLife - 1, 0);
    }

    /// <summary>
    /// ゲームオーバになるかどうか
    /// </summary>
    public bool IsGameOver()
    {
        if (DataManager.Instance.Chapter == E_CHAPTER.CHAPTER_0)
        {
            return false;
        }

        return PlayerLife < 1;
    }

    #endregion

    #region Score

    public void AddScore(int score)
    {
        if (score < 1)
        {
            return;
        }

        // チャプター0はスコアが入らない
        if (DataManager.Instance.Chapter == E_CHAPTER.CHAPTER_0)
        {
            return;
        }

        Score += (ulong)score;
    }

    #endregion

    #region BestScore

    public void UpdateBestScore(ulong score)
    {
        BestScore = score;
    }

    #endregion

    #region Level

    public void IncreaseLevel()
    {
        var maxLevel = m_ConstantParam.MaxLevel;
        if (Level == maxLevel - 1)
        {
            AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_POWER_UP);
        }

        Level = Mathf.Min(Level + 1, maxLevel);
    }

    #endregion

    #region Exp

    public int GetCurrentNecessaryExp()
    {
        var param = GetCurrentLevelParam();
        if (param == null)
        {
            Debug.LogWarning("Expパラメータを参照できませんでした");
            return 0;
        }

        return param.NecessaryExpNextLevel;
    }

    public void AddExp(int exp)
    {
        var maxLevel = m_ConstantParam.MaxLevel;

        // スコア増加(レベルMAXの時)
        if (Level >= maxLevel)
        {
            AddScore(exp);
            return;
        }

        var addedExp = Exp + exp;
        var necessaryExp = GetCurrentNecessaryExp();
        while (addedExp >= necessaryExp)
        {
            if (Level < maxLevel)
            {
                IncreaseLevel();
                addedExp -= necessaryExp;
                necessaryExp = GetCurrentNecessaryExp();
            }
            else
            {
                AddScore(addedExp);
                break;
            }

        }

        // Levelが最大の時はExpゲージも最大状態にしておく
        if (Level >= maxLevel)
        {
            addedExp = necessaryExp;
        }

        Exp = addedExp;
    }

    #endregion

    #region Energy

    public int GetCurrentNecessaryEnergyCharge()
    {
        var necessaryChargeValues = m_ConstantParam.NecessaryEnergyChargeNextStocks;
        if (necessaryChargeValues == null || necessaryChargeValues.Length < 1)
        {
            Debug.LogWarning("エナジーチャージ量を指定する配列の要素数が0です");
            return 0;
        }

        var index = Mathf.Min(necessaryChargeValues.Length - 1, EnergyStock);
        return necessaryChargeValues[index];
    }

    public void AddEnergyCharge(int charge)
    {
        var maxEnergy = m_ConstantParam.MaxEnergy;
        if (EnergyStock >= maxEnergy)
        {
            // すでにストックが上限に達している場合
            AddScore(charge);
            return;
        }

        var necessaryChargeValue = GetCurrentNecessaryEnergyCharge();
        var addedCharge = EnergyCharge + charge;
        while (addedCharge >= necessaryChargeValue)
        {
            if (EnergyStock < maxEnergy)
            {
                IncreaseEnergyStock();
                addedCharge -= necessaryChargeValue;
                necessaryChargeValue = GetCurrentNecessaryEnergyCharge();
            }
            else
            {
                AddScore(addedCharge);
                break;
            }
        }

        // ストックが最大の時はEnergyChargeゲージも最大状態にしておく
        if (EnergyStock >= maxEnergy)
        {
            addedCharge = necessaryChargeValue;
        }

        EnergyCharge = addedCharge;
    }

    /// <summary>
    /// エナジーストックを一つ増やす
    /// </summary>
    public void IncreaseEnergyStock()
    {
        EnergyStock = Mathf.Min(EnergyStock + 1, m_ConstantParam.MaxEnergy);
        IncreaseEnergyStockAction?.Invoke();
    }

    /// <summary>
    /// エナジーストックを一つ消費する
    /// </summary>
    public void ConsumeEnergyStock()
    {
        // ストックが最大の時はゲージが最大状態になっているので、0に戻す
        if (EnergyStock >= m_ConstantParam.MaxEnergy)
        {
            EnergyCharge = 0;
        }

        EnergyStock = Mathf.Max(EnergyStock - 1, 0);
        ConsumeEnergyStockAction?.Invoke();
    }

    #endregion

    #region Hacking Succeed Count

    public void SetPerfectHackingSuccessCount(int count)
    {
        MinHackingTryNum = count;
    }

    public void IncreaseHackingTryCount()
    {
        HackingTryCount++;
    }

    public void SetHackingComplete(bool isComplete)
    {
        //IsHackingComplete = isComplete;
    }

    public void OnHackingResult(bool isHackingSuccess)
    {
        //if (isHackingSuccess)
        //{
        //    HackingSuccessCount++;
        //    if (HackingSuccessCount >= 1)
        //    {
        //        AddScore(HackingSuccessBonus * HackingSuccessCount);
        //    }
        //}
        //else
        //{
        //    HackingSuccessCount = 0;
        //}
    }

    #endregion
}
