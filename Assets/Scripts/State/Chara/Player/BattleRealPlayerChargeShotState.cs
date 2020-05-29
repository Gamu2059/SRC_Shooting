using UnityEngine;

partial class BattleRealPlayerController
{
    private class ChargeShotState : StateCycle
    {
        private bool m_IsShotted;

        public override void OnStart()
        {
            base.OnStart();

            var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 0.3f);
            timer.SetTimeoutCallBack(() =>
            {
                timer = null;
                Target.ChargeShot();
                m_IsShotted = true;
            });
            BattleRealTimerManager.Instance.RegistTimer(timer);
            BattleRealEffectManager.Instance.PauseAllEffect();

            var player = BattleRealPlayerManager.Instance.Player;
            var centerPos = BattleRealStageManager.Instance.CalcViewportPosFromWorldPosition(player.transform, false);

            // StageManagerは原点が中央にあるため、原点をずらす
            centerPos += Vector2.one * 0.5f;
            BattleRealUiManager.Instance.FrontViewEffect.PlayEffect(centerPos);

            Target.SetInvinsible(0.3f, false);
            m_IsShotted = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var input = RewiredInputManager.Instance;
            var moveDir = input.AxisDir;
            if (moveDir.x != 0 || moveDir.y != 0)
            {
                float speed = 0;
                if (input.Slowly == E_REWIRED_INPUT_STATE.STAY)
                {
                    speed = Target.m_ParamSet.PlayerSlowMoveSpeed;
                }
                else
                {
                    speed = Target.m_ParamSet.PlayerBaseMoveSpeed;
                }

                var move = moveDir.ToVector3XZ() * speed * Time.deltaTime;
                Target.transform.Translate(move, Space.World);
            }

            if (Target.IsRestrictPosition)
            {
                // 移動直後に位置制限を掛ける
                Target.RestrictPosition();
            }

            switch (input.Shot)
            {
                case E_REWIRED_INPUT_STATE.DOWN:
                case E_REWIRED_INPUT_STATE.STAY:
                    Target.StartShotBullet();
                    break;
                case E_REWIRED_INPUT_STATE.UP:
                case E_REWIRED_INPUT_STATE.NONE:
                    Target.StopShotBullet();
                    break;
            }
            //if (input.Shot == E_REWIRED_INPUT_STATE.STAY)
            //{
            //    Target.ShotBullet();
            //}

            Target.m_ShotDelay += Time.deltaTime;

            if (m_IsShotted && !Target.IsUsingChargeShot())
            {
                Target.RequestChangeDefaultGameState();
            }
        }
    }
}
