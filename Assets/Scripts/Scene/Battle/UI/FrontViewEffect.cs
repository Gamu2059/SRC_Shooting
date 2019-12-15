#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リアルモードの画面エフェクトを制御する。
/// </summary>
public class FrontViewEffect : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private RawImage m_RawImage;

    [SerializeField]
    private AnimationCurve m_InverseEffectRadiusCurve;

    [SerializeField]
    private Material m_InverseEffect;

    #endregion

    #region Field

    private float m_Duration;
    private Material m_UseEffect;
    private bool m_IsAnimating;
    private float m_TimeCount;

    #endregion

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_UseEffect = Instantiate(m_InverseEffect);
        m_Duration = m_InverseEffectRadiusCurve.Duration();
    }

    public override void OnFinalize()
    {
        Destroy(m_UseEffect);

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsAnimating)
        {
            if (m_TimeCount > m_Duration)
            {
                StopEffect();
            }
            else
            {
                var radius = m_InverseEffectRadiusCurve.Evaluate(m_TimeCount);
                m_RawImage.material.SetFloat("_Radius", radius);
                m_TimeCount += Time.deltaTime;
            }
        }
    }

    public void PlayEffect(Vector2 centerPos)
    {
        if (m_IsAnimating)
        {
            return;
        }

        m_IsAnimating = true;
        m_TimeCount = 0;
        m_RawImage.material = m_UseEffect;
        m_RawImage.material.SetFloat("_PosX", centerPos.x);
        m_RawImage.material.SetFloat("_PosY", centerPos.y);
    }

    public void StopEffect()
    {
        m_IsAnimating = false;
        m_RawImage.material.SetFloat("_Radius", 100);
    }
}
