using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CommonUiButtonEffector : MonoBehaviour
{
    #region Define

    // TMP のmaterialForRenderingのシェーダが Distance Field になっていることが前提です

    private const string VERTICAL_EFFECT_TIME = "_VerticalEffectTime";
    private const string GLOW_COLOR = "_OutlineColor";
    private const string GLOW_OFFSET = "_OutlineWidth";
    private const string GLOW_OUTER = "_FaceDilate";

    #endregion

    #region Field Inspector

    [SerializeField]
    private TextMeshProUGUI m_Text;

    [Header("Show Effect")]

    [SerializeField]
    private float m_ShowEffectDuration;

    [Header("Submit Effect")]
    
    [SerializeField]
    private float m_SubmitEffectDuration;

    [SerializeField]
    private Color m_OutlineColor;

    [SerializeField]
    private float m_OutlineWidth = 1;

    [SerializeField]
    private float m_FaceDilate = 1;

    #endregion

    #region Field

    private Material m_UseMaterial;

    private Color m_SeqColor;
    private float m_SeqWidth;
    private float m_SeqDilate;
    private float m_SeqVerticalEffectTime;

    private Tween m_SubmitTween;
    private Tween m_ShowTween;

    #endregion

    private void Awake()
    {
        m_UseMaterial = Instantiate(m_Text.fontSharedMaterial);
        m_Text.material = m_UseMaterial;

        m_SeqColor = m_Text.color;
        m_SeqWidth = 0;
        m_SeqDilate = 0;
        m_SeqVerticalEffectTime = 1;

        ApplyEffect();
    }

    private void OnDestroy()
    {
        if (m_UseMaterial != null)
        {
            Destroy(m_UseMaterial);
            m_UseMaterial = null;
        }
    }

    /// <summary>
    /// Submitを受け取った時に呼び出す
    /// </summary>
    public void OnSubmit()
    {
        m_SubmitTween?.Complete();

        m_SeqColor = m_OutlineColor;
        m_SeqWidth = 0f;
        m_SeqDilate = 0f;
        ApplyEffect();

        var targetColor = new Color(m_OutlineColor.r, m_OutlineColor.g, m_OutlineColor.b, 0f);

        m_SubmitTween = DOTween.Sequence()
            .Append(DOTween.To(() => m_SeqColor, x => m_SeqColor = x, targetColor, m_SubmitEffectDuration))
            .Join(DOTween.To(() => m_SeqWidth, x => m_SeqWidth = x, m_OutlineWidth, m_SubmitEffectDuration))
            .Join(DOTween.To(() => m_SeqDilate, x => m_SeqDilate = x, m_FaceDilate, m_SubmitEffectDuration))
            .OnUpdate(ApplyEffect);
    }

    /// <summary>
    /// Viewの表示開始時に呼び出したりする
    /// </summary>
    public void OnShowStart()
    {
        m_ShowTween?.Complete();

        m_SeqVerticalEffectTime = 0;

        m_ShowTween = DOTween.Sequence()
            .Append(DOTween.To(() => m_SeqVerticalEffectTime, x => m_SeqVerticalEffectTime = x, 1, m_ShowEffectDuration))
            .OnUpdate(ApplyEffect);
    }

    private void ApplyEffect()
    {
        ApplyEffect(m_SeqColor, m_SeqWidth, m_SeqDilate, m_SeqVerticalEffectTime);
    }

    private void ApplyEffect(Color color, float offset, float outer, float vert)
    {
        m_UseMaterial.SetColor(GLOW_COLOR, color);
        m_UseMaterial.SetFloat(GLOW_OFFSET, offset);
        m_UseMaterial.SetFloat(GLOW_OUTER, outer);
        m_UseMaterial.SetFloat(VERTICAL_EFFECT_TIME, vert);
        m_Text.fontMaterial = m_UseMaterial;
        m_Text.UpdateFontAsset();
    }
}
