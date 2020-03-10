#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// カットシーンを制御する。
/// </summary>
public class CutsceneManager : SingletonMonoBehavior<CutsceneManager>
{
    /// <summary>
    /// カットシーン終了時に呼び出されるコールバック
    /// </summary>
    public Action OnCompleteCutscene;

    #region Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        OnCompleteCutscene?.Invoke();
        OnCompleteCutscene = null;
        base.OnFinalize();
    }

    #endregion
}
