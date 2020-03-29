using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleHackingManager
{
    private class GameState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleHackingUiManager.Instance.SetAlpha(1);
            BattleHackingInputManager.Instance.RegistInput();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            // 消滅の更新
            BattleHackingEnemyManager.Instance.GotoPool();
            BattleHackingBulletManager.Instance.GotoPool();
            BattleHackingEffectManager.Instance.GotoPool();
            BattleHackingCollisionManager.Instance.DestroyDrawingColliderMeshes();

            BattleHackingInputManager.Instance.OnUpdate();
            BattleHackingTimerManager.Instance.OnUpdate();
            BattleHackingPlayerManager.Instance.OnUpdate();
            BattleHackingEnemyManager.Instance.OnUpdate();
            BattleHackingBulletManager.Instance.OnUpdate();
            BattleHackingEffectManager.Instance.OnUpdate();
            BattleHackingCollisionManager.Instance.OnUpdate();
            BattleHackingCameraManager.Instance.OnUpdate();
            BattleHackingUiManager.Instance.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            BattleHackingTimerManager.Instance.OnLateUpdate();
            BattleHackingPlayerManager.Instance.OnLateUpdate();
            BattleHackingEnemyManager.Instance.OnLateUpdate();
            BattleHackingBulletManager.Instance.OnLateUpdate();
            BattleHackingEffectManager.Instance.OnLateUpdate();
            BattleHackingCollisionManager.Instance.OnLateUpdate();
            BattleHackingCameraManager.Instance.OnLateUpdate();
            BattleHackingUiManager.Instance.OnLateUpdate();

            // 衝突フラグクリア
            BattleHackingPlayerManager.Instance.ClearColliderFlag();
            BattleHackingEnemyManager.Instance.ClearColliderFlag();
            BattleHackingBulletManager.Instance.ClearColliderFlag();

            // 衝突情報の更新
            BattleHackingPlayerManager.Instance.UpdateCollider();
            BattleHackingEnemyManager.Instance.UpdateCollider();
            BattleHackingBulletManager.Instance.UpdateCollider();

            // 衝突判定処理
            BattleHackingCollisionManager.Instance.CheckCollision();
            BattleHackingCollisionManager.Instance.DrawCollider();

            // 衝突処理
            BattleHackingPlayerManager.Instance.ProcessCollision();
            BattleHackingEnemyManager.Instance.ProcessCollision();
            BattleHackingBulletManager.Instance.ProcessCollision();

            // ゲームの終了をチェック
            CheckGameEnd();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            BattleHackingTimerManager.Instance.OnFixedUpdate();
            BattleHackingPlayerManager.Instance.OnFixedUpdate();
            BattleHackingEnemyManager.Instance.OnFixedUpdate();
            BattleHackingBulletManager.Instance.OnFixedUpdate();
            BattleHackingEffectManager.Instance.OnFixedUpdate();
            BattleHackingCollisionManager.Instance.OnFixedUpdate();
            BattleHackingCameraManager.Instance.OnFixedUpdate();
            BattleHackingUiManager.Instance.OnFixedUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            BattleHackingInputManager.Instance.RemoveInput();
        }

        private void CheckGameEnd()
        {
            if (Target.m_IsDeadPlayer && Target.m_IsDeadBoss)
            {
                Target.RequestChangeState(E_BATTLE_HACKING_STATE.GAME_CLEAR);
            }
            else
            {
                if (Target.m_IsDeadPlayer)
                {
                    Target.RequestChangeState(E_BATTLE_HACKING_STATE.GAME_OVER);
                }
                if (Target.m_IsDeadBoss)
                {
                    Target.RequestChangeState(E_BATTLE_HACKING_STATE.GAME_CLEAR);
                }
            }
        }
    }
}
