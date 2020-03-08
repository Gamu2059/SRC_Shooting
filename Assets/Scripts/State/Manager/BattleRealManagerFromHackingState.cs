partial class BattleRealManager
{
    private class FromHackingState : StateCycleBase<BattleRealManager>
    {
        public FromHackingState(BattleRealManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            var battleData = DataManager.Instance.BattleData;
            battleData.OnHackingResult(Target.m_BattleManager.HackingManager.IsHackingSuccess);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.OnTransitionToReal?.Invoke();
        }
    }
}
