partial class BattleManager
{
    private class ToRealState : StateCycleBase<BattleManager>
    {
        public ToRealState(BattleManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            Target.RealManager.RequestChangeState(E_BATTLE_REAL_STATE.FROM_HACKING);
            Target.HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.TRANSITION_TO_REAL);

            Target.m_BattleRealStageManager.gameObject.SetActive(true);

            Target.m_VideoPlayer.clip = Target.m_ParamSet.ToRealMovie;
            Target.m_VideoPlayer.Play();
            Target.m_VideoPlayer.gameObject.SetActive(true);

            AudioManager.Instance.Play(Target.m_ParamSet.ToRealSe);

            Target.BattleRealUiManager.PlayToReal();
            Target.BattleHackingUiManager.PlayToReal();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!Target.m_VideoPlayer.isPlaying)
            {
                Target.RequestChangeState(E_BATTLE_STATE.REAL_MODE);
            }

            Target.RealManager.OnUpdate();
            Target.HackingManager.OnUpdate();

            Target.BattleRealUiManager.OnUpdate();
            Target.BattleHackingUiManager.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            Target.RealManager.OnLateUpdate();
            Target.HackingManager.OnLateUpdate();

            Target.BattleRealUiManager.OnLateUpdate();
            Target.BattleHackingUiManager.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Target.RealManager.OnFixedUpdate();
            Target.HackingManager.OnFixedUpdate();

            Target.BattleRealUiManager.OnFixedUpdate();
            Target.BattleHackingUiManager.OnFixedUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);
            Target.RealManager.RequestChangeState(E_BATTLE_REAL_STATE.GAME);

            Target.m_BattleHackingStageManager.gameObject.SetActive(false);
            Target.m_VideoPlayer.gameObject.SetActive(false);
            Target.m_VideoPlayer.Stop();
        }
    }
}
