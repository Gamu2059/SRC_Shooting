using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage1
{
    public class WhiteOutController : EventControllableScript
    {
        private GameObject m_WhiteOut;
        private Renderer m_WhiteOutRenderer;
        private Color m_Color;

        private string m_ObjectName;
        private float m_Duration;
        private MaterialPropertyBlock m_PropBlock;
        private int m_ColorPropId;

        public override void OnInitialize()
        {
            base.OnInitialize();

            m_ParamSet.ApplyFloatParam("Duration", ref m_Duration);
            m_ParamSet.ApplyStringParam("Object Name", ref m_ObjectName);
        }

        public override void OnFinalize()
        {
            GameObject.Destroy(m_WhiteOut);
            base.OnFinalize();
        }

        public override void OnStart()
        {
            base.OnStart();

            m_PropBlock = new MaterialPropertyBlock();
            m_WhiteOut = GameObject.Find(m_ObjectName);

            if (m_WhiteOut == null)
            {
                DestroyScript();
                return;
            }

            m_ColorPropId = Shader.PropertyToID("_Color");
            m_WhiteOutRenderer = m_WhiteOut.GetComponent<Renderer>();
            m_WhiteOutRenderer.GetPropertyBlock(m_PropBlock);

            m_Color = Color.white;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            m_Color.a -= 1 / m_Duration * Time.deltaTime;
            m_PropBlock.SetColor(m_ColorPropId, m_Color);
            m_WhiteOutRenderer.SetPropertyBlock(m_PropBlock);

            if (m_Color.a <= 0)
            {
                DestroyScript();
            }
        }
    }
}
