using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクタの移動モーションを制御する。
/// </summary>
public class CharacterMoveMotionController : ControllableMonoBehavior, IAutoControlOnCharaController
{
    private const string V_SPEED = "VSpeed";
    private const string H_SPEED = "HSpeed";

    [SerializeField]
    private Animator m_TargetAnimator;

    [SerializeField]
    private float m_Leap;

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
        var rad = transform.eulerAngles.y;
        var c = Mathf.Cos(rad);
        var s = Mathf.Sin(rad);
        m_TargetH = c * d.x - s * d.y;
        m_TargetV = s * d.x + c * d.y;

        m_NowH = Mathf.Lerp(m_NowH, m_TargetH, m_Leap * Time.deltaTime);
        m_NowV = Mathf.Lerp(m_NowV, m_TargetV, m_Leap * Time.deltaTime);

        m_TargetAnimator.SetFloat(H_SPEED, m_NowH);
        m_TargetAnimator.SetFloat(V_SPEED, m_NowV);

        m_PrePosition = transform.position;
    }
}
