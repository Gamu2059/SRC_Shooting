#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 汎用のメニューアイテムコンポーネント
/// </summary>
[Serializable]
public class CommonUiMenuItem : MonoBehaviour
{
    /// <summary>
    /// フォーカスされた時に呼び出される
    /// </summary>
    public void Focus(bool isForce)
    {
        OnFocus(isForce);
    }

    /// <summary>
    /// デフォーカスされた時に呼び出される
    /// </summary>
    public void Defocus(bool isForce)
    {
        OnDefocus(isForce);
    }

    /// <summary>
    /// 選択された時に呼び出される
    /// </summary>
    public void Select()
    {
        OnSelect();
    }

    protected virtual void OnFocus(bool isForce)
    {

    }

    protected virtual void OnDefocus(bool isForce)
    {

    }

    protected virtual void OnSelect()
    {

    }
}
