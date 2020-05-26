using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア・ステージ進度などの記録を管理するマネージャ
/// </summary>
public class PlayerRecordManager : SingletonMonoBehavior<PlayerRecordManager>
{
    #region Field Inspector

    [SerializeField]
    private PlayerRecordManagerParamSet m_ParamSet;

    #endregion

    private Dictionary<E_DIFFICULTY, List<PlayerRecord>> m_StoryModePlayerRecords;
    private Dictionary<E_CHAPTER, List<PlayerRecord>> m_ChapterModePlayerRecords;

    private int m_MaxRecordNum;

    public override void OnInitialize(){
        m_StoryModePlayerRecords = new Dictionary<E_DIFFICULTY, List<PlayerRecord>> 
        {
            {E_DIFFICULTY.EASY, new List<PlayerRecord>() },
            {E_DIFFICULTY.NORMAL, new List<PlayerRecord>() },
            {E_DIFFICULTY.HARD, new List<PlayerRecord>() },
            {E_DIFFICULTY.HADES, new List<PlayerRecord>() }
        };
        m_ChapterModePlayerRecords = new Dictionary<E_CHAPTER, List<PlayerRecord>>
        {
            { E_CHAPTER.CHAPTER_0, new List<PlayerRecord>() },
            { E_CHAPTER.CHAPTER_1, new List<PlayerRecord>() },
            { E_CHAPTER.CHAPTER_2, new List<PlayerRecord>() },
            { E_CHAPTER.CHAPTER_3, new List<PlayerRecord>() },
            { E_CHAPTER.CHAPTER_4, new List<PlayerRecord>() },
            { E_CHAPTER.CHAPTER_5, new List<PlayerRecord>() },
            { E_CHAPTER.CHAPTER_6, new List<PlayerRecord>() },
        };
        
        m_MaxRecordNum = 1000;

        SaveDataManager.Load();
        var maxScore = (ulong)SaveDataManager.GetInt("BestScore", 0);
        m_StoryModePlayerRecords[E_DIFFICULTY.NORMAL].Add(new PlayerRecord("Nanashi", maxScore, E_CHAPTER.CHAPTER_1, new System.DateTime()));
    }

    public void AddStoryModeRecord(PlayerRecord record)
    {
        void f(E_DIFFICULTY d) {
            var recs = m_StoryModePlayerRecords[d];

            var maxScore = GetTopRecord(recs).m_FinalScore;
            if(record.m_FinalScore > maxScore)
            {
                SaveDataManager.SetInt("BestScore", (int)record.m_FinalScore);
                SaveDataManager.Save();
            }

            if(recs.Count + 1 > m_MaxRecordNum)
            {
                recs.RemoveAt(recs.Count - 1);
                recs.Add(record);
                SortRecord(recs);
            }
            else
            {
                recs.Add(record);
                SortRecord(recs);
            }
        }

        f(record.StageDifficulty());
    }

    public void AddChapterModeRecord(PlayerRecord record)
    {
        void f(E_CHAPTER s)
        {
            var recs = m_ChapterModePlayerRecords[s];
            if (recs.Count + 1 > m_MaxRecordNum)
            {
                recs.RemoveAt(recs.Count - 1);
                recs.Add(record);
                SortRecord(recs);
            }
            else
            {
                recs.Add(record);
                SortRecord(recs);
            }
        }

        f(record.m_FinalReachedStage);
    }

    private void SortRecord(List<PlayerRecord> records)
    {
        records.Sort((a, b) => {
            if (b.m_FinalScore > a.m_FinalScore)
            {
                return 1;
            }

            if (b.m_FinalScore < a.m_FinalScore)
            {
                return -1;
            }

            return 0;
        });
    }

    public PlayerRecord GetDummyRecord(string name = "Nanashi", ulong score = 1, E_CHAPTER stage = E_CHAPTER.CHAPTER_1, System.DateTime date = new System.DateTime())
    {
        return new PlayerRecord(name, score, stage, date);
    }

    public PlayerRecord GetTopRecord(List<PlayerRecord> recs)
    {
        SortRecord(recs);
        return recs[0];
    }

    public PlayerRecord GetTopRecord()
    {
        SortRecord(m_StoryModePlayerRecords[E_DIFFICULTY.NORMAL]);
        return m_StoryModePlayerRecords[E_DIFFICULTY.NORMAL][0];
    }


    public List<PlayerRecord> GetStoryModeRecordsInRange(E_DIFFICULTY difficulty, int range)
    {
        var recs = m_StoryModePlayerRecords[difficulty];
        SortRecord(recs);

        var len = recs.Count;

        if(len < range)
        {
            var res = recs.GetRange(0, len);
            var rem = range - len;
            while (rem != 0)
            {
                res.Add(GetDummyRecord("Nanashi", 1, E_CHAPTER.CHAPTER_1, new System.DateTime(2019, 5, 1)));
                rem--;
            }
            return res;
        }
        else
        {
            return recs.GetRange(0, range);
        }
    }

    public List<PlayerRecord> GetChapterModeRecordsInRange(E_CHAPTER stage, int range)
    {
        var recs = m_ChapterModePlayerRecords[stage];
        SortRecord(recs);

        var len = recs.Count;

        if (len < range)
        {
            var res = recs.GetRange(0, len);
            var rem = range - len;
            while (rem != 0)
            {
                res.Add(GetDummyRecord("Nanashi", 1, stage, new System.DateTime(2019, 5, 1)));
                rem--;
            }
            return res;
        }
        else
        {
            return recs.GetRange(0, range);
        }
    }
}
