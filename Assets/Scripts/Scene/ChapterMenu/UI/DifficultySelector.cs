#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 難易度選択用のメニューセレクタ
/// </summary>
public class DifficultySelector : ControllableMonoBehavior
{
    #region Define

    private enum E_DIFFICULTY_MENU_STATE
    {
        FORCUS_EASY,
        SELECT_EASY,
        FORCUS_NORMAL,
        SELECT_NORMAL,
        FORCUS_HARD,
        SELECT_HARD,
        FORCUS_HADES,
        SELECT_HADES,
    }

    private class StateCycle : StateCycleBase<ChapterMenuManager, E_DIFFICULTY_MENU_STATE> { }

    private class ChapterMenuManagerState : State<E_DIFFICULTY_MENU_STATE, ChapterMenuManager>
    {
        public ChapterMenuManagerState(E_DIFFICULTY_MENU_STATE state, ChapterMenuManager target) : base(state, target) { }
        public ChapterMenuManagerState(E_DIFFICULTY_MENU_STATE state, ChapterMenuManager target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion
}
