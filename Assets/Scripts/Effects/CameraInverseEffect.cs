#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraInverseEffect : ControllableMonoBehavior
{
    [SerializeField]
    private Material m_Material;

    [SerializeField]
    private Vector2 m_CenterViewPos;

    [SerializeField]
    private float m_Radius;

    private Material m_UseMaterial;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_UseMaterial = Instantiate(m_Material);
    }

    public override void OnFinalize()
    {
        Destroy(m_UseMaterial);
        m_UseMaterial = null;

        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        if (m_UseMaterial != null)
        {
            m_UseMaterial.SetFloat("CenterX", m_CenterViewPos.x);
            m_UseMaterial.SetFloat("CenterY", m_CenterViewPos.y);
            m_UseMaterial.SetFloat("Radius", m_Radius);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_UseMaterial != null)
        {
            Graphics.Blit(source, destination, m_UseMaterial);
        }
    }
}
