partial class BattleRealManager
{
    private class EndState : StateCycleBase<BattleRealManager>
    {
        public EndState(BattleRealManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            AudioManager.Instance.StopAllBgm();
            AudioManager.Instance.StopAllSe();
        }
    }
}
