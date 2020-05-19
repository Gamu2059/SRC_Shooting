using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class OnChangeValueAdxSoundController : MonoBehaviour
{
    [SerializeField]
    private UIBehaviour m_Target;

    [SerializeField]
    private E_COMMON_SOUND m_ChangeValueSound;

    private void Start()
    {
        switch (m_Target)
        {
            case Toggle toggle:
                toggle.OnValueChangedAsObservable().Subscribe(_ => PlaySound()).AddTo(this);
                break;
            case Scrollbar scrollbar:
                scrollbar.OnValueChangedAsObservable().Subscribe(_ => PlaySound()).AddTo(this);
                break;
            case ScrollRect scrollRect:
                scrollRect.OnValueChangedAsObservable().Subscribe(_ => PlaySound()).AddTo(this);
                break;
            case Slider slider:
                slider.OnValueChangedAsObservable().Subscribe(_ => PlaySound()).AddTo(this);
                break;
            case InputField inputField:
                inputField.OnValueChangedAsObservable().Subscribe(_ => PlaySound()).AddTo(this);
                break;
            case Dropdown dropdown:
                dropdown.OnValueChangedAsObservable().Subscribe(_ => PlaySound()).AddTo(this);
                break;
        }
    }

    private void PlaySound()
    {
        AudioManager.Instance.Play(m_ChangeValueSound);
    }
}
