partial class BattleRealManager
{
    private class FromHackingState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            var battleData = DataManager.Instance.BattleData;
            battleData.OnHackingResult(HackingDataHolder.IsHackingSuccess);
        }
    }
}
