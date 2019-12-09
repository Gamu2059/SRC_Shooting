using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOrbitController : ControllableMonoBehavior
{
    [SerializeField]
    private ParticleSystem m_Particle;

    [SerializeField]
    private AnimationCurve m_LerpCurve;

    private ParticleSystem.Particle[] m_Particles;

    private void Update()
    {
        OnUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_Particles = new ParticleSystem.Particle[m_Particle.main.maxParticles];
        var activeNum = m_Particle.GetParticles(m_Particles);
        var lifeTime = m_Particle.main.startLifetime.constant;
        for (int i=0;i< activeNum; i++)
        {
            var currentTime = lifeTime - m_Particles[i].remainingLifetime;
            var normalizedLifeTime = currentTime / lifeTime;

            var localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);

            // 粒子の位置から正面方向に下した位置
            var dotForward = Vector3.Dot(localForward, m_Particles[i].position) * localForward;
            var velocity = m_Particles[i].velocity;
            var delta = (dotForward - m_Particles[i].position).normalized;
            var nextVelocity = Vector3.LerpUnclamped(velocity.normalized, delta, m_LerpCurve.Evaluate(normalizedLifeTime));
            m_Particles[i].velocity = nextVelocity.normalized * velocity.magnitude;
        }

        m_Particle.SetParticles(m_Particles, activeNum);
    }
}
