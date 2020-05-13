using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableAdxSoundController : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    [SerializeField]
    private E_COMMON_SOUND m_SelectSound;

    [SerializeField]
    private E_COMMON_SOUND m_SubmitSound;

    public void OnSelect(BaseEventData e)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(m_SelectSound);
        }
    }

    public void OnSubmit(BaseEventData e)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(m_SubmitSound);
        }
    }
}
