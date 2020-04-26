#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterMenuUIManager : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private CommonUiStatedButton m_BackButton;
    public CommonUiStatedButton BackButton => m_BackButton;

    [SerializeField]
    private CommonUiStatedButton m_StartButton;
    public CommonUiStatedButton StartButton => m_StartButton;

    [SerializeField]
    private CommonUiMenu m_DifficultyMenu;
    public CommonUiMenu DifficultyMenu => m_DifficultyMenu;

    [SerializeField]
    private CommonUiMenu m_ChapterMenu;
    public CommonUiMenu ChapterMenu => m_ChapterMenu;

    [SerializeField]
    private CommonUiMenu m_OptionMenu;
    public CommonUiMenu OptionMenu => m_OptionMenu;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_DifficultyMenu.OnInitialize();
        m_ChapterMenu.OnInitialize();
        m_OptionMenu.OnInitialize();
        m_BackButton.OnInitialize();
        m_StartButton.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_StartButton.OnFinalize();
        m_BackButton.OnFinalize();
        m_OptionMenu.OnFinalize();
        m_ChapterMenu.OnFinalize();
        m_DifficultyMenu.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_DifficultyMenu.OnUpdate();
        m_ChapterMenu.OnUpdate();
        m_OptionMenu.OnUpdate();
        m_BackButton.OnUpdate();
        m_StartButton.OnUpdate();
    }

    #endregion
}
