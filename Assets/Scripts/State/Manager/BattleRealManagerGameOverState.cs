partial class BattleRealManager
{
    private class GameOverState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_BattleManager.BattleRealUiManager.SetEnableBossUI(false);
            Target.PlayerManager.StopChargeShot();
            Target.PlayerManager.SetPlayerActive(false);
        }
    }
}
