#pragma warning disable 0649

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CustomImageEffect : ControllableMonoBehavior
{
    [SerializeField]
    private Material m_Material;

    [SerializeField]
    private float m_ShiftLevel;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_ShiftLevel = 0;
        m_Material.SetFloat("_ShiftLevel", 0);
    }

    public override void OnUpdate()
    {

        m_Material.SetFloat("_ShiftLevel", m_ShiftLevel);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // イメージエフェクトの適用
        if (m_Material != null)
        {
            Graphics.Blit(source, destination, m_Material);
        }
    }
}