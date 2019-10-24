using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int m_Last {get; private set;}

    public float m_BestScore {get; private set;}

    public int m_HackingSucceedCount {get; private set;}

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
        m_Last = 3;
    }

    public void ResetPlayerLast(int last){
        m_Last = last;
    }

    public void SetLast(int last){
        m_Last = last;
    }

    public void IncreaseLast(){
        m_Last++;
    }

    public void DecreaseLast(){
        m_Last--;
    }

    public void ResetBestScore(){
        m_BestScore = GameManager.Instance.PlayerRecordManager.GetTopRecord().m_FinalScore;
    }

    public void UpdateBestScore(float score){
        m_BestScore = score;
    }

    public void ResetHackingSucceedCount(){
        m_HackingSucceedCount = 0;
    }

    public void IncreaseHackingSucceedCount(){
        m_HackingSucceedCount++;
    }
}
