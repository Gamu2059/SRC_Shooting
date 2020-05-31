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
    [SerializeField, Tooltip("再生するキューシート")]
    private E_CUE_SHEET m_CueSheet;
    public E_CUE_SHEET CueSheet => m_CueSheet;

    [SerializeField, Tooltip("再生するサウンドのキューネーム")]
    private string m_CueName;
    public string CueName => m_CueName;

    [SerializeField, Tooltip("再生の開始時間(ms)")]
    private long m_StartTime = 0;
    public long StartTime => m_StartTime;

    [SerializeField, Tooltip("ピッチ")]
    private float m_Pitch = 0;
    public float Pitch => m_Pitch;
}
