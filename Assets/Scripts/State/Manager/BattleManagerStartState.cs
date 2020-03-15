partial class BattleManager
{
    /// <summary>
    /// バトルシーン開始時に一度だけ遷移するステート
    /// </summary>
    private class StartState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_RealManager.RequestChangeState(E_BATTLE_REAL_STATE.START);
            Target.m_HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.START);

            Target.m_VideoPlayer.gameObject.SetActive(false);

            //Target.BattleRealUiManager.PlayStartTelop();

            Target.IsReadyBeforeShow = true;
            Target.RequestChangeState(E_BATTLE_STATE.REAL_MODE);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
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
    }
}
