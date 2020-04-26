#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アニメーションするメニューアイテム
/// </summary>
public class TextAnimationUiMenuItem : CommonUiMenuItem
{
    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private string m_EnableName;

    [SerializeField]
    private string m_EnableForceName;

    [SerializeField]
    private string m_DisableName;

    [SerializeField]
    private string m_DisableForceName;

    protected override void OnFocus(bool isForce)
    {
        base.OnFocus(isForce);
        m_Animator?.Play(isForce ? m_EnableForceName : m_EnableName, 0);
    }

    protected override void OnDefocus(bool isForce)
    {
        base.OnDefocus(isForce);
        m_Animator?.Play(isForce ? m_DisableForceName : m_DisableName, 0);
    }
}
