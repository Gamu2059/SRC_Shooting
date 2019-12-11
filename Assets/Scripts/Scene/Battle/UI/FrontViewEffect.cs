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

    private Material m_UseInverseEffect;
    private bool m_IsAnimatingInverseEffect;
    private float m_InverseEffectTimeCount;

    #endregion

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_UseInverseEffect = Instantiate(m_InverseEffect);
    }

    public override void OnFinalize()
    {
        Destroy(m_UseInverseEffect);

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsAnimatingInverseEffect)
        {
            var maxTime = m_InverseEffectRadiusCurve.keys[m_InverseEffectRadiusCurve.keys.Length - 1].time;
            if (m_InverseEffectTimeCount > maxTime)
            {
                StopInverseEffect();
            }
            else
            {
                var radius = m_InverseEffectRadiusCurve.Evaluate(m_InverseEffectTimeCount);
                m_RawImage.material.SetFloat("_Radius", radius);
                m_InverseEffectTimeCount += Time.deltaTime;
            }
        }
    }

    public void PlayInverseEffect(Vector2 centerPos)
    {
        if (m_IsAnimatingInverseEffect)
        {
            return;
        }

        m_IsAnimatingInverseEffect = true;
        m_InverseEffectTimeCount = 0;
        m_RawImage.material = m_UseInverseEffect;
        m_RawImage.material.SetFloat("_PosX", centerPos.x);
        m_RawImage.material.SetFloat("_PosY", centerPos.y);
    }

    public void StopInverseEffect()
    {
        m_IsAnimatingInverseEffect = false;
        m_RawImage.material.SetFloat("_Radius", 100);
    }
}
