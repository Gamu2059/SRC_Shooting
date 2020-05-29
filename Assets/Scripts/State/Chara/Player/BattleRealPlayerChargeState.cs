using UnityEngine;

partial class BattleRealPlayerController
{
    private class ChargeState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.ChargeStart();
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

            if (input.ChargeShot == E_REWIRED_INPUT_STATE.UP)
            {
                Target.RequestChangeState(E_BATTLE_REAL_PLAYER_STATE.CHARGE_SHOT);
            }

            Target.m_ShotDelay += Time.deltaTime;
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.ChargeEnd();
        }
    }
}
