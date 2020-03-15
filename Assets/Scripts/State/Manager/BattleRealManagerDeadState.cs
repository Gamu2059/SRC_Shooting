using UnityEngine;

partial class BattleRealManager
{
    private class DeadState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleRealPlayerManager.Instance.SetPlayerActive(false);

            // シェイクが邪魔になるので止める
            BattleRealCameraManager.Instance.StopShake();

            var battleData = DataManager.Instance.BattleData;
            if (battleData.PlayerLife < 1)
            {
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 1);
                timer.SetTimeoutCallBack(() =>
                {
                    timer = null;
                    Target.GameOver();
                });
                TimerManager.Instance.RegistTimer(timer);
                Time.timeScale = 0f;
            }
            else
            {
                battleData.DecreasePlayerLife();
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 2);
                timer.SetTimeoutCallBack(() =>
                {
                    timer = null;
                    Target.RequestChangeState(E_BATTLE_REAL_STATE.GAME);
                });
                TimerManager.Instance.RegistTimer(timer);
                Time.timeScale = 0.1f;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            // 消滅の更新
            BattleRealEnemyGroupManager.Instance.GotoPool();
            BattleRealEnemyManager.Instance.GotoPool();
            BattleRealBulletManager.Instance.GotoPool();
            BattleRealItemManager.Instance.GotoPool();
            BattleRealEffectManager.Instance.GotoPool();
            BattleRealCollisionManager.Instance.DestroyDrawingColliderMeshes();

            BattleRealInputManager.Instance.OnUpdate();
            BattleRealTimerManager.Instance.OnUpdate();
            BattleRealEventManager.Instance.OnUpdate();
            //BattleRealPlayerManager.Instance.OnUpdate();
            BattleRealEnemyGroupManager.Instance.OnUpdate();
            BattleRealEnemyManager.Instance.OnUpdate();
            BattleRealBulletManager.Instance.OnUpdate();
            BattleRealItemManager.Instance.OnUpdate();
            BattleRealEffectManager.Instance.OnUpdate();
            BattleRealCameraManager.Instance.OnUpdate();
            BattleRealUiManager.Instance.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            BattleRealTimerManager.Instance.OnLateUpdate();
            BattleRealEventManager.Instance.OnLateUpdate();
            //BattleRealPlayerManager.Instance.OnLateUpdate();
            BattleRealEnemyGroupManager.Instance.OnLateUpdate();
            BattleRealEnemyManager.Instance.OnLateUpdate();
            BattleRealBulletManager.Instance.OnLateUpdate();
            BattleRealItemManager.Instance.OnLateUpdate();
            BattleRealEffectManager.Instance.OnLateUpdate();
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
            //BattleRealPlayerManager.Instance.OnFixedUpdate();
            BattleRealEnemyGroupManager.Instance.OnFixedUpdate();
            BattleRealEnemyManager.Instance.OnFixedUpdate();
            BattleRealBulletManager.Instance.OnFixedUpdate();
            BattleRealItemManager.Instance.OnFixedUpdate();
            BattleRealEffectManager.Instance.OnFixedUpdate();
            BattleRealCameraManager.Instance.OnFixedUpdate();
            BattleRealUiManager.Instance.OnFixedUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            BattleRealPlayerManager.Instance.SetRespawnPlayerPosition();
            BattleRealPlayerManager.Instance.SetPlayerActive(true);
            BattleRealPlayerManager.Instance.SetPlayerInvinsible();
            Time.timeScale = 1;
        }
    }
}
