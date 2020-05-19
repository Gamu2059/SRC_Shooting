partial class BattleRealManager
{
    /// <summary>
    /// バトルシーン開始時に一度だけ遷移するステート
    /// </summary>
    private class StartState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            BattleRealStageManager.Instance.gameObject.SetActive(true);

            BattleRealTimerManager.Instance.OnStart();
            BattleRealEventManager.Instance.OnStart();
            BattleRealPlayerManager.Instance.OnStart();
            BattleRealEnemyGroupManager.Instance.OnStart();
            BattleRealEnemyManager.Instance.OnStart();
            BattleRealBulletGeneratorManager.Instance.OnStart();
            BattleRealBulletManager.Instance.OnStart();
            BattleRealItemManager.Instance.OnStart();
            BattleRealEffectManager.Instance.OnStart();
            BattleRealSequenceObjectManager.Instance.OnStart();
            BattleRealCollisionManager.Instance.OnStart();
            BattleRealCameraManager.Instance.OnStart();
            BattleRealUiManager.Instance.OnStart();
        }
    }
}
