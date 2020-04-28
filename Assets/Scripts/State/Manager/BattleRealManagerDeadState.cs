using UnityEngine;

partial class BattleRealManager
{
    private class DeadState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleRealPlayerManager.Instance.DeadPlayer();

            // シェイクが邪魔になるので止める
            BattleRealCameraManager.Instance.StopShake();

            var testDataManager = BattleTestDataManager.Instance;
            var isNotGameOver = testDataManager != null && testDataManager.IsNotGameOver;

            var battleData = DataManager.Instance.BattleData;
            if (!isNotGameOver && battleData.IsGameOver())
            {
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1);
                timer.SetTimeoutCallBack(() =>
                {
                    timer = null;
                    Target.GameOver();
                });
                BattleRealTimerManager.Instance.RegistTimer(timer);
            }
            else
            {
                battleData.DecreasePlayerLife();
                battleData.ResetChain();
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1);
                timer.SetTimeoutCallBack(() =>
                {
                    timer = null;
                    BattleRealPlayerManager.Instance.RespawnPlayer(true);
                    Target.RequestChangeState(E_BATTLE_REAL_STATE.GAME);
                });
                BattleRealTimerManager.Instance.RegistTimer(timer);
            }
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

            BattleRealInputManager.Instance.OnUpdate();
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
    }
}
