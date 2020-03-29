partial class BattleRealManager
{
    private class WaitCutSceneState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleRealStageManager.Instance.gameObject.SetActive(false);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            BattleRealStageManager.Instance.gameObject.SetActive(true);
        }
    }
}
