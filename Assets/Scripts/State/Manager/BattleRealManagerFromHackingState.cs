partial class BattleRealManager
{
    private class FromHackingState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleRealStageManager.Instance.gameObject.SetActive(true);
            var battleData = DataManager.Instance.BattleData;
            battleData.OnHackingResult(HackingDataHolder.IsHackingSuccess);
            BattleRealUiManager.Instance.PlayToReal();
        }
    }
}
