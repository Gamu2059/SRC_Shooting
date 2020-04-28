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

            AudioManager.Instance.Play(E_COMMON_SOUND.HACK_START);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.m_RealManager.OnUpdate();
            Target.m_HackingManager.OnUpdate();
            Target.m_HackInController.OnUpdate();
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
        }

        private void OnChangeToHackingMode()
        {
            Target.RequestChangeState(E_BATTLE_STATE.HACKING_MODE);
        }
    }
}
