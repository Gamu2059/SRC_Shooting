partial class BattleHackingManager
{
    private class StartState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleHackingStageManager.Instance.gameObject.SetActive(false);
            BattleHackingTimerManager.Instance.OnStart();
            BattleHackingPlayerManager.Instance.OnStart();
            BattleHackingEnemyManager.Instance.OnStart();
            BattleHackingBulletManager.Instance.OnStart();
            BattleHackingEffectManager.Instance.OnStart();
            BattleHackingCollisionManager.Instance.OnStart();
            BattleHackingCameraManager.Instance.OnStart();
            BattleHackingUiManager.Instance.OnStart();

            Target.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);
        }
    }
}
