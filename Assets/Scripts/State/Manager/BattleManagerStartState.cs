partial class BattleManager
{
    private class StartState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.RealManager.OnStart();
            Target.HackingManager.OnStart();

            Target.BattleRealUiManager.OnStart();
            Target.BattleHackingUiManager.OnStart();

            Target.m_BattleRealStageManager.gameObject.SetActive(true);
            Target.m_BattleHackingStageManager.gameObject.SetActive(false);
            Target.m_VideoPlayer.gameObject.SetActive(false);

            Target.RequestChangeState(E_BATTLE_STATE.REAL_MODE);

            //Target.BattleRealUiManager.PlayStartTelop();

            Target.IsReadyBeforeShow = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
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
    }
}
