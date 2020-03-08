partial class BattleRealManager
{
    private class WaitTalkState : StateCycleBase<BattleRealManager>
    {
        public WaitTalkState(BattleRealManager target) : base(target) { }
    }
}
