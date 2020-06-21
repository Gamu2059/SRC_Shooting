using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// StoryStartMenuの制御クラス
/// </summary>
public class StoryStartMenu : MonoBehaviour
{
    [SerializeField]
    private ChoiceDifficultyMenuIndicator m_DifficultyMenu;

    [SerializeField]
    private ChoiceNumMenuIndicator m_LifeMenu;

    [SerializeField]
    private ChoiceNumMenuIndicator m_EnergyMenu;

    private bool m_IsSubmitStart = false;

    public void OnSubmitResetOptions()
    {
        m_LifeMenu?.ResetDefault();
        m_EnergyMenu?.ResetDefault();
    }

    public void OnSubmitStart()
    {
        if (m_IsSubmitStart)
        {
            return;
        }

        m_IsSubmitStart = true;
        DataManager.Instance.LifeOption = m_LifeMenu.Num;
        DataManager.Instance.EnergyOption = m_EnergyMenu.Num;
        DataManager.Instance.GameMode = E_GAME_MODE.STORY;
        DataManager.Instance.Difficulty = m_DifficultyMenu.Difficulty;
        DataManager.Instance.Chapter = E_CHAPTER.CHAPTER_0;
        DataManager.Instance.IsSelectedGame = true;
        DataManager.Instance.OnShootingStart();
        DataManager.Instance.TransitionToCurrentChapterScene();
        AudioManager.Instance.Play(E_COMMON_SOUND.SYSTEM_START);
    }

    private void Awake()
    {
        m_LifeMenu.SetNum(DataManager.Instance.LifeOption);
        m_EnergyMenu.SetNum(DataManager.Instance.EnergyOption);
    }
}
