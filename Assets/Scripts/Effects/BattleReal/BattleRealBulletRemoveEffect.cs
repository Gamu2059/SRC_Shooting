using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 弾消し時のスコア表記エフェクトクラス。
/// </summary>
public class BattleRealBulletRemoveEffect : BattleCommonEffectController
{
    [SerializeField]
    private TextMeshPro m_Text;

    [SerializeField]
    private AnimationCurve m_ZPosAnim;

    [SerializeField]
    private AnimationCurve m_AlphaAnim;

    public override void OnInitialize()
    {
        base.OnInitialize();
        ApplyAnim();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        ApplyAnim();
    }

    public void SetPoint(int value)
    {
        m_Text.text = value.ToString();
    }

    private void ApplyAnim()
    {
        if (Duration <= 0)
        {
            return;
        }

        var rate = NowLifeTime / Duration;
        var z = m_ZPosAnim.Evaluate(rate);
        var a = m_AlphaAnim.Evaluate(rate);

        var pos = m_Text.transform.localPosition;
        pos.z = z;
        m_Text.transform.localPosition = pos;

        m_Text.alpha = a;
    }
}
