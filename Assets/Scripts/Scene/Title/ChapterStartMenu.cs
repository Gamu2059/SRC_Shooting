using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E_SCENE = BaseSceneManager.E_SCENE;

/// <summary>
/// ChapterStartMenuの制御クラス
/// </summary>
public class ChapterStartMenu : MonoBehaviour
{
    private readonly Dictionary<E_CHAPTER, E_SCENE> m_Dict = new Dictionary<E_CHAPTER, E_SCENE>()
    {
        { E_CHAPTER.CHAPTER_0, E_SCENE.STAGE0 },
        { E_CHAPTER.CHAPTER_1, E_SCENE.STAGE1 },
        { E_CHAPTER.CHAPTER_2, E_SCENE.STAGE2 },
        { E_CHAPTER.CHAPTER_3, E_SCENE.STAGE3 },
        { E_CHAPTER.CHAPTER_4, E_SCENE.STAGE4 },
        { E_CHAPTER.CHAPTER_5, E_SCENE.STAGE5 },
        { E_CHAPTER.CHAPTER_6, E_SCENE.STAGE6 },
    };

    [SerializeField]
    private ChoiceDifficultyMenuIndicator m_DifficultyMenu;

    [SerializeField]
    private ChoiceChapterMenuIndicator m_ChapterMenu;

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
        DataManager.Instance.LifeOption = m_LifeMenu.Num;
        DataManager.Instance.EnergyOption = m_EnergyMenu.Num;
        DataManager.Instance.GameMode = E_GAME_MODE.CHAPTER;
        DataManager.Instance.Difficulty = m_DifficultyMenu.Difficulty;
        DataManager.Instance.Chapter = m_ChapterMenu.Chapter;
        DataManager.Instance.IsSelectedGame = true;
        BaseSceneManager.Instance.LoadScene(m_Dict[m_ChapterMenu.Chapter]);
        AudioManager.Instance.Play(E_COMMON_SOUND.SYSTEM_START);
    }

    private void Start()
    {
        m_LifeMenu.SetNum(DataManager.Instance.LifeOption);
        m_EnergyMenu.SetNum(DataManager.Instance.EnergyOption);
    }
}
