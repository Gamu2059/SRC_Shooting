using UnityEngine;

partial class BattleRealManager
{
    private class ChargeShotState : StateCycle
    {
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

            BattleRealEffectManager.Instance.PauseAllEffect();

            var player = BattleRealPlayerManager.Instance.Player;
            var centerPos = BattleRealStageManager.Instance.CalcViewportPosFromWorldPosition(player.transform, false);

            // StageManagerは原点が中央にあるため、原点をずらす
            centerPos += Vector2.one * 0.5f;

            BattleRealUiManager.Instance.FrontViewEffect.PlayEffect(centerPos);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            BattleRealUiManager.Instance.FrontViewEffect.StopEffect();
            BattleRealEffectManager.Instance.ResumeAllEffect();
            BattleRealPlayerManager.Instance.ChargeShot();
        }
    }
}
