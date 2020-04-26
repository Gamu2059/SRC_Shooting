#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterMenuManager : ControllableMonoBehavior
{
    #region Define

    private enum E_STATE
    {
        FOCUS_DIFFICULTY,
        FOCUS_CHAPTER,
        FOCUS_OPTION,
        FOCUS_BACK,
        FOCUS_START,
    }

    private class StateCycle : StateCycleBase<ChapterMenuManager, E_STATE> { }

    private class InnerState : State<E_STATE, ChapterMenuManager>
    {
        public InnerState(E_STATE state, ChapterMenuManager target) : base(state, target) { }
        public InnerState(E_STATE state, ChapterMenuManager target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field Inspector

    [SerializeField]
    private ChapterMenuUIManager m_UiManager;

    [SerializeField]
    private PlaySoundParam m_CursorSe;

    [SerializeField]
    private PlaySoundParam m_OkSe;

    [SerializeField]
    private PlaySoundParam m_CancelSe;

    [SerializeField]
    private PlaySoundParam m_StartSe;

    #endregion

    #region Field

    private TwoAxisInputManager InputManager;
    private StateMachine<E_STATE, ChapterMenuManager> m_StateMachine;
    private E_STATE m_PreBackState;
    private bool m_IsSelectedDifficulty;
    private bool m_IsSelectedChapter;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_STATE, ChapterMenuManager>();
        m_StateMachine.AddState(new InnerState(E_STATE.FOCUS_DIFFICULTY, this, new FocusDifficultyState()));
        m_StateMachine.AddState(new InnerState(E_STATE.FOCUS_CHAPTER, this, new FocusChapterState()));
        m_StateMachine.AddState(new InnerState(E_STATE.FOCUS_OPTION, this, new FocusOptionState()));
        m_StateMachine.AddState(new InnerState(E_STATE.FOCUS_BACK, this, new FocusBackState()));
        m_StateMachine.AddState(new InnerState(E_STATE.FOCUS_START, this, new FocusStartState()));

        InputManager = new TwoAxisInputManager();
        InputManager.OnInitialize();

        m_UiManager.OnInitialize();
        m_UiManager.DifficultyMenu.FocusMenuItem(0, true);

        RequestChangeState(E_STATE.FOCUS_DIFFICULTY);
    }

    public override void OnFinalize()
    {
        InputManager.RemoveInput();

        m_UiManager.OnFinalize();
        m_StateMachine.OnFinalize();
        InputManager.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        InputManager.OnStart();
        InputManager.RegistInput();
        m_UiManager.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        InputManager.OnUpdate();
        m_StateMachine.OnUpdate();
    }

    #endregion

    private void PlayCursor()
    {
        AudioManager.Instance.Play(m_CursorSe);
    }

    private void PlayOk()
    {
        AudioManager.Instance.Play(m_OkSe);
    }

    private void PlayCancel()
    {
        AudioManager.Instance.Play(m_CancelSe);
    }

    private void PlayStart()
    {
        AudioManager.Instance.Play(m_StartSe);
    }

    #region Focus Difficulty State

    private class FocusDifficultyState : StateCycle
    {
        private CommonUiMenu m_Menu;

        public override void OnStart()
        {
            base.OnStart();
            m_Menu = Target.m_UiManager.DifficultyMenu;
            m_Menu.FocusMenu();
            m_Menu.FocusItemAction += Target.PlayCursor;

            if (Target.IsReadyEnableStartButton())
            {
                Target.m_UiManager.StartButton.EnableButton();
            }
            else
            {
                Target.m_UiManager.StartButton.DisableButton();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Target.IsMoveDownOutOfRange(m_Menu))
            {
                Target.RequestChangeState(E_STATE.FOCUS_BACK);
                Target.m_PreBackState = E_STATE.FOCUS_DIFFICULTY;
                Target.m_IsSelectedDifficulty = false;
            }
            else if (Target.CheckSelectAction(() => Target.RequestChangeState(E_STATE.FOCUS_CHAPTER)))
            {
                Target.m_IsSelectedDifficulty = true;
                return;
            }
            else if (Target.CheckCancelAction(() => Target.RequestChangeState(E_STATE.FOCUS_BACK)))
            {
                Target.m_PreBackState = E_STATE.FOCUS_DIFFICULTY;
                Target.m_IsSelectedDifficulty = false;
                return;
            }

            Target.m_UiManager.OnUpdate();
        }

        public override void OnEnd()
        {
            m_Menu.FocusItemAction -= Target.PlayCursor;
            m_Menu.DefocusMenu();
            base.OnEnd();
        }
    }

    #endregion

    #region Focus Chapter State

    private class FocusChapterState : StateCycle
    {
        private CommonUiMenu m_Menu;

        public override void OnStart()
        {
            base.OnStart();
            m_Menu = Target.m_UiManager.ChapterMenu;
            m_Menu.FocusMenu();
            m_Menu.FocusMenuItem(0, true);
            m_Menu.FocusItemAction += Target.PlayCursor;

            if (Target.IsReadyEnableStartButton())
            {
                Target.m_UiManager.StartButton.EnableButton();
            }
            else
            {
                Target.m_UiManager.StartButton.DisableButton();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Target.IsMoveDownOutOfRange(m_Menu))
            {
                Target.RequestChangeState(E_STATE.FOCUS_BACK);
                Target.m_PreBackState = E_STATE.FOCUS_CHAPTER;
                Target.m_IsSelectedChapter = false;
                return;
            }
            else if (Target.CheckSelectAction(() => Target.RequestChangeState(E_STATE.FOCUS_OPTION)))
            {
                Target.m_IsSelectedChapter = true;
                return;
            }
            else if (Target.CheckCancelAction(() => Target.RequestChangeState(E_STATE.FOCUS_DIFFICULTY)))
            {
                Target.m_IsSelectedChapter = false;
                return;
            }

            Target.m_UiManager.OnUpdate();
        }

        public override void OnEnd()
        {
            m_Menu.FocusItemAction -= Target.PlayCursor;
            m_Menu.DefocusMenu();
            base.OnEnd();
        }
    }

    #endregion

    #region Focus Option State

    private class FocusOptionState : StateCycle
    {
        private CommonUiMenu m_Menu;

        public override void OnStart()
        {
            base.OnStart();
            m_Menu = Target.m_UiManager.OptionMenu;
            m_Menu.FocusMenu();
            m_Menu.FocusMenuItem(0, true);
            m_Menu.FocusItemAction += Target.PlayCursor;

            if (Target.IsReadyEnableStartButton())
            {
                Target.m_UiManager.StartButton.EnableButton();
            }
            else
            {
                Target.m_UiManager.StartButton.DisableButton();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Target.IsMoveDownOutOfRange(m_Menu))
            {
                Target.RequestChangeState(E_STATE.FOCUS_BACK);
                Target.m_PreBackState = E_STATE.FOCUS_OPTION;
                return;
            }
            else if (Target.CheckSelectAction(() => Target.RequestChangeState(E_STATE.FOCUS_START)))
            {
                return;
            }
            else if (Target.CheckCancelAction(() => Target.RequestChangeState(E_STATE.FOCUS_CHAPTER)))
            {
                return;
            }

            Target.m_UiManager.OnUpdate();
        }

        public override void OnEnd()
        {
            m_Menu.FocusItemAction -= Target.PlayCursor;
            m_Menu.DefocusMenu();
            base.OnEnd();
        }
    }

    #endregion

    #region Focus Back State

    private class FocusBackState : StateCycle
    {
        private CommonUiStatedButton m_Button;

        public override void OnStart()
        {
            base.OnStart();
            m_Button = Target.m_UiManager.BackButton;
            m_Button.FocusButton();

            if (Target.IsReadyEnableStartButton())
            {
                Target.m_UiManager.StartButton.EnableButton();
            }
            else
            {
                Target.m_UiManager.StartButton.DisableButton();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Target.CheckSelectAction(() => Target.ExitScene()))
            {
                return;
            }
            else if (Target.InputManager.MoveDir.y > 0)
            {
                Target.PlayOk();
                Target.RequestChangeState(Target.m_PreBackState);
                return;
            }
            else if (Target.InputManager.MoveDir.x > 0)
            {
                if (Target.IsReadyEnableStartButton())
                {
                    Target.PlayOk();
                    Target.RequestChangeState(E_STATE.FOCUS_START);
                    return;
                }
            }

            Target.m_UiManager.OnUpdate();
        }

        public override void OnEnd()
        {
            m_Button.DefocusButton();
            base.OnEnd();
        }
    }

    #endregion

    #region Focus Start State

    private class FocusStartState : StateCycle
    {
        private CommonUiStatedButton m_Button;

        public override void OnStart()
        {
            base.OnStart();
            m_Button = Target.m_UiManager.StartButton;
            m_Button.FocusButton();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Target.CheckSelectAction(() => Target.GameStart()))
            {
                return;
            }
            else if (Target.CheckCancelAction(() => Target.RequestChangeState(E_STATE.FOCUS_OPTION)))
            {
                return;
            }
            else if (Target.InputManager.MoveDir.y > 0)
            {
                Target.PlayOk();
                Target.RequestChangeState(E_STATE.FOCUS_OPTION);
                return;
            }
            else if (Target.InputManager.MoveDir.x < 0)
            {
                Target.PlayCancel();
                Target.RequestChangeState(E_STATE.FOCUS_BACK);
                return;
            }

            Target.m_UiManager.OnUpdate();
        }

        public override void OnEnd()
        {
            m_Button.DefocusButton();
            base.OnEnd();
        }
    }

    #endregion

    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    private bool IsReadyEnableStartButton()
    {
        return m_IsSelectedDifficulty && m_IsSelectedChapter;
    }

    /// <summary>
    /// メニューの項目数を超えて下に移動しようとしたかどうか
    /// </summary>
    private bool IsMoveDownOutOfRange(CommonUiMenu menu)
    {
        if (menu == null || menu.IsAnimationFocusMove)
        {
            return false;
        }

        var moveDir = InputManager.MoveDir;
        return moveDir.y < 0 && menu.CurrentFocusIndex >= menu.GetMenuItemCount() - 1;
    }

    private bool CheckSelectAction(Action onSelect)
    {
        if (InputManager.Submit == E_INPUT_STATE.DOWN)
        {
            if (onSelect != null)
            {
                PlayOk();
                onSelect.Invoke();
            }

            return true;
        }

        return false;
    }

    private bool CheckCancelAction(Action onCancel)
    {
        if (InputManager.Cancel == E_INPUT_STATE.DOWN)
        {
            if (onCancel != null)
            {
                PlayCancel();
                onCancel.Invoke();
            }

            return true;
        }

        return false;
    }

    private void CheckStartAction(BaseSceneManager.E_SCENE scene)
    {
        PlayStart();
        BaseSceneManager.Instance.LoadScene(scene);
        InputManager.RemoveInput();
    }

    private void GameStart()
    {
        InputManager.RemoveInput();

        var difficultyIdx = m_UiManager.DifficultyMenu.CurrentFocusIndex;
        var chapterIdx = m_UiManager.ChapterMenu.CurrentFocusIndex;

        E_DIFFICULTY difficulty = E_DIFFICULTY.EASY;
        switch (difficultyIdx)
        {
            case 0:
                difficulty = E_DIFFICULTY.EASY;
                break;
            case 1:
                difficulty = E_DIFFICULTY.NORMAL;
                break;
            case 2:
                difficulty = E_DIFFICULTY.HARD;
                break;
            case 3:
                difficulty = E_DIFFICULTY.HADES;
                break;
        }

        E_CHAPTER chapter = E_CHAPTER.CHAPTER_1;
        switch (chapterIdx)
        {
            case 0:
                chapter = E_CHAPTER.CHAPTER_0;
                break;
            case 1:
                chapter = E_CHAPTER.CHAPTER_1;
                break;
            case 2:
                chapter = E_CHAPTER.CHAPTER_2;
                break;
            case 3:
                chapter = E_CHAPTER.CHAPTER_3;
                break;
            case 4:
                chapter = E_CHAPTER.CHAPTER_4;
                break;
            case 5:
                chapter = E_CHAPTER.CHAPTER_5;
                break;
            case 6:
                chapter = E_CHAPTER.CHAPTER_6;
                break;
        }

        DataManager.Instance.GameMode = E_GAME_MODE.CHAPTER;
        DataManager.Instance.Difficulty = difficulty;
        DataManager.Instance.Chapter = chapter;
        DataManager.Instance.IsSelectedGame = true;

        switch (chapter)
        {
            case E_CHAPTER.CHAPTER_0:
                BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE0);
                break;
            case E_CHAPTER.CHAPTER_1:
                BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
                break;
            case E_CHAPTER.CHAPTER_2:
                BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE2);
                break;
            case E_CHAPTER.CHAPTER_3:
                BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE3);
                break;
            case E_CHAPTER.CHAPTER_4:
                BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE4);
                break;
            case E_CHAPTER.CHAPTER_5:
                BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE5);
                break;
            case E_CHAPTER.CHAPTER_6:
                BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE6);
                break;
        }
    }

    private void ExitScene()
    {
        InputManager.RemoveInput();
        BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
    }

    //#region Forcus Chap0

    //private void StartOnForcusChap0()
    //{
    //    m_UiManager.ForcusMenu(0);
    //}

    //private void UpdateOnForcusChap0()
    //{
    //    CheckForcusActionVertical(null, ()=>m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_1));
    //    CheckSelectAction(()=>m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_0));
    //    CheckCancelAction(() => ExitScene());
    //}

    //private void EndOnForcusChap0()
    //{

    //}

    //#endregion

    //#region Select Chap0

    //private void StartOnSelectChap0()
    //{
    //    Debug.Log("Chapter0が選択されました");
    //    // m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_0);
    //    CheckStartAction(BaseSceneManager.E_SCENE.STAGE0);
    //}

    //private void UpdateOnSelectChap0()
    //{

    //}

    //private void EndOnSelectChap0()
    //{

    //}

    //#endregion

    //#region Forcus Chap1

    //private void StartOnForcusChap1()
    //{
    //    m_UiManager.ForcusMenu(1);
    //}

    //private void UpdateOnForcusChap1()
    //{
    //    CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_0), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_2));
    //    CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_1));
    //    CheckCancelAction(() => ExitScene());
    //}

    //private void EndOnForcusChap1()
    //{

    //}

    //#endregion

    //#region Select Chap1

    //private void StartOnSelectChap1()
    //{
    //    Debug.Log("Chapter1が選択されました");
    //    // m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_1);
    //    CheckStartAction(BaseSceneManager.E_SCENE.STAGE1);
    //}

    //private void UpdateOnSelectChap1()
    //{

    //}

    //private void EndOnSelectChap1()
    //{

    //}

    //#endregion

    //#region Forcus Chap2

    //private void StartOnForcusChap2()
    //{
    //    m_UiManager.ForcusMenu(2);
    //}

    //private void UpdateOnForcusChap2()
    //{
    //    CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_1), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_3));
    //    CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_2));
    //    CheckCancelAction(() => ExitScene());
    //}

    //private void EndOnForcusChap2()
    //{

    //}

    //#endregion

    //#region Select Chap2

    //private void StartOnSelectChap2()
    //{
    //    Debug.Log("Chapter2が選択されました");
    //    // m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_2);
    //    CheckStartAction(BaseSceneManager.E_SCENE.STAGE2);
    //}

    //private void UpdateOnSelectChap2()
    //{

    //}

    //private void EndOnSelectChap2()
    //{

    //}

    //#endregion

    //#region Forcus Chap3

    //private void StartOnForcusChap3()
    //{
    //    m_UiManager.ForcusMenu(3);
    //}

    //private void UpdateOnForcusChap3()
    //{
    //    CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_2), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_4));
    //    CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_3));
    //    CheckCancelAction(() => ExitScene());
    //}

    //private void EndOnForcusChap3()
    //{

    //}

    //#endregion

    //#region Select Chap3

    //private void StartOnSelectChap3()
    //{
    //    Debug.Log("Chapter3が選択されました");
    //    // m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_3);
    //    CheckStartAction(BaseSceneManager.E_SCENE.STAGE3);
    //}

    //private void UpdateOnSelectChap3()
    //{

    //}

    //private void EndOnSelectChap3()
    //{

    //}

    //#endregion

    //#region Forcus Chap4

    //private void StartOnForcusChap4()
    //{
    //    m_UiManager.ForcusMenu(4);
    //}

    //private void UpdateOnForcusChap4()
    //{
    //    CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_3), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_5));
    //    CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_4));
    //    CheckCancelAction(() => ExitScene());
    //}

    //private void EndOnForcusChap4()
    //{

    //}

    //#endregion

    //#region Select Chap4

    //private void StartOnSelectChap4()
    //{
    //    Debug.Log("Chapter4が選択されました");
    //    // m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_4);
    //    CheckStartAction(BaseSceneManager.E_SCENE.STAGE4);
    //}

    //private void UpdateOnSelectChap4()
    //{

    //}

    //private void EndOnSelectChap4()
    //{

    //}

    //#endregion

    //#region Forcus Chap5

    //private void StartOnForcusChap5()
    //{
    //    m_UiManager.ForcusMenu(5);
    //}

    //private void UpdateOnForcusChap5()
    //{
    //    CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_4), () => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_6));
    //    CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_5));
    //    CheckCancelAction(() => ExitScene());
    //}

    //private void EndOnForcusChap5()
    //{

    //}

    //#endregion

    //#region Select Chap5

    //private void StartOnSelectChap5()
    //{
    //    Debug.Log("Chapter5が選択されました");
    //    // m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_5);
    //    CheckStartAction(BaseSceneManager.E_SCENE.STAGE5);
    //}

    //private void UpdateOnSelectChap5()
    //{

    //}

    //private void EndOnSelectChap5()
    //{

    //}

    //#endregion

    //#region Forcus Chap6

    //private void StartOnForcusChap6()
    //{
    //    m_UiManager.ForcusMenu(6);
    //}

    //private void UpdateOnForcusChap6()
    //{
    //    CheckForcusActionVertical(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_5), null);
    //    CheckSelectAction(() => m_StateMachine.Goto(E_CHAPTER_MENU_STATE.SELECT_CHAP_6));
    //    CheckCancelAction(() => ExitScene());
    //}

    //private void EndOnForcusChap6()
    //{

    //}

    //#endregion

    //#region Select Chap6

    //private void StartOnSelectChap6()
    //{
    //    Debug.Log("Chapter6が選択されました");
    //    // m_StateMachine.Goto(E_CHAPTER_MENU_STATE.FORCUS_CHAP_6);
    //    CheckStartAction(BaseSceneManager.E_SCENE.STAGE6);
    //}

    //private void UpdateOnSelectChap6()
    //{

    //}

    //private void EndOnSelectChap6()
    //{

    //}

    //#endregion
}
