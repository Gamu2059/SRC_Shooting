#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InfC761のハッキングモードの見た目の制御コンポーネント。
/// </summary>
public class InfC761HackingGraphicsController : ControllableMonoBehavior
{
    private const string RADIUS = "_Radius";

    [SerializeField]
    private Transform m_EyeHolder = default;

    [SerializeField]
    private Transform m_EyeMain = default;
    public Transform Eye => m_EyeMain;

    [SerializeField]
    private SpriteRenderer m_BodyRenderer = default;

    [SerializeField]
    private SpriteRenderer m_BodyLightRenderer = default;

    //[SerializeField]
    //private SpriteRenderer m_EyeLightRenderer = default;

    [SerializeField]
    private float m_EyeMoveRadius = default;

    [SerializeField]
    private Material m_DefeatEffect;

    [SerializeField]
    private Material m_NormalEffect;

    [SerializeField]
    private AnimationCurve m_DefeatRadiusCurve;

    private Material m_UseDefeatEffect;
    private bool m_IsAnimation;
    private float m_Duration;
    private float m_TimeCount;

    private bool m_IsMoveEye = default;

    //private float m_ColorReturnTime = default;

    /// <summary>
    /// OnInitializeやOnStartは複数回呼ばれてしまうのでOnAwakeにした
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();
        m_UseDefeatEffect = Instantiate(m_DefeatEffect);
        m_IsAnimation = false;
        m_Duration = m_DefeatRadiusCurve.Duration();
    }

    /// <summary>
    /// OnFinalizeは複数回呼ばれてしまうのでOnDestroyedにした
    /// </summary>
    protected override void OnDestroyed()
    {
        Destroy(m_UseDefeatEffect);
        base.OnDestroyed();
    }

    public override void OnStart()
    {
        base.OnStart();
        m_EyeHolder.gameObject.SetActive(true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsMoveEye)
        {
            var player = BattleHackingPlayerManager.Instance.Player;
            if (player != null)
            {
                var dir = player.transform.position - m_EyeMain.position;
                dir = dir.ToVector2XZ().normalized.ToVector3XZ();
                m_EyeHolder.localPosition = dir * m_EyeMoveRadius;
            }
        }
        else
        {
            m_EyeHolder.position = Vector3.zero;
        }

        if (m_IsAnimation)
        {
            if (m_TimeCount <= m_Duration)
            {
                CheckMaterial();

                var radius = m_DefeatRadiusCurve.Evaluate(m_TimeCount);
                m_UseDefeatEffect.SetFloat(RADIUS, radius);
                m_TimeCount += Time.deltaTime;
            }
            else
            {
                m_IsAnimation = false;
            }
        }
    }

    /// <summary>
    /// レンダラのマテリアルをチェックし、強制的にエフェクト用マテリアルに切り替える
    /// </summary>
    private void CheckMaterial()
    {
        if (m_BodyRenderer != null && m_BodyRenderer.sharedMaterial != m_UseDefeatEffect)
        {
            m_BodyRenderer.sharedMaterial = m_UseDefeatEffect;
        }

        if (m_BodyLightRenderer != null && m_BodyLightRenderer.sharedMaterial != m_UseDefeatEffect)
        {
            m_BodyLightRenderer.sharedMaterial = m_UseDefeatEffect;
        }
    }

    public void SetEnableEyeMove(bool isEnable)
    {
        m_IsMoveEye = isEnable;
    }

    public void PlayDestroyAnimation()
    {
        if (m_IsAnimation)
        {
            return;
        }

        m_IsAnimation = true;
        m_TimeCount = 0;
    }

    public void HideEye()
    {
        m_EyeHolder.gameObject.SetActive(false);
    }
}
