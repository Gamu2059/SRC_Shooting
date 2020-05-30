using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// 複数ステージを跨ぐようなバトルデータを保持する。
/// </summary>
public class BattleData
{
    #region Define

    public class DataOnConstructor
    {
        public BattleConstantParam ConstParam;
        public ulong BestScore;
        public int LifeOption;
        public int EnergyOption;
    }

    public class DataOnChapterStart
    {
        public E_CHAPTER Chapter;
        public BattleAchievementParam AchievementParam;
    }

    #endregion

    #region Field

    private BattleConstantParam m_ConstantParam;

    private BattleAchievementParam m_AchievementParam;

    /// <summary>
    /// 残機<br/>
    /// チャプターを跨いで保持される
    /// </summary>
    public ReactiveProperty<int> PlayerLife { get; private set; }

    /// <summary>
    /// 同じ挑戦条件のベストスコア<br/>
    /// ストーリーモードかチャプターモードかの違いによって取得されるデータが異なる<br/>
    /// チャプターを跨いで保持される
    /// </summary>
    public ReactiveProperty<ulong> BestScore { get; private set; }

    /// <summary>
    /// 現在の累積スコア<br/>
    /// チャプターモードの場合はChapterScoreと同値となる<br/>
    /// ストーリーモードの場合は今までのスコアの累計値となる<br/>
    /// チャプターを跨いで保持される
    /// </summary>
    public ReactiveProperty<ulong> Score { get; private set; }

    /// <summary>
    /// 現在のチャプターにおけるスコア<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<ulong> ScoreInChapter { get; private set; }

    /// <summary>
    /// レベル<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<int> LevelInChapter { get; private set; }

    /// <summary>
    /// EXP<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<int> ExpInChapter { get; private set; }

    /// <summary>
    /// チャージしきったエナジーの数<br/>
    /// チャプターを跨いで保持される
    /// </summary>
    public ReactiveProperty<int> EnergyStock { get; private set; }

    /// <summary>
    /// チャージ中のエナジー<br/>
    /// チャプターを跨いで保持される
    /// </summary>
    public ReactiveProperty<int> EnergyCharge { get; private set; }

    /// <summary>
    /// ハッキングを連続で成功させた回数<br/>
    /// チャプターを跨いで保持される
    /// </summary>
    public ReactiveProperty<int> HackingSuccessChain { get; private set; }

    /// <summary>
    /// チェイン数<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<int> ChainInChapter { get; private set; }

    /// <summary>
    /// 最高チェイン数<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<int> MaxChainInChapter { get; private set; }

    /// <summary>
    /// 消した敵弾の数<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<int> BulletRemoveInChapter { get; private set; }

    /// <summary>
    /// 取得した秘密アイテムの数<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<int> SecretItemInChapter { get; private set; }

    /// <summary>
    /// チャプターのボス撃破数<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<int> BossDefeatCountInChapter { get; private set; }

    /// <summary>
    /// チャプターのボス救出数<br/>
    /// チャプターごとに初期化される
    /// </summary>
    public ReactiveProperty<int> BossRescueCountInChapter { get; private set; }

    /// <summary>
    /// 全チャプターの累計ボス救出数<br/>
    /// チャプターを跨いで保持される
    /// </summary>
    public ReactiveProperty<int> TotalBossRescue { get; private set; }

    /// <summary>
    /// チャージショットの強化段階
    /// </summary>
    public ReactiveProperty<int> ChargeLevel { get; private set; }

    public E_CHAPTER m_CurrentChapter;

    #endregion

    #region Callback

    public Action IncreaseEnergyStockAction;
    public Action ConsumeEnergyStockAction;

    #endregion

    public BattleData(DataOnConstructor data)
    {
        m_ConstantParam = data.ConstParam;

        PlayerLife = new ReactiveProperty<int>(data.LifeOption);
        BestScore = new ReactiveProperty<ulong>(data.BestScore);
        Score = new ReactiveProperty<ulong>(0);
        ScoreInChapter = new ReactiveProperty<ulong>(0);
        LevelInChapter = new ReactiveProperty<int>(0);
        ExpInChapter = new ReactiveProperty<int>(0);
        EnergyStock = new ReactiveProperty<int>(data.EnergyOption);
        EnergyCharge = new ReactiveProperty<int>(0);
        HackingSuccessChain = new ReactiveProperty<int>(0);
        ChainInChapter = new ReactiveProperty<int>(0);
        MaxChainInChapter = new ReactiveProperty<int>(0);
        BulletRemoveInChapter = new ReactiveProperty<int>(0);
        SecretItemInChapter = new ReactiveProperty<int>(0);
        BossDefeatCountInChapter = new ReactiveProperty<int>(0);
        BossRescueCountInChapter = new ReactiveProperty<int>(0);
        TotalBossRescue = new ReactiveProperty<int>(0);
        ChargeLevel = new ReactiveProperty<int>(0);
    }

    public void OnFinalize()
    {
        TotalBossRescue?.Dispose();
        BossRescueCountInChapter?.Dispose();
        BossDefeatCountInChapter?.Dispose();
        SecretItemInChapter?.Dispose();
        BulletRemoveInChapter?.Dispose();
        MaxChainInChapter?.Dispose();
        ChainInChapter?.Dispose();
        HackingSuccessChain?.Dispose();
        EnergyCharge?.Dispose();
        EnergyStock?.Dispose();
        ExpInChapter?.Dispose();
        LevelInChapter?.Dispose();
        ScoreInChapter?.Dispose();
        Score?.Dispose();
        BestScore?.Dispose();
        PlayerLife?.Dispose();
    }

    public void InitDataOnChapterStart(DataOnChapterStart data)
    {
        m_CurrentChapter = data.Chapter;
        m_AchievementParam = data.AchievementParam;

        ScoreInChapter.Value = 0;
        LevelInChapter.Value = 0;
        ExpInChapter.Value = 0;
        ChainInChapter.Value = 0;
        MaxChainInChapter.Value = 0;
        BulletRemoveInChapter.Value = 0;
        SecretItemInChapter.Value = 0;
        BossDefeatCountInChapter.Value = 0;
        BossRescueCountInChapter.Value = 0;
        ChargeLevel.Value = 0;
    }


    #region Player Life

    public void AddPlayerLife(int num)
    {
        if (num < 1)
        {
            return;
        }

        var newNum = Mathf.Min(PlayerLife.Value + num, m_ConstantParam.MaxLife);
        if (newNum != PlayerLife.Value)
        {
            PlayerLife.Value = newNum;
        }
    }

    public void DecreasePlayerLife()
    {
        // チャプター0は残機が減らない
        if (m_CurrentChapter == E_CHAPTER.CHAPTER_0)
        {
            return;
        }

        var newNum = Mathf.Max(PlayerLife.Value - 1, 0);
        if (newNum != PlayerLife.Value)
        {
            PlayerLife.Value = newNum;
        }
    }

    /// <summary>
    /// ゲームオーバになるかどうか
    /// </summary>
    public bool IsGameOver()
    {
        if (m_CurrentChapter == E_CHAPTER.CHAPTER_0)
        {
            return false;
        }

        return PlayerLife.Value < 1;
    }

    #endregion

    #region Score

    public void AddScore(int score, bool applyChainCorrection = false)
    {
        if (score < 1)
        {
            return;
        }

        // チャプター0はスコアが入らない
        if (m_CurrentChapter == E_CHAPTER.CHAPTER_0)
        {
            return;
        }

        ulong s = (ulong)score;
        if (applyChainCorrection)
        {
            s = CalcCorrectChainScore(score);
        }

        Score.Value += s;
        ScoreInChapter.Value += s;

        if (Score.Value > BestScore.Value)
        {
            BestScore.Value = Score.Value;
        }
    }

    private ulong CalcCorrectChainScore(int score)
    {
        return (ulong)(score * (1 + ChainInChapter.Value / 100f));
    }

    #endregion

    #region Level

    public BattleRealPlayerLevelData GetCurrentPlayerLevelParam()
    {
        var idx = Mathf.Min(LevelInChapter.Value, m_ConstantParam.MaxLevel - 1);
        return m_ConstantParam.PlayerLevelDatas[idx];
    }

    public void IncreaseLevel()
    {
        var newNum = Mathf.Min(LevelInChapter.Value + 1, m_ConstantParam.MaxLevel);
        if (newNum != LevelInChapter.Value)
        {
            LevelInChapter.Value = newNum;
        }
    }

    #endregion

    #region Exp

    public int GetCurrentNecessaryExp()
    {
        var param = GetCurrentPlayerLevelParam();
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
        if (LevelInChapter.Value >= maxLevel)
        {
            AddScore(exp);
            return;
        }

        var addedExp = ExpInChapter.Value + exp;
        var necessaryExp = GetCurrentNecessaryExp();
        while (addedExp >= necessaryExp)
        {
            if (LevelInChapter.Value < maxLevel)
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
        if (LevelInChapter.Value >= maxLevel)
        {
            addedExp = necessaryExp;
        }

        ExpInChapter.Value = addedExp;
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

        var index = Mathf.Min(necessaryChargeValues.Length - 1, EnergyStock.Value);
        return necessaryChargeValues[index];
    }

    public void AddEnergyCharge(int charge)
    {
        var maxEnergy = m_ConstantParam.MaxEnergy;
        if (EnergyStock.Value >= maxEnergy)
        {
            // すでにストックが上限に達している場合
            AddScore(charge);
            return;
        }

        var necessaryChargeValue = GetCurrentNecessaryEnergyCharge();
        var addedCharge = EnergyCharge.Value + charge;
        while (addedCharge >= necessaryChargeValue)
        {
            if (EnergyStock.Value < maxEnergy)
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
        if (EnergyStock.Value >= maxEnergy)
        {
            addedCharge = necessaryChargeValue;
        }

        EnergyCharge.Value = addedCharge;
    }

    /// <summary>
    /// エナジーストックを一つ増やす
    /// </summary>
    public void IncreaseEnergyStock()
    {
        var newNum = Mathf.Min(EnergyStock.Value + 1, m_ConstantParam.MaxEnergy);
        if (newNum != EnergyStock.Value)
        {
            EnergyStock.Value = newNum;
            IncreaseEnergyStockAction?.Invoke();
        }
    }

    /// <summary>
    /// エナジーストックを一つ消費する
    /// </summary>
    public void ConsumeEnergyStock()
    {
        // ストックが最大の時はゲージが最大状態になっているので、0に戻す
        if (EnergyStock.Value >= m_ConstantParam.MaxEnergy)
        {
            EnergyCharge.Value = 0;
        }

        var newNum = Mathf.Max(EnergyStock.Value - 1, 0);
        if (newNum != EnergyStock.Value)
        {
            EnergyStock.Value = newNum;
            ConsumeEnergyStockAction?.Invoke();
        }
    }

    #endregion

    #region Chain

    public void IncreaseChain()
    {
        ChainInChapter.Value++;
        MaxChainInChapter.Value = Math.Max(MaxChainInChapter.Value, ChainInChapter.Value);
    }

    public void ResetChain()
    {
        ChainInChapter.Value = 0;
    }

    #endregion

    #region Remove Bullet

    public void IncreaseRemoveBullet()
    {
        BulletRemoveInChapter.Value++;
    }

    #endregion

    #region Secret Item

    public void IncreaseSecretItem()
    {
        SecretItemInChapter.Value++;
    }

    #endregion

    #region Hacking Success Chain

    public void IncreaseHackingSuccessChain()
    {
        HackingSuccessChain.Value++;
    }

    public void ResetHackingSuccessChain()
    {
        HackingSuccessChain.Value = 0;
    }

    #endregion

    #region Achievement

    public int GetAchievementTargetValue(E_ACHIEVEMENT_TYPE type)
    {
        switch (type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                return GetAchievementTargetLevel();
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                return GetAchievementTargetMaxChain();
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                return GetAchievementTargetBulletRemove();
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                return GetAchievementTargetSecretItem();
            case E_ACHIEVEMENT_TYPE.RESCUE:
                return GetAchievementTargetRescue();
        }

        return 0;
    }

    public int GetAchievementCurrentValue(E_ACHIEVEMENT_TYPE type)
    {
        switch (type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                return LevelInChapter.Value;
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                return MaxChainInChapter.Value;
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                return BulletRemoveInChapter.Value;
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                return SecretItemInChapter.Value;
            case E_ACHIEVEMENT_TYPE.RESCUE:
                return BossRescueCountInChapter.Value;
        }

        return 0;
    }

    public bool IsAchieve(E_ACHIEVEMENT_TYPE type)
    {
        switch (type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                return IsAchieveLevel();
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                return IsAchieveMaxChain();
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                return IsAchieveBulletRemove();
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                return IsAchieveSecretItem();
            case E_ACHIEVEMENT_TYPE.RESCUE:
                return IsAchieveRescue();
        }

        return false;
    }

    public int GetAchievementTargetLevel()
    {
        if (m_AchievementParam == null)
        {
            return 0;
        }

        return m_AchievementParam.TargetLevel;
    }

    public bool IsAchieveLevel()
    {
        return LevelInChapter.Value >= GetAchievementTargetLevel();
    }

    public int GetAchievementTargetMaxChain()
    {
        if (m_AchievementParam == null)
        {
            return 0;
        }

        return m_AchievementParam.TargetMaxChain;
    }

    public bool IsAchieveMaxChain()
    {
        return MaxChainInChapter.Value >= GetAchievementTargetMaxChain();
    }

    public int GetAchievementTargetBulletRemove()
    {
        if (m_AchievementParam == null)
        {
            return 0;
        }

        return m_AchievementParam.TargetBulletRemove;
    }

    public bool IsAchieveBulletRemove()
    {
        return BulletRemoveInChapter.Value >= GetAchievementTargetBulletRemove();
    }

    public int GetAchievementTargetSecretItem()
    {
        if (m_AchievementParam == null)
        {
            return 0;
        }

        return m_AchievementParam.TargetSecretItem;
    }

    public bool IsAchieveSecretItem()
    {
        return SecretItemInChapter.Value >= GetAchievementTargetSecretItem();
    }

    public int GetAchievementTargetRescue()
    {
        if (m_AchievementParam == null)
        {
            return 0;
        }

        return m_AchievementParam.TargetRescue;
    }

    public bool IsAchieveRescue()
    {
        return BossRescueCountInChapter.Value >= GetAchievementTargetRescue();
    }

    #endregion

    #region Charge Shot

    public BattleRealChargeShotLevelData GetCurrentChargeLevelParam()
    {
        var idx = Mathf.Min(ChargeLevel.Value, m_ConstantParam.MaxChargeLevel - 1);
        return m_ConstantParam.ChargeLevelDatas[idx];
    }

    /// <summary>
    /// チャージ強化レベルを一つ上げる
    /// </summary>
    public void IncreaseChargeLevel()
    {
        var newNum = Mathf.Min(ChargeLevel.Value + 1, m_ConstantParam.MaxChargeLevel - 1);
        if (newNum != ChargeLevel.Value)
        {
            ChargeLevel.Value = newNum;
        }
    }

    public void ResetChargeLevel()
    {
        ChargeLevel.Value = 0;
    }

    /// <summary>
    /// 最大レベルかどうか
    /// </summary>
    public bool IsMaxChargeLevel()
    {
        // 0から始まるので、-1にする
        return ChargeLevel.Value >= m_ConstantParam.MaxChargeLevel - 1;
    }

    #endregion

    public bool IsOpenFinalChapter()
    {
        return false;
    }
}
