#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DefaultTransition : TransitionController
{
    [SerializeField]
    private CanvasGroup m_CanvasGroup;

	[SerializeField]
	private float m_Duration;

	protected override IEnumerator OnHide( Action onComplete )
	{
        m_CanvasGroup.DOFade(1, m_Duration);
		yield return new WaitForSeconds( m_Duration );
		EventUtility.SafeInvokeAction( onComplete );
        m_CanvasGroup.alpha = 1;
	}

	protected override IEnumerator OnShow( Action onComplete )
    {
        m_CanvasGroup.alpha = 1;
		m_CanvasGroup.DOFade( 0, m_Duration );
		yield return new WaitForSeconds( m_Duration );
		EventUtility.SafeInvokeAction( onComplete );
	}
}
