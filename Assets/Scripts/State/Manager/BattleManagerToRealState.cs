partial class BattleManager
{
    private class ToRealState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_RealManager.RequestChangeState(E_BATTLE_REAL_STATE.FROM_HACKING);
            Target.m_HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.TO_REAL);

            Target.m_HackOutController.ChangeToRealModeAction += OnChangeToRealMode;
            Target.m_HackOutController.OnStart();

            AudioManager.Instance.Play(E_COMMON_SOUND.HACK_END);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.m_RealManager.OnUpdate();
            Target.m_HackingManager.OnUpdate();
            Target.m_HackOutController.OnUpdate();
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
            Target.m_HackOutController.OnEnd();
            Target.m_HackOutController.ChangeToRealModeAction -= OnChangeToRealMode;
        }

        private void OnChangeToRealMode()
        {
            Target.RequestChangeState(E_BATTLE_STATE.REAL_MODE);
        }
    }
}
