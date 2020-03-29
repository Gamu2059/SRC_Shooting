partial class BattleManager
{
    private class ToRealState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_RealManager.RequestChangeState(E_BATTLE_REAL_STATE.FROM_HACKING);
            Target.m_HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.TO_REAL);

            Target.m_VideoPlayer.clip = Target.m_ParamSet.ToRealMovie;
            Target.m_VideoPlayer.Play();
            Target.m_VideoPlayer.gameObject.SetActive(true);

            AudioManager.Instance.Play(Target.m_ParamSet.ToRealSe);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!Target.m_VideoPlayer.isPlaying)
            {
                Target.RequestChangeState(E_BATTLE_STATE.REAL_MODE);
            }
            Target.m_RealManager.OnUpdate();
            Target.m_HackingManager.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            Target.m_RealManager.OnLateUpdate();
            Target.m_HackingManager.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Target.m_RealManager.OnFixedUpdate();
            Target.m_HackingManager.OnFixedUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.m_VideoPlayer.gameObject.SetActive(false);
            Target.m_VideoPlayer.Stop();
        }
    }
}
