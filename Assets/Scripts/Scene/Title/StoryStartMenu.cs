using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E_SCENE = BaseSceneManager.E_SCENE;

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

    public void OnSubmitResetOptions()
    {
        m_LifeMenu?.ResetDefault();
        m_EnergyMenu?.ResetDefault();
    }

    public void OnSubmitStart()
    {
        DataManager.Instance.GameMode = E_GAME_MODE.STORY;
        DataManager.Instance.Difficulty = m_DifficultyMenu.Difficulty;
        DataManager.Instance.Chapter = E_CHAPTER.CHAPTER_0;
        DataManager.Instance.IsSelectedGame = true;
        BaseSceneManager.Instance.LoadScene(E_SCENE.STAGE0);
        AudioManager.Instance.Play(E_COMMON_SOUND.SYSTEM_START);
    }
}
