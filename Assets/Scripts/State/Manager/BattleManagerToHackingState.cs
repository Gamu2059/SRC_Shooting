partial class BattleManager
{
    private class ToHackingState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_RealManager.RequestChangeState(E_BATTLE_REAL_STATE.TO_HACKING);
            Target.m_HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.FROM_REAL);

            DataManager.Instance.BattleData.IncreaseHackingTryCount();

            Target.m_HackInController.ChangeToHackingModeAction += OnChangeToHackingMode;
            Target.m_HackInController.OnStart();

            //Target.m_VideoPlayer.clip = Target.m_ParamSet.ToHackingMovie;
            //Target.m_VideoPlayer.Play();
            //Target.m_VideoPlayer.gameObject.SetActive(true);

            AudioManager.Instance.Play(E_COMMON_SOUND.HACK_START);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //if (Target.m_VideoPlayer.isPlaying)
            //{
            //    //var movieTime = m_ParamSet.ToHackingMovie.length;
            //    //var normalizedTime = (float)(m_VideoPlayer.time / movieTime);

            //    //var fadeOutVideoValue = m_ParamSet.FadeOutVideoParam.Evaluate(normalizedTime);
            //    //var fadeInVideoValue = m_ParamSet.FadeInVideoParam.Evaluate(normalizedTime);

            //    //m_BattleRealUiManager.SetAlpha(fadeOutVideoValue);
            //    //m_BattleHackingUiManager.SetAlpha(fadeInVideoValue);
            //}
            //else
            //{
            //    Target.RequestChangeState(E_BATTLE_STATE.HACKING_MODE);
            //}

            Target.m_RealManager.OnUpdate();
            Target.m_HackingManager.OnUpdate();
            Target.m_HackInController.OnStart();
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
            Target.m_HackInController.OnEnd();
            Target.m_HackInController.ChangeToHackingModeAction -= OnChangeToHackingMode;
            //Target.m_VideoPlayer.gameObject.SetActive(false);
            //Target.m_VideoPlayer.Stop();
        }

        private void OnChangeToHackingMode()
        {
            Target.RequestChangeState(E_BATTLE_STATE.HACKING_MODE);
        }
    }
}
