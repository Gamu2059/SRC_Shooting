using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class SelectableColorTint : MonoBehaviour
{
    [SerializeField]
    private Selectable m_Target;

    [SerializeField]
    private Graphic m_GraphicTarget;

    [SerializeField]
    private Color m_NormalColor;

    [SerializeField]
    private Color m_FocusColor;

    [SerializeField]
    private Color m_DisableColor;

    private bool m_Interractable;
    private bool m_Select;

    private void Start()
    {
        m_Interractable = m_Target.IsInteractable();
        m_Target.OnSelectAsObservable().Subscribe(_ => OnSelect()).AddTo(this);
        m_Target.OnDeselectAsObservable().Subscribe(_ => OnDeselect()).AddTo(this);
        m_Select = false;
        Change();
    }

    private void Update()
    {
        var interractable = m_Target.IsInteractable();
        if (interractable != m_Interractable)
        {
            m_Interractable = interractable;
            Change();
        }  
    }

    private void OnSelect()
    {
        m_Select = true;
        Change();
    }

    private void OnDeselect()
    {
        m_Select = false;
        Change();
    }

    private void Change()
    {
        if (m_Interractable)
        {
            m_GraphicTarget.color = m_Select ? m_FocusColor : m_NormalColor;
        }
        else
        {
            m_GraphicTarget.color = m_DisableColor;
        }
    }
}
