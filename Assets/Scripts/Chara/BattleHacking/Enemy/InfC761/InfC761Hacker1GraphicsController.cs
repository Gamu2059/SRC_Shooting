using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InfC761のハッキングモードの見た目の制御コンポーネント。
/// </summary>
public class InfC761Hacker1GraphicsController : ControllableMonoBehavior
{
    [SerializeField]
    private Transform m_EyeHolder = default;

    [SerializeField]
    private Transform m_EyeMain = default;
    public Transform Eye => m_EyeMain;

    [SerializeField]
    private SpriteRenderer m_BodyRenderer = default;

    [SerializeField]
    private SpriteRenderer m_BodyLightRenderer = default;

    [SerializeField]
    private SpriteRenderer m_EyeLightRenderer = default;

    [SerializeField]
    private float m_EyeMoveRadius = default;

    private bool m_IsMoveEye = default;

    private float m_ColorReturnTime = default;

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
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (m_ColorReturnTime > 0)
        {
            m_ColorReturnTime -= Time.fixedDeltaTime;
            if (m_ColorReturnTime <= 0)
            {
                if (m_BodyRenderer != null)
                {
                    m_BodyRenderer.color = Color.white;
                }
            }
        }
    }

    public void SetEnableEyeMove(bool isEnable)
    {
        m_IsMoveEye = isEnable;
    }

    public void ChangeLightColor(Color color)
    {
        if (m_BodyLightRenderer != null)
        {
            m_BodyLightRenderer.color = color;
        }

        if (m_EyeLightRenderer != null)
        {
            m_EyeLightRenderer.color = color;
        }
    }

    public void OnSufferBullet()
    {
        if (m_BodyRenderer != null)
        {
            m_BodyRenderer.color = Color.red;
            m_ColorReturnTime = 0.3f;
        }
    }
}
