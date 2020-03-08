partial class BattleManager
{
    private class EndState : StateCycleBase<BattleManager>
    {
        public EndState(BattleManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            Target.RealManager.RequestChangeState(E_BATTLE_REAL_STATE.END);
            Target.HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.END);
            Target.ExitGame();
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
