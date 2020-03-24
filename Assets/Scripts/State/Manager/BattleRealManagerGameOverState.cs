partial class BattleRealManager
{
    private class GameOverState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            AudioManager.Instance.StopAllBgm();
            //AudioManager.Instance.Play(m_ParamSet.GameOverSe);
            //m_GameOverController.PlayGameOver();

            //Target.m_BattleManager.BattleRealUiManager.SetEnableBossUI(false);
            BattleRealPlayerManager.Instance.StopChargeShot();
            BattleRealPlayerManager.Instance.SetPlayerActive(false);
        }
    }
}
