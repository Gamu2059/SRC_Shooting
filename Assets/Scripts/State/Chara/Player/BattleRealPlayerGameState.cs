﻿using UnityEngine;

partial class BattleRealPlayerController
{
    private class GameState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            if (Target.m_DefaultGameState != E_BATTLE_REAL_PLAYER_STATE.GAME)
            {
                Target.RequestChangeDefaultGameState();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Target.m_DefaultGameState != E_BATTLE_REAL_PLAYER_STATE.GAME)
            {
                Target.RequestChangeDefaultGameState();
                return;
            }

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

            if (input.ChangeWeapon == E_REWIRED_INPUT_STATE.DOWN)
            {
                Target.IsLaserType = !Target.IsLaserType;
                Target.ChangeWeapon();
                Target.ChangeWeaponTypeAction?.Invoke(Target.IsLaserType);
            }

            if (input.ChargeShot == E_REWIRED_INPUT_STATE.DOWN)
            {
                if (DataManager.Instance.BattleData.EnergyStock.Value > 0)
                {
                    Target.RequestChangeState(E_BATTLE_REAL_PLAYER_STATE.CHARGE);
                }
            }
        }

        public override void OnEnd()
        {
            base.OnEnd();
        }
    }
}
