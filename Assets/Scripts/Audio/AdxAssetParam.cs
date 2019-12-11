using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ADX2のパラメータをアセットとして分離したもの。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sound/Adx Param", fileName = "param.adx_param.asset")]
public class AdxAssetParam : ScriptableObject
{
    [Serializable]
    public struct AisacSet
    {
        public E_AISAC_TYPE AisacType;
        public string Name;
    }

    [Serializable]
    public struct CueSet
    {
        public E_CUE_NAME CueName;
        public E_CUE_SHEET BelongCueSheet;
        public string Name;
    }

    [SerializeField]
    private AisacSet[] m_AisacSets;
    public AisacSet[] AisacSets => m_AisacSets;

    [SerializeField]
    private CueSet[] m_CueSets;
    public CueSet[] CueSets => m_CueSets;
}
