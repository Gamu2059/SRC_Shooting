using System.Collections;
#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// BGM再生用のパラメータ
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sound/Play Sound", fileName = "param.play_sound.asset")]

public class PlaySoundParam : ScriptableObject
{
    [SerializeField]
    private E_CUE_SHEET m_CueSheet;
    public E_CUE_SHEET CueSheet => m_CueSheet;

    [SerializeField]
    private string m_CueName;
    public string CueName => m_CueName;
}
