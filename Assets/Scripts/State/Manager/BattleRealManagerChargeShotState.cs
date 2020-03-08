using UnityEngine;

partial class BattleRealManager
{
    private class ChargeShotState : StateCycleBase<BattleRealManager>
    {
        public ChargeShotState(BattleRealManager target) : base(target) { }

        public override void OnStart()
        {
            base.OnStart();
            var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 0.3f);
            timer.SetTimeoutCallBack(() =>
            {
                timer.DestroyTimer();
                Target.RequestChangeState(E_BATTLE_REAL_STATE.GAME);
            });
            TimerManager.Instance.RegistTimer(timer);

            Target.EffectManager.PauseAllEffect();

            var player = Target.PlayerManager.Player;
            var centerPos = Target.m_BattleManager.BattleRealStageManager.CalcViewportPosFromWorldPosition(player.transform, false);

            // StageManagerは原点が中央にあるため、原点をずらす
            centerPos += Vector2.one * 0.5f;

            Target.m_BattleManager.BattleRealUiManager.FrontViewEffect.PlayEffect(centerPos);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.m_BattleManager.BattleRealUiManager.FrontViewEffect.StopEffect();
            Target.EffectManager.ResumeAllEffect();
            Target.PlayerManager.ChargeShot();
        }
    }
}
