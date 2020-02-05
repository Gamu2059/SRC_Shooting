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

        SaveDataManager.Load();

        int maxScore = SaveDataManager.GetInt("BestScore", 0);
        m_PlayerRecords.Add(new PlayerRecord("Nanashi", maxScore, E_STATE.NORMAL_1, new System.DateTime()));
    }

    public void AddRecord(PlayerRecord record){

        var maxScore = GetTopRecord().m_FinalScore;
        if (record.m_FinalScore > maxScore)
        {
            SaveDataManager.SetInt("BestScore", (int)record.m_FinalScore);
            SaveDataManager.Save();
        }

        if (m_PlayerRecords.Count + 1 > m_MaxRecordNum){
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

    private string ReachedStageStrFromInt(int stage){
        if(stage != 7){
            return stage.ToString();
        }else{
            return "All Clear!";
        }
    }

    public PlayerRecord GetDummyRecord()
    {
        return new PlayerRecord("Nanashi", 1, E_STATE.NORMAL_1, new System.DateTime(2019, 5, 1));
    }

    public PlayerRecord GetTopRecord(){
        SortRecord();
        return m_PlayerRecords[0];
    }

    public List<PlayerRecord> GetRecordsInRange(int range)
    {
        var len = m_PlayerRecords.Count;

        if(len < range)
        {
            var res = m_PlayerRecords.GetRange(0, len);
            var rem = range - len;
            while(rem != 0)
            {
                res.Add(GetDummyRecord());
                rem--;
            }
            return res;
        }
        else
        {
            return m_PlayerRecords.GetRange(0, range);
        }
    }
}
