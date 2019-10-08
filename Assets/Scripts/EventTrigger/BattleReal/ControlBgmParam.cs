using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ControlBgmParam
{
    public enum E_BGM_CONTROL_TYPE
    {
        PLAY,
        STOP,
    }

    public E_BGM_CONTROL_TYPE ControlType;

    public AudioClip BgmClip;

    public float FadeOutDuration;

    public float FadeInStartOffset;

    public float FadeInDuration;
}
