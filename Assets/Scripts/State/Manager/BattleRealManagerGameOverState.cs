partial class BattleRealManager
{
    private class GameOverState : StateCycleBase<BattleRealManager>
    {
        public GameOverState(BattleRealManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            Target.m_BattleManager.BattleRealUiManager.SetEnableBossUI(false);
            Target.PlayerManager.StopChargeShot();
            Target.PlayerManager.SetPlayerActive(false);
        }
    }
}
