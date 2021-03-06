﻿using UnityEngine;

partial class BattleRealManager
{
    private class GameState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleRealStageManager.Instance.gameObject.SetActive(true);
            BattleRealUiManager.Instance.SetAlpha(1);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            // 消滅の更新
            BattleRealEnemyGroupManager.Instance.GotoPool();
            BattleRealEnemyManager.Instance.GotoPool();
            BattleRealBulletGeneratorManager.Instance.GotoPool();
            BattleRealBulletManager.Instance.GotoPool();
            BattleRealItemManager.Instance.GotoPool();
            BattleRealEffectManager.Instance.GotoPool();
            BattleRealSequenceObjectManager.Instance.GotoDestroy();
            BattleRealCollisionManager.Instance.DestroyDrawingColliderMeshes();

            BattleRealTimerManager.Instance.OnUpdate();
            BattleRealEventManager.Instance.OnUpdate();
            BattleRealPlayerManager.Instance.OnUpdate();
            BattleRealEnemyGroupManager.Instance.OnUpdate();
            BattleRealEnemyManager.Instance.OnUpdate();
            BattleRealBulletGeneratorManager.Instance.OnUpdate();
            BattleRealBulletManager.Instance.OnUpdate();
            BattleRealItemManager.Instance.OnUpdate();
            BattleRealEffectManager.Instance.OnUpdate();
            BattleRealSequenceObjectManager.Instance.OnUpdate();
            BattleRealCameraManager.Instance.OnUpdate();
            BattleRealUiManager.Instance.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            BattleRealTimerManager.Instance.OnLateUpdate();
            BattleRealEventManager.Instance.OnLateUpdate();
            BattleRealPlayerManager.Instance.OnLateUpdate();
            BattleRealEnemyGroupManager.Instance.OnLateUpdate();
            BattleRealEnemyManager.Instance.OnLateUpdate();
            BattleRealBulletGeneratorManager.Instance.OnLateUpdate();
            BattleRealBulletManager.Instance.OnLateUpdate();
            BattleRealItemManager.Instance.OnLateUpdate();
            BattleRealEffectManager.Instance.OnLateUpdate();
            BattleRealSequenceObjectManager.Instance.OnLateUpdate();
            BattleRealCameraManager.Instance.OnLateUpdate();
            BattleRealUiManager.Instance.OnLateUpdate();

            // 衝突フラグクリア
            BattleRealPlayerManager.Instance.ClearColliderFlag();
            BattleRealEnemyManager.Instance.ClearColliderFlag();
            BattleRealBulletManager.Instance.ClearColliderFlag();
            BattleRealItemManager.Instance.ClearColliderFlag();

            // 衝突情報の更新
            BattleRealPlayerManager.Instance.UpdateCollider();
            BattleRealEnemyManager.Instance.UpdateCollider();
            BattleRealBulletManager.Instance.UpdateCollider();
            BattleRealItemManager.Instance.UpdateCollider();

            // 衝突判定処理
            BattleRealCollisionManager.Instance.CheckCollision();
            BattleRealCollisionManager.Instance.DrawCollider();

            // 衝突処理
            BattleRealPlayerManager.Instance.ProcessCollision();
            BattleRealEnemyManager.Instance.ProcessCollision();
            BattleRealBulletManager.Instance.ProcessCollision();
            BattleRealItemManager.Instance.ProcessCollision();

            CheckDeadPlayer();

            DebugInput();
        }

        private void CheckDeadPlayer()
        {
            var player = BattleRealPlayerManager.Instance.Player;
            if (player != null && player.IsDead)
            {
                Target.RequestChangeState(E_BATTLE_REAL_STATE.DEAD);
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            BattleRealTimerManager.Instance.OnFixedUpdate();
            BattleRealEventManager.Instance.OnFixedUpdate();
            BattleRealPlayerManager.Instance.OnFixedUpdate();
            BattleRealEnemyGroupManager.Instance.OnFixedUpdate();
            BattleRealEnemyManager.Instance.OnFixedUpdate();
            BattleRealBulletGeneratorManager.Instance.OnFixedUpdate();
            BattleRealBulletManager.Instance.OnFixedUpdate();
            BattleRealItemManager.Instance.OnFixedUpdate();
            BattleRealEffectManager.Instance.OnFixedUpdate();
            BattleRealSequenceObjectManager.Instance.OnFixedUpdate();
            BattleRealCameraManager.Instance.OnFixedUpdate();
            BattleRealUiManager.Instance.OnFixedUpdate();
        }

        private void DebugInput()
        {
            if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl))
            {
                DataManager.Instance.BattleData.AddPlayerLife(1);
            }
            else if (Input.GetKeyDown(KeyCode.W) && Input.GetKey(KeyCode.LeftControl))
            {
                DataManager.Instance.BattleData.LevelInChapter.Value++;
            }
            else if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftControl))
            {
                DataManager.Instance.BattleData.IncreaseEnergyStock();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 5;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 6;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 7;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 8;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9) && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 9;
            }
        }
    }
}
