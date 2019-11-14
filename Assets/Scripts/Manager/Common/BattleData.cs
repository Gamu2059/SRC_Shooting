using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数ステージを跨ぐようなバトルデータを保持する。
/// </summary>
public class BattleData
{
    /// <summary>
    /// 残機
    /// </summary>
    public int Remain { get; private set; }

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
    public int HackingSecceedCount { get; private set; }

    public void ResetScore()
    {
        Score = 0;
    }

    public void AddScore(double score)
    {
        Score += score;
    }

    public void ResetLevel()
    {

    }

    public void ResetExp()
    {
        Exp = 0;
    }

    public void AddExp(int exp)
    {
        Exp += exp;
    }
}
