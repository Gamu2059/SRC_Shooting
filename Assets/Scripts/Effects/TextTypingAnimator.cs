#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTypingAnimator : ControllableMonoBehavior
{

    [SerializeField]
    private Text m_Text;

    [SerializeField, Multiline]
    private string m_TargetText;

    [SerializeField]
    private float m_TypingSpeed = 10;

    [SerializeField]
    private int m_ForwardIndexNum = 2;

    [SerializeField]
    private PlaySoundParam m_TypingSe;

    private int m_CurrentIndex;
    private float m_TimeCount;
    private bool m_IsComplete;

    private bool m_IsAnimating;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_Text.text = "";
        m_TimeCount = 0;
        m_CurrentIndex = 0;
        m_IsComplete = false;
        m_IsAnimating = false;
    }

    public override void OnUpdate()
    {

        if (!m_IsAnimating || m_IsComplete)
        {
            return;
        }

        if (m_TimeCount >= m_TypingSpeed)
        {
            m_TimeCount -= m_TypingSpeed;
            m_CurrentIndex += m_ForwardIndexNum;
            if (m_CurrentIndex > m_TargetText.Length)
            {
                m_CurrentIndex = m_TargetText.Length;
            }
            AudioManager.Instance.Play(m_TypingSe);
        }

        m_TimeCount += Time.unscaledDeltaTime;
        m_Text.text = m_TargetText.Substring(0, m_CurrentIndex);
        if (m_CurrentIndex >= m_TargetText.Length)
        {
            m_IsComplete = true;
        }
    }

    #endregion

    public void StartAnimation()
    {
        m_IsAnimating = true;
    }

    public void StopAnimation()
    {
        m_IsAnimating = false;
    }
}
