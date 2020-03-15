partial class BattleRealManager
{
    private class WaitCutSceneState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_BattleManager.BattleRealStageManager.gameObject.SetActive(false);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.m_BattleManager.BattleRealStageManager.gameObject.SetActive(true);
        }
    }
}
