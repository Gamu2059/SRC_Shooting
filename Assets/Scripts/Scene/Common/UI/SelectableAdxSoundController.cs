using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SelectableAdxSoundController : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    [SerializeField]
    private Selectable m_Target;

    [SerializeField]
    private bool m_UseSelectSound;

    [SerializeField]
    private E_COMMON_SOUND m_SelectSound;

    [SerializeField]
    private bool m_UseSubmitSound;

    [SerializeField]
    private E_COMMON_SOUND m_SubmitSound;

    [SerializeField]
    private E_COMMON_SOUND m_DisableSound;

    private bool m_OnClicked = false;

    public void OnSelect(BaseEventData e)
    {
        if (AudioManager.Instance != null && m_UseSelectSound)
        {
            AudioManager.Instance.Play(m_SelectSound);
        }
    }

    public void OnSubmit(BaseEventData e)
    {
        if (m_OnClicked)
        {
            m_OnClicked = false;
            return;
        }

        if (AudioManager.Instance != null)
        {
            if (m_Target == null || m_Target.IsInteractable())
            {
                AudioManager.Instance.Play(m_SubmitSound);
            }
            else
            {
                AudioManager.Instance.Play(m_DisableSound);
            }
        }
    }

    public void OnClick()
    {
        if (AudioManager.Instance != null)
        {
            if (m_Target == null || m_Target.IsInteractable())
            {
                AudioManager.Instance.Play(m_SubmitSound);
            }
            else
            {
                AudioManager.Instance.Play(m_DisableSound);
            }
        }

        m_OnClicked = true;
    }
}
