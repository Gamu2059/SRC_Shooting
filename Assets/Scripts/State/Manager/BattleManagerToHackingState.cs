partial class BattleManager
{
    private class ToHackingState : StateCycleBase<BattleManager>
    {
        public ToHackingState(BattleManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            Target.RealManager.RequestChangeState(E_BATTLE_REAL_STATE.TO_HACKING);
            Target.HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.TRANSITION_TO_HACKING);

            DataManager.Instance.BattleData.IncreaseHackingTryCount();

            Target.BattleHackingStageManager.gameObject.SetActive(true);

            Target.m_VideoPlayer.clip = Target.m_ParamSet.ToHackingMovie;
            Target.m_VideoPlayer.Play();
            Target.m_VideoPlayer.gameObject.SetActive(true);

            AudioManager.Instance.Play(Target.m_ParamSet.ToHackingSe);

            Target.BattleRealUiManager.PlayToHacking();
            Target.BattleHackingUiManager.PlayToHacking();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Target.m_VideoPlayer.isPlaying)
            {
                //var movieTime = m_ParamSet.ToHackingMovie.length;
                //var normalizedTime = (float)(m_VideoPlayer.time / movieTime);

                //var fadeOutVideoValue = m_ParamSet.FadeOutVideoParam.Evaluate(normalizedTime);
                //var fadeInVideoValue = m_ParamSet.FadeInVideoParam.Evaluate(normalizedTime);

                //m_BattleRealUiManager.SetAlpha(fadeOutVideoValue);
                //m_BattleHackingUiManager.SetAlpha(fadeInVideoValue);
            }
            else
            {
                Target.RequestChangeState(E_BATTLE_STATE.HACKING_MODE);
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
            Target.RealManager.RequestChangeState(E_BATTLE_REAL_STATE.STAY_HACKING);
            Target.HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.GAME);

            Target.m_BattleRealStageManager.gameObject.SetActive(false);
            Target.m_VideoPlayer.gameObject.SetActive(false);
            Target.m_VideoPlayer.Stop();
        }
    }
}
