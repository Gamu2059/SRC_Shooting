partial class BattleRealPlayerController
{
    private class NonGameState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            CheckInterruptEnd();

            // Gameステートでないので通常弾を撃つのを止める
            Target.StopShotBullet();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CheckInterruptEnd();
        }

        private void CheckInterruptEnd()
        {
            if (Target.m_DefaultGameState != E_BATTLE_REAL_PLAYER_STATE.NON_GAME)
            {
                Target.RequestChangeDefaultGameState();
            }
        }
    }
}
