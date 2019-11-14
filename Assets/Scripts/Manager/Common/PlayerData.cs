using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトル中のプレイヤーデータを保持する。
/// </summary>
public class PlayerData
{
    /// <summary>
    /// 残機数
    /// </summary>
    public int Remain {get; private set;}

    /// <summary>
    /// ベストスコア
    /// </summary>
    public float BestScore {get; private set;}

    /// <summary>
    /// ハッキング成功回数
    /// </summary>
    public int HackingSucceedCount {get; private set;}

    public PlayerData(int last){
        ResetHackingSucceedCount();
        ResetBestScore();
        ResetPlayerLast(last);
    }

    public PlayerData(){
        ResetHackingSucceedCount();
        ResetBestScore();
        ResetPlayerLast();
    }

    public void ResetPlayerLast(){
        Remain = 3;
    }

    public void ResetPlayerLast(int last){
        Remain = last;
    }

    public void SetLast(int last){
        Remain = last;
    }

    public void IncreaseLast(){
        Remain++;
    }

    public void DecreaseLast(){
        Remain--;
    }

    public void ResetBestScore(){
        BestScore = GameManager.Instance.PlayerRecordManager.GetTopRecord().m_FinalScore;
    }

    public void UpdateBestScore(float score){
        BestScore = score;
    }

    public void ResetHackingSucceedCount(){
        HackingSucceedCount = 0;
    }

    public void IncreaseHackingSucceedCount(){
        HackingSucceedCount++;
    }
}
