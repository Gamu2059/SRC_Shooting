#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ハッキングモードのグリッドホールエフェクトを制御する。
/// </summary>
public class HackingGridHoleEffect : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private RawImage m_RawImage;

    [SerializeField]
    private AnimationCurve m_GridHoleEffectRadiusCurve;

    [SerializeField]
    private Material m_GridHoleEffect;

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

        m_UseEffect = Instantiate(m_GridHoleEffect);
        m_Duration = m_GridHoleEffectRadiusCurve.Duration();
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
                var radius = m_GridHoleEffectRadiusCurve.Evaluate(m_TimeCount);
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
        m_RawImage.material.SetFloat("_Radius", 0);
    }
}
