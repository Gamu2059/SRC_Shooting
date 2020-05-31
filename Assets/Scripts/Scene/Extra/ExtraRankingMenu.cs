using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraRankingMenu : MonoBehaviour
{
    [SerializeField]
    private ChoiceRankingCategoryMenuIndicator m_CategoryMenu;

    [SerializeField]
    private ChoiceDifficultyMenuIndicator m_DifficultyMenu;

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
        //m_CategoryMenu.Category;
        //m_DifficultyMenu.Difficulty;
    }
}
