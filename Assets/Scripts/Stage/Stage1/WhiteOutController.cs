using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteOutController : EventControllableScript
{
    private GameObject m_WhiteOut;
    private Renderer m_WhiteOutRenderer;
    private Material m_WhiteOutMaterial;
    private Color m_Color;

    public override void OnStart()
    {
        base.OnStart();

        m_WhiteOut = GameObject.Find("WhiteOut");
        m_WhiteOutRenderer = m_WhiteOut.GetComponent<Renderer>();
        m_WhiteOutMaterial = m_WhiteOutRenderer.material;
        m_Color = m_WhiteOutMaterial.GetColor("_Color");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_Color.a -= 0.5f * Time.deltaTime;
        m_WhiteOutMaterial.SetColor("_Color", m_Color);

        if (m_Color.a <= 0)
        {
            DestroyScript();
        }
    }
}
