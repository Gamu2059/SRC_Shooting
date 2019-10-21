using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア・ステージ進度などの記録を管理するマネージャ
/// </summary>
public class PlayerRecordManager : ControllableObject
{
    public static PlayerRecordManager Instance => GameManager.Instance.PlayerRecordManager;

    private List<PlayerRecord> m_PlayerRecords;

    private int m_MaxRecordNum;

    public override void OnInitialize(){
        m_PlayerRecords = new List<PlayerRecord>();
        m_MaxRecordNum = 1000;
        AddDummyScore();
    }

    public void AddRecord(PlayerRecord record){
        if(m_PlayerRecords.Count + 1 > m_MaxRecordNum){
            m_PlayerRecords.RemoveAt(m_PlayerRecords.Count - 1);
            m_PlayerRecords.Add(record);
            SortRecord();
        }else{
            m_PlayerRecords.Add(record);
            SortRecord();
        }
    }

    private void SortRecord(){
        m_PlayerRecords.Sort((a, b) => {
            if(b.m_FinalScore > a.m_FinalScore){
                return 1;
            }

            if(b.m_FinalScore < a.m_FinalScore){
                return -1;
            }

            return 0;
        });
    }

    public void ShowRecord(){
        Debug.Log("Score Ranking ...");
        SortRecord();
        int i = 0;
        foreach (PlayerRecord rec in m_PlayerRecords.GetRange(0,5))
        {
            i++;
            string showStr = string.Format("{0} : Score={1} , Stage={2}, Date={3}", i, rec.m_FinalScore, ReachedStageStrFromInt(rec.m_FinalReachedStage), rec.m_PlayedDate.ToString("yyyy/MM/dd"));
            Debug.Log(showStr);
        }
    }

    private void AddDummyScore(){
        m_PlayerRecords.Add(new PlayerRecord(1000, 1, new System.DateTime(2000, 8, 16)));
        m_PlayerRecords.Add(new PlayerRecord(2000, 2, new System.DateTime(2000, 8, 16)));
        m_PlayerRecords.Add(new PlayerRecord(3000, 3, new System.DateTime(2000, 8, 16)));
        m_PlayerRecords.Add(new PlayerRecord(4000, 4, new System.DateTime(2000, 8, 16)));
        m_PlayerRecords.Add(new PlayerRecord(5000, 5, new System.DateTime(2000, 8, 16)));
        m_PlayerRecords.Add(new PlayerRecord(6000, 6, new System.DateTime(2000, 8, 16)));
        m_PlayerRecords.Add(new PlayerRecord(9999, 7, new System.DateTime(2000, 8, 16)));
    }

    private string ReachedStageStrFromInt(int stage){
        if(stage != 7){
            return stage.ToString();
        }else{
            return "All Clear!";
        }
    }

    public PlayerRecord GetTopRecord(){
        SortRecord();
        return m_PlayerRecords[0];
    }
}
