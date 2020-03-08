partial class BattleRealManager
{
    private class ToHackingState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.OnTransitionToHacking?.Invoke();
        }
    }
}
