#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスのチャージエフェクトをカメラに適用するためのクラス。
/// </summary>
public class CameraBossChargeEffect : ControllableMonoBehavior
{
    [SerializeField]
    private Material m_Material;

    private Material m_UseMaterial;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_UseMaterial = Instantiate(m_Material);
    }

    protected override void OnDestroyed()
    {
        Destroy(m_UseMaterial);
        m_UseMaterial = null;
        base.OnDestroyed();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_UseMaterial != null)
        {
            Graphics.Blit(source, destination, m_UseMaterial);
        }
    }
}
