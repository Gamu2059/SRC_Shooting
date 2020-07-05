using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// スコア・ステージ進度などの記録を管理するマネージャ
/// </summary>
public class PlayerRecordManager : SingletonMonoBehavior<PlayerRecordManager>
{
    public const int MAX_RECORD_NUM = 10;

    private readonly E_DIFFICULTY[] Difficulties = new E_DIFFICULTY[]
    {
        E_DIFFICULTY.EASY,
        E_DIFFICULTY.NORMAL,
        E_DIFFICULTY.HARD,
        E_DIFFICULTY.HADES,
    };

    private readonly E_CHAPTER[] Chapters = new E_CHAPTER[]
    {
        E_CHAPTER.CHAPTER_1,
        E_CHAPTER.CHAPTER_2,
        E_CHAPTER.CHAPTER_3,
        E_CHAPTER.CHAPTER_4,
        E_CHAPTER.CHAPTER_5,
        E_CHAPTER.CHAPTER_6,
    };

    #region Field Inspector

    [SerializeField]
    private PlayerRecordManagerParamSet m_ParamSet;

    #endregion

    //private Dictionary<E_DIFFICULTY, List<PlayerRecord>> m_StoryModePlayerRecords;
    private Dictionary<(E_CHAPTER, E_DIFFICULTY), List<PlayerRecord>> m_ChapterModePlayerRecords;

    public override void OnInitialize()
    {
        m_ChapterModePlayerRecords = new Dictionary<(E_CHAPTER, E_DIFFICULTY), List<PlayerRecord>>();

        LoadRecords();
    }

    private void LoadRecords()
    {
        SaveDataManager.Load();

        // チャプターデータの読み込み
        foreach (var difficulty in Difficulties)
        {
            foreach (var chapter in Chapters)
            {
                var key = string.Format("Chapter_{0}_{1}", chapter, difficulty);
                var list = SaveDataManager.GetList(key, new List<PlayerRecord>());
                m_ChapterModePlayerRecords.Add((chapter, difficulty), list);
            }
        }
    }

    private void SaveRecords(E_CHAPTER chapter, E_DIFFICULTY difficulty)
    {
        var list = GetRecordList(chapter, difficulty);
        var key = string.Format("Chapter_{0}_{1}", chapter, difficulty);
        SaveDataManager.SetList(key, list);
        SaveDataManager.Save();
    }

    private List<PlayerRecord> GetRecordList(E_CHAPTER chapter, E_DIFFICULTY difficulty)
    {
        if (!m_ChapterModePlayerRecords.TryGetValue((chapter, difficulty), out var list))
        {
            list = new List<PlayerRecord>();
            m_ChapterModePlayerRecords.Add((chapter, difficulty), list);
        }

        return list;
    }

    public void AddStoryModeRecord(PlayerRecord record)
    {
        //void f(E_DIFFICULTY d) {
        //    var recs = m_StoryModePlayerRecords[d];

        //    var maxScore = GetTopRecord(recs).m_FinalScore;
        //    if(record.m_FinalScore > maxScore)
        //    {
        //        SaveDataManager.SetInt("BestScore", (int)record.m_FinalScore);
        //        SaveDataManager.Save();
        //    }

        //    if(recs.Count + 1 > m_MaxRecordNum)
        //    {
        //        recs.RemoveAt(recs.Count - 1);
        //        recs.Add(record);
        //        SortRecord(recs);
        //    }
        //    else
        //    {
        //        recs.Add(record);
        //        SortRecord(recs);
        //    }
        //}

        //f(record.StageDifficulty());
    }

    public void AddChapterModeRecord(E_CHAPTER chapter, E_DIFFICULTY difficulty, PlayerRecord record)
    {
        if (record == null)
        {
            return;
        }

        Debug.LogFormat("AddChapterModeRecord  score : {0}", record.FinalScore);
        var list = GetRecordList(chapter, difficulty);
        list.Add(record);
        SortRecord(list);
        if (list.Count > MAX_RECORD_NUM)
        {
            list.RemoveAt(MAX_RECORD_NUM);
        }

        SaveRecords(chapter, difficulty);
    }

    private void SortRecord(List<PlayerRecord> records)
    {
        records.Sort((a, b) =>
        {
            if (b.FinalScore > a.FinalScore)
            {
                return 1;
            }

            if (b.FinalScore < a.FinalScore)
            {
                return -1;
            }

            return 0;
        });
    }

    //public PlayerRecord GetDummyRecord(string name = "Nanashi", ulong score = 1, E_CHAPTER stage = E_CHAPTER.CHAPTER_1, System.DateTime date = new System.DateTime())
    //{
    //    return new PlayerRecord(name, score, stage, date);
    //}

    //public PlayerRecord GetTopRecord(List<PlayerRecord> recs)
    //{
    //    SortRecord(recs);
    //    return recs[0];
    //}

    //public PlayerRecord GetTopRecord()
    //{
    //    SortRecord(m_StoryModePlayerRecords[E_DIFFICULTY.NORMAL]);
    //    return m_StoryModePlayerRecords[E_DIFFICULTY.NORMAL][0];
    //}


    public List<PlayerRecord> GetStoryModeRecordsInRange(E_DIFFICULTY difficulty, int range)
    {
        //var recs = m_StoryModePlayerRecords[difficulty];
        //SortRecord(recs);

        //var len = recs.Count;

        //if(len < range)
        //{
        //    var res = recs.GetRange(0, len);
        //    var rem = range - len;
        //    while (rem != 0)
        //    {
        //        res.Add(GetDummyRecord("Nanashi", 1, E_CHAPTER.CHAPTER_1, new System.DateTime(2019, 5, 1)));
        //        rem--;
        //    }
        //    return res;
        //}
        //else
        //{
        //    return recs.GetRange(0, range);
        //}
        return null;
    }

    public List<PlayerRecord> GetChapterModeRecords(E_CHAPTER chapter, E_DIFFICULTY difficulty)
    {
        var list = GetRecordList(chapter, difficulty);
        SortRecord(list);
        return list;
    }

    public ulong GetChapterModeBestScore(E_CHAPTER chapter, E_DIFFICULTY difficulty)
    {
        var list = GetChapterModeRecords(chapter, difficulty);
        if (list == null || list.Count < 1)
        {
            return 0;
        }

        return list[0].FinalScore;
    }
}
