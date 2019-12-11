#pragma warning disable 0649

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CustomImageEffect : ControllableMonoBehavior
{
    [SerializeField]
    private Material m_Material;

    [SerializeField]
    private float m_ShiftLevel;

    private Material m_UseMaterial;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_UseMaterial = Instantiate(m_Material);

        m_ShiftLevel = 0;
        m_UseMaterial.SetFloat("_ShiftLevel", 0);
    }

    public override void OnFinalize()
    {
        Destroy(m_UseMaterial);
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        m_UseMaterial.SetFloat("_ShiftLevel", m_ShiftLevel);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_UseMaterial != null)
        {
            Graphics.Blit(source, destination, m_UseMaterial);
        }
    }
}