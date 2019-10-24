using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InfC761のハッキングモードの見た目の制御コンポーネント。
/// </summary>
public class InfC761Hacker1GraphicsController : ControllableMonoBehavior
{
    [SerializeField]
    private Transform m_Eye;
    public Transform Eye => m_Eye;

    [SerializeField]
    private SpriteRenderer m_BodyRenderer;

    [SerializeField]
    private SpriteRenderer m_BodyLightRenderer;

    [SerializeField]
    private SpriteRenderer m_EyeLightRenderer;

    [SerializeField]
    private float m_EyeMoveRadius;

    private bool m_IsMoveEye;

    private float m_ColorReturnTime;

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsMoveEye)
        {
            var player = BattleHackingPlayerManager.Instance.Player;
            if (player != null)
            {
                var dir = player.transform.position - m_Eye.position;
                dir = dir.ToVector2XZ().normalized.ToVector3XZ();
                m_Eye.localPosition = dir * m_EyeMoveRadius;
            }
        }
        else
        {
            m_Eye.position = Vector3.zero;
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
