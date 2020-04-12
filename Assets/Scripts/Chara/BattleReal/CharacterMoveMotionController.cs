using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクタの移動モーションを制御する。
/// </summary>
public class CharacterMoveMotionController : ControllableMonoBehavior, IAutoControlOnCharaController
{
    [SerializeField]
    private Animator m_TargetAnimator;

    [SerializeField]
    private string m_VerticalKey = "VSpeed";

    [SerializeField]
    private string m_HorizontalKey = "HSpeed";

    [SerializeField]
    private float m_Leap = 0.1f;

    private bool m_IsEnableController;
    public bool IsEnableController { get { return m_IsEnableController; } set { m_IsEnableController = value; } }

    private Vector3 m_PrePosition;
    private float m_TargetV;
    private float m_TargetH;
    private float m_NowV;
    private float m_NowH;

    public override void OnStart()
    {
        base.OnStart();
        m_PrePosition = transform.position;
        m_TargetV = m_TargetH = 0;
        m_NowV = m_NowH = 0;
        IsEnableController = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_TargetAnimator == null)
        {
            return;
        }

        var delta = transform.position - m_PrePosition;
        var d = (delta.ToVector2XZ()).normalized;
        var rad = transform.eulerAngles.y.DegToRad();
        var c = Mathf.Cos(rad);
        var s = Mathf.Sin(rad);
        m_TargetH = c * d.x - s * d.y;
        m_TargetV = s * d.x + c * d.y;

        m_NowH = Mathf.Lerp(m_NowH, m_TargetH, m_Leap);
        m_NowV = Mathf.Lerp(m_NowV, m_TargetV, m_Leap);

        m_TargetAnimator.SetFloat(m_HorizontalKey, m_NowH);
        m_TargetAnimator.SetFloat(m_VerticalKey, m_NowV);

        m_PrePosition = transform.position;
    }
}
