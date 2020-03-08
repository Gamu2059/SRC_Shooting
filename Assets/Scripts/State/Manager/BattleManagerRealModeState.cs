partial class BattleManager
{
    private partial class RealModeState : StateCycleBase<BattleManager>
    {
        public RealModeState(BattleManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            AudioManager.Instance.OperateAisac(E_AISAC_TYPE.AISAC_HACK, E_CUE_SHEET.BGM, 0);
            Target.BattleRealUiManager.SetAlpha(1);
            Target.BattleHackingUiManager.SetAlpha(0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Target.m_IsStartHackingMode)
            {
                Target.m_IsStartHackingMode = false;
                Target.RequestChangeState(E_BATTLE_STATE.TRANSITION_TO_HACKING);
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
    }
}
