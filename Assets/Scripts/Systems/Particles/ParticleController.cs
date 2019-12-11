#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : ControllableMonoBehavior
{
    [SerializeField]
    private float m_Duration;

    private float m_Time;

    private void Start()
    {
        m_Time = 0;
    }

    private void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= m_Duration)
        {
            Destroy(gameObject);
        }
    }
}
