using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 再生する音をEnumに対応させるもの。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sound/Common Sound Param", fileName = "param.common_sound_param.asset")]
public class CommonSoundParam : ScriptableObject
{
    [Serializable]
    public class Set
    {
        public E_COMMON_SOUND Type;
        public PlaySoundParam Param;
    }

    [SerializeField]
    private Set[] m_Sets;

    public Dictionary<E_COMMON_SOUND, PlaySoundParam> CreateCommonSoundDict()
    {
        var dict = new Dictionary<E_COMMON_SOUND, PlaySoundParam>();
        foreach (var commonSoundSet in m_Sets)
        {
            dict.Add(commonSoundSet.Type, commonSoundSet.Param);
        }

        return dict;
    }
}
