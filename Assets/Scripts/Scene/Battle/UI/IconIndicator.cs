#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アイコンを制御する
/// </summary>
public class IconIndicator : ControllableMonoBehavior
{
    private const string ICON_SCALE = "icon_scale";

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private Image m_Image;

    [SerializeField]
    private Color m_EnableColor;

    [SerializeField]
    private Color m_DisableColor;

    public bool IsEnable { get; private set; }

    public void PlayAnimation()
    {
        m_Animator.Play(ICON_SCALE);
    }

    public void SetEnable(bool isEnable)
    {
        IsEnable = isEnable;
        m_Image.color = IsEnable ? m_EnableColor : m_DisableColor;
    }
}
