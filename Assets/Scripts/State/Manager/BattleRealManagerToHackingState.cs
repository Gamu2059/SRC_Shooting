partial class BattleRealManager
{
    private class ToHackingState : StateCycleBase<BattleRealManager>
    {
        public ToHackingState(BattleRealManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            Target.OnTransitionToHacking?.Invoke();
        }
    }
}
