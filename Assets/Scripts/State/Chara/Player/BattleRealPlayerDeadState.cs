partial class BattleRealPlayerController
{
    private class DeadState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            Target.StopChargeShot();
            Target.gameObject.SetActive(false);
            Target.IsDead = true;

            var deadEffect = BattleRealPlayerManager.Instance.ParamSet.DeadEffectParam;
            BattleRealEffectManager.Instance.CreateEffect(deadEffect, Target.transform);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();

            Target.IsDead = false;
            Target.gameObject.SetActive(true);
        }
    }
}
