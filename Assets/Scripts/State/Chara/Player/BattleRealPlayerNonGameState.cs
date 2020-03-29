partial class BattleRealPlayerController
{
    private class NonGameState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            CheckInterruptEnd();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CheckInterruptEnd();
        }

        private void CheckInterruptEnd()
        {
            if (Target.m_DefaultGameState != E_STATE.NON_GAME)
            {
                Target.RequestChangeDefaultGameState();
            }
        }
    }
}
