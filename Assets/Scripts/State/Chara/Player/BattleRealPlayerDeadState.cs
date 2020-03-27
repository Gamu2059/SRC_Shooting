partial class BattleRealPlayerController
{
    private class DeadState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            Target.gameObject.SetActive(false);
            Target.IsDead = true;

            Target.StopChargeShot();
            // 死亡SEは色々な処理の後にしておかないと、プレイヤーSEの停止に巻き込まれる可能性がある
            AudioManager.Instance.Play(BattleRealPlayerManager.Instance.ParamSet.DeadSe);
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
