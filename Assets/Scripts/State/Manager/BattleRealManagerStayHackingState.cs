partial class BattleRealManager
{
    private class StayHackingState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleRealStageManager.Instance.gameObject.SetActive(false);
            BattleRealUiManager.Instance.SetAlpha(0);
        }
    }
}
