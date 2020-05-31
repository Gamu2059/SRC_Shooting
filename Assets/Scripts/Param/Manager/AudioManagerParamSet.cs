using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/Common/AudioManagerParamSet", fileName = "param.audio.asset")]
public class AudioManagerParamSet : ScriptableObject
{
    [SerializeField]
    private AdxAssetParam m_AdxAssetParam;
    public AdxAssetParam AdxAssetParam => m_AdxAssetParam;

    [SerializeField]
    private CommonSoundParam m_CommonSoundParam;
    public CommonSoundParam CommonSoundParam => m_CommonSoundParam;
}
