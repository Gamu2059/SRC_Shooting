partial class BattleRealManager
{
    private class GameOverState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            
            DataManager.Instance.OnChapterEnd(false);

            AudioManager.Instance.StopAllBgm();
            AudioManager.Instance.StopAllSe();

            BattleRealPlayerManager.Instance.StopChargeShot();
            BattleRealUiManager.Instance.PlayGameOver();
            BattleRealUiManager.Instance.DisableAllBossUI();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            BattleRealUiManager.Instance.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            BattleRealUiManager.Instance.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            BattleRealUiManager.Instance.OnFixedUpdate();
        }
    }
}
