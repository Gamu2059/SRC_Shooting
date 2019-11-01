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
        CONTROL_AISAC,
    }

    public E_BGM_CONTROL_TYPE ControlType;

    public string PlayBgmName;

    public AudioManager.E_AISAC_TYPE AisacType;

    public float AisacValue;
}
