using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterModeRankingTextSetManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_ElementRoot;

    [SerializeField]
    private ChapterRankingElement m_ElementPrefab;

    private List<ChapterRankingElement> m_Elements;

    private void Awake()
    {
        m_Elements = new List<ChapterRankingElement>();
        for (var i = 0; i < PlayerRecordManager.MAX_RECORD_NUM; i++)
        {
            var element = Instantiate(m_ElementPrefab, m_ElementRoot);
            m_Elements.Add(element);
        }
    }

    public void ShowRanking(E_CHAPTER chapter, E_DIFFICULTY difficulty)
    {
        var list = PlayerRecordManager.Instance.GetChapterModeRecords(chapter, difficulty);
        if (list == null || list.Count < 1)
        {
            m_Elements.ForEach(e => e.gameObject.SetActive(false));
        }

        for (var i = 0; i < PlayerRecordManager.MAX_RECORD_NUM; i++)
        {
            var e = m_Elements[i];
            if (i >= list.Count)
            {
                e.gameObject.SetActive(false);
                continue;
            }

            e.gameObject.SetActive(true);
            e.SetRanking(i + 1, list[i]);
        }
    }
}
