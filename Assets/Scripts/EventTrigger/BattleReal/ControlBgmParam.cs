using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ControlBgmParam
{
    public enum E_BGM_CONTROL_TYPE
    {
        PLAY,
        PLAY_IMMEDIATE,
        STOP,
        STOP_IMMEDIATE,
        CONTROL_AISAC,
    }

    public E_BGM_CONTROL_TYPE ControlType;

    public string PlayBgmName;

    public float FadeDuration;

    public AnimationCurve FadeOutCurve;

    public AnimationCurve FadeInCurve;

    public AudioManager.E_AISAC_TYPE AisacType;

    public float AisacValue;
}
