using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraRankingMenu : MonoBehaviour
{
    [SerializeField]
    private ChoiceRankingCategoryMenuIndicator m_CategoryMenu;

    [SerializeField]
    private ChoiceDifficultyMenuIndicator m_DifficultyMenu;

    [SerializeField]
    private StoryModeRankingTextSetManager m_StoryModeBoard;

    [SerializeField]
    private ChapterModeRankingTextSetManager m_ChapterModeBoard;

    private void Start()
    {
        m_CategoryMenu.OnChangeValue += ShowRanking;
        m_DifficultyMenu.OnChangeValue += ShowRanking;

        ShowRanking();
    }

    private void OnDestroy()
    {
        m_DifficultyMenu.OnChangeValue -= ShowRanking;
        m_CategoryMenu.OnChangeValue -= ShowRanking;
    }

    private void ShowRanking()
    {
        var category = m_CategoryMenu.Category;
        var difficulty = m_DifficultyMenu.Difficulty;

        if (category == E_RANKING_CATEGORY.STORY)
        {
            m_StoryModeBoard.gameObject.SetActive(true);
            m_ChapterModeBoard.gameObject.SetActive(false);
        }
        else
        {
            m_StoryModeBoard.gameObject.SetActive(false);
            m_ChapterModeBoard.gameObject.SetActive(true);
            m_ChapterModeBoard.ShowRanking(ToChapter(category), difficulty);
        }
    }

    private E_CHAPTER ToChapter(E_RANKING_CATEGORY category)
    {
        switch (category)
        {
            case E_RANKING_CATEGORY.CHAPTER_1: return E_CHAPTER.CHAPTER_1;
            case E_RANKING_CATEGORY.CHAPTER_2: return E_CHAPTER.CHAPTER_2;
            case E_RANKING_CATEGORY.CHAPTER_3: return E_CHAPTER.CHAPTER_3;
            case E_RANKING_CATEGORY.CHAPTER_4: return E_CHAPTER.CHAPTER_4;
            case E_RANKING_CATEGORY.CHAPTER_5: return E_CHAPTER.CHAPTER_5;
            case E_RANKING_CATEGORY.CHAPTER_6: return E_CHAPTER.CHAPTER_6;
            default: return E_CHAPTER.CHAPTER_0;
        }
    }
}
