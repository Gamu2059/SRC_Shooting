using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

public class SettingMenu : MonoBehaviour
{
    #region Field Inspector

    [SerializeField]
    private Slider m_BgmSlider;

    [SerializeField]
    private Slider m_SeSlider;

    [SerializeField]
    private Button m_KeyConfigButton;

    #endregion

    private void Start()
    {
        m_BgmSlider.value = AudioManager.Instance.GetBgmVolume();
        m_SeSlider.value = AudioManager.Instance.GetSeVolume();

        m_BgmSlider.OnValueChangedAsObservable().Subscribe(ChangeBgmVolume).AddTo(this);
        m_SeSlider.OnValueChangedAsObservable().Subscribe(ChangeSeVolume).AddTo(this);

        m_KeyConfigButton.OnClickAsObservable().Subscribe(_ => OpenKeyConfig()).AddTo(this);
    }

    private void ChangeBgmVolume(float value)
    {
        AudioManager.Instance.SetBgmVolume(value);
    }

    private void ChangeSeVolume(float value)
    {
        AudioManager.Instance.SetSeVolume(value);
    }

    private void OpenKeyConfig()
    {
        ControlMapperManager.Instance?.Open();
    }
}
