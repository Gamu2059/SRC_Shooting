using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// BGM再生用のパラメータ
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sound/Play BGM", fileName = "param.play_bgm.asset")]

public class PlayBgmParam : ScriptableObject
{
    [SerializeField]
    private E_CUE_SHEET m_CueSheet;
    public E_CUE_SHEET CueSheet => m_CueSheet;

    [SerializeField]
    private string m_BgmName;
    public string BgmName => m_BgmName;
}
