using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTypingAnimator : MonoBehaviour {

    [SerializeField]
    private Text m_Text;

    [SerializeField, Multiline]
    private string m_TargetText;

    [SerializeField]
    private float m_TypingSpeed = 10;

    [SerializeField]
    private int m_ForwardIndexNum = 2;

    private int m_CurrentIndex;
    private float m_TimeCount;
    private bool m_IsComplete;

    private void Start () {
        m_Text.text = "";
        m_TimeCount = 0;
        m_CurrentIndex = 0;
        m_IsComplete = false;
    }

    private void Update () {

        if (m_IsComplete)
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
        }

        m_TimeCount += Time.deltaTime;
        m_Text.text = m_TargetText.Substring(0, m_CurrentIndex);
        if (m_CurrentIndex >= m_TargetText.Length) {
            m_IsComplete = true;
        }
    }
}
