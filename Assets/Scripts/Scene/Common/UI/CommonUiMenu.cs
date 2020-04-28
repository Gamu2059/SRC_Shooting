#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommonUiMenu : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private List<CommonUiMenuItem> m_MenuItems;
    protected List<CommonUiMenuItem> MenuItems => m_MenuItems;

    [SerializeField]
    private float m_WaitFocesMoveTime;
    protected float WaitFocesMoveTime => m_WaitFocesMoveTime;

    #endregion

    #region Field

    protected bool IsFocus { get; private set; }
    protected float WaitFocusMoveTimeCount { get; private set; }
    public bool IsAnimationFocusMove { get; private set; }

    public int CurrentFocusIndex { get; private set; }
    public CommonUiMenuItem CurrentFocusMenuItem { get; private set; }

    #endregion

    #region Open Callback

    public Action FocusItemAction;
    public Action SelectItemAction;
    public Action DefocusAllItemAction;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        WaitFocusMoveTimeCount = 0;
        CurrentFocusMenuItem = null;
        CurrentFocusIndex = -1;
        DefocusAllMenuItem(true);
        DefocusMenu();
    }

    public override void OnFinalize()
    {
        CurrentFocusIndex = -1;
        CurrentFocusMenuItem = null;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (IsFocus)
        {
            var vertical = Input.GetAxis("Vertical");
            if (vertical > 0)
            {
                // 上へ
                FocusMenuItem(CurrentFocusIndex - 1);
            }
            else if (vertical < 0)
            {
                // 下へ
                FocusMenuItem(CurrentFocusIndex + 1);
            }

            if (WaitFocusMoveTimeCount > 0)
            {
                WaitFocusMoveTimeCount -= Time.deltaTime;
                if (WaitFocusMoveTimeCount <= 0)
                {
                    IsAnimationFocusMove = false;
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// メニューをフォーカスする
    /// </summary>
    public virtual void FocusMenu()
    {
        IsFocus = true;
        FocusMenuItem(CurrentFocusIndex);
    }

    /// <summary>
    /// メニューをデフォーカスする
    /// </summary>
    public virtual void DefocusMenu()
    {
        IsFocus = false;
        //DefocusAllMenuItem(true);
    }

    /// <summary>
    /// 全ての項目をデフォーカスする
    /// </summary>
    public void DefocusAllMenuItem(bool isForce)
    {
        m_MenuItems?.ForEach(m => m.Defocus(isForce));
        DefocusAllItemAction?.Invoke();
    }

    /// <summary>
    /// 指定したインデックスの項目にフォーカスする
    /// </summary>
    public void FocusMenuItem(int index, bool isForce = false)
    {
        if (index < 0 || index >= m_MenuItems.Count)
        {
            return;
        }

        if (!isForce)
        {
            if (index == CurrentFocusIndex || IsAnimationFocusMove)
            {
                return;
            }
        }

        CurrentFocusMenuItem?.Defocus(false);
        CurrentFocusIndex = index;
        CurrentFocusMenuItem = m_MenuItems[CurrentFocusIndex];
        CurrentFocusMenuItem?.Focus(false);
        WaitFocusMoveTimeCount = m_WaitFocesMoveTime;
        IsAnimationFocusMove = true;
        FocusItemAction?.Invoke();
    }

    /// <summary>
    /// 現在フォーカスされている項目を選択する
    /// </summary>
    public void SelectMenuItem()
    {
        if (CurrentFocusMenuItem == null)
        {
            Debug.LogWarning("現在フォーカスされている項目がありません");
            return;
        }

        CurrentFocusMenuItem.Select();
        SelectItemAction?.Invoke();
    }

    public int GetMenuItemCount()
    {
        return m_MenuItems != null ? m_MenuItems.Count : 0;
    }
}
