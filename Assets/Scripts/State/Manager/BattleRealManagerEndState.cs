partial class BattleRealManager
{
    private class EndState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            AudioManager.Instance.StopAllBgm();
            AudioManager.Instance.StopAllSe();
        }
    }
}
