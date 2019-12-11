using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ControlSoundParam
{
    public enum E_SOUND_CONTROL_TYPE
    {
        PLAY,
        STOP,
        CONTROL_AISAC,
    }

    public E_SOUND_CONTROL_TYPE ControlType;

    public PlaySoundParam PlaySoundParam;

    public E_CUE_SHEET StopSoundGroup;

    public OperateAisacParam OperateAisacParam;
}
