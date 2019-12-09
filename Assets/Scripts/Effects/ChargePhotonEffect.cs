#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チャージの粒子が中央に集結するエフェクトを制御する
/// </summary>
public class ChargePhotonEffect : ControllableMonoBehavior
{
    [SerializeField]
    private ParticleSystem m_ParticleSystem;

    [SerializeField, Tooltip("粒子の速度の大きさのカーブ")]
    private AnimationCurve m_VelocityCurve;

    [SerializeField, Tooltip("trueにすると、Updateを単独で呼び出すようになる")]
    private bool m_ControllBySelf;

    private void Update()
    {
        if (m_ControllBySelf)
        {
            OnUpdate();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
        var activeNum = m_ParticleSystem.GetParticles(particles);
        var lifetime = m_ParticleSystem.main.startLifetime.constant;
        for (int i=0;i<activeNum;i++)
        {
            var currentTime = lifetime - particles[i].remainingLifetime;
            var normalizedLifeTime = currentTime / lifetime;
            var mag = m_VelocityCurve.Evaluate(normalizedLifeTime);
            particles[i].velocity = particles[i].velocity.normalized * mag;
        }

        m_ParticleSystem.SetParticles(particles, activeNum);
    }
}
