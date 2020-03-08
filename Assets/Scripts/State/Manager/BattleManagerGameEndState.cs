partial class BattleManager
{
    private class GameEndState : StateCycleBase<BattleManager>
    {
        public GameEndState(BattleManager target) : base(target) { }
    }
}
