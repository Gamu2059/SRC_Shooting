#pragma warning disable 0649

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

    [SerializeField]
    private AisacSet[] m_AisacSets;
    public AisacSet[] AisacSets => m_AisacSets;

    [SerializeField]
    private E_CUE_SHEET[] m_BgmCueSheets;
    public E_CUE_SHEET[] BgmCueSheets => m_BgmCueSheets;

    [SerializeField]
    private E_CUE_SHEET[] m_SeCueSheets;
    public E_CUE_SHEET[] SeCueSheets => m_SeCueSheets;
}
