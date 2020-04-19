using System.Collections;
#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// AISAC操作用のパラメータ
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sound/Operate Aisac", fileName = "param.operate_aisac.asset")]
public class OperateAisacParam : ScriptableObject
{
    [SerializeField]
    private E_AISAC_TYPE m_AisacType;
    public E_AISAC_TYPE AisacType => m_AisacType;

    [SerializeField]
    private E_CUE_SHEET m_TargetCueSheet;
    public E_CUE_SHEET TargetCueSheet => m_TargetCueSheet;

    [SerializeField]
    private bool m_UseAnimationValue;
    public bool UseAnimationValue => m_UseAnimationValue;

    [SerializeField]
    private float m_TargetValue;
    public float TargetValue => m_TargetValue;

    [SerializeField]
    private AnimationCurve m_AnimationValue;
    public AnimationCurve AnimationValue => m_AnimationValue;
}
