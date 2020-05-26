using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/Common/SystemRecordManagerParamSet", fileName = "param.system_record.asset")]
public class SystemRecordManagerParamSet : ScriptableObject
{
    [SerializeField]
    private float m_InitBgmVolume;
    public float InitBgmVolume => m_InitBgmVolume;

    [SerializeField]
    private float m_InitSeVolume;
    public float InitSeVolume => m_InitSeVolume;
}
