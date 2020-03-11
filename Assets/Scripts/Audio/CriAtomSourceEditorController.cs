using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// CriAtomSourceコンポーネントと同じオブジェクトにアタッチして使用して下さい。
/// </summary>
[RequireComponent(typeof(CriAtomSource))]
public class CriAtomSourceEditorController : MonoBehaviour
{
    private CriAtomSource m_Source;
    private bool m_IsMute;
    private float m_Volume;

    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(this);
        return;
#else
        m_Source = GetComponent<CriAtomSource>();
        if (m_Source == null)
        {
            Destroy(this);
            return;
        }

        m_Volume = m_Source.volume;
        m_IsMute = EditorUtility.audioMasterMute;
        EditorApplication.pauseStateChanged += OnPauseStateChanged;

        SetMute(m_IsMute);
#endif
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        EditorApplication.pauseStateChanged -= OnPauseStateChanged;
    }

    private void Update()
    {
        if (m_IsMute)
        {
            var volume = m_Source.volume;
            if (volume > 0 && m_Volume != volume)
            {
                m_Volume = volume;
                m_Source.volume = 0;
            }
        }

        var isMute = EditorUtility.audioMasterMute;
        if (m_IsMute != isMute)
        {
            m_IsMute = isMute;
            SetMute(m_IsMute);
        }
    }

    private void SetMute(bool isMute)
    {
        if (isMute)
        {
            m_Volume = m_Source.volume;
            m_Source.volume = 0;
        }
        else
        {
            m_Source.volume = m_Volume;
        }
    }

    private void OnPauseStateChanged(PauseState state)
    {
        m_Source.Pause(state == PauseState.Paused);
    }
#endif
}
