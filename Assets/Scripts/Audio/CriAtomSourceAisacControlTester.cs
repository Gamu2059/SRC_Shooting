using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CriAtomSource))]
public class CriAtomSourceAisacControlTester : MonoBehaviour
{
    [Serializable]
    private struct AisacData
    {
        public string Name;

        [Range(0, 1)]
        public float Value;
    }

    [SerializeField]
    private AisacData[] m_Params;

    private CriAtomSource m_Source;

    private void Awake()
    {
        m_Source = GetComponent<CriAtomSource>();
    }

    private void Update()
    {
        if (m_Source == null)
        {
            return;
        }

        foreach(var param in m_Params)
        {
            m_Source.SetAisacControl(param.Name, param.Value);
        }
    }
}
