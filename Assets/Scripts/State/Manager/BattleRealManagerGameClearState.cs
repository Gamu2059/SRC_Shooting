partial class BattleRealManager
{
    private class GameClearState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            var battleManager = Target.m_BattleManager;
            battleManager.BattleRealUiManager.SetEnableBossUI(false);
            Target.PlayerManager.StopChargeShot();

            battleManager.BattleRealUiManager.PlayGameClearAnimation();
            var hideViewWaitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1f);
            hideViewWaitTimer.SetTimeoutCallBack(() =>
            {
                battleManager.BattleRealUiManager.PlayMainViewHideAnimation();
                var resultWaitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1f);
                resultWaitTimer.SetTimeoutCallBack(() =>
                {
                    battleManager.BattleRealUiManager.DisplayResult();
                });
                TimerManager.Instance.RegistTimer(resultWaitTimer);
            });
            TimerManager.Instance.RegistTimer(hideViewWaitTimer);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            // 消滅の更新
            Target.EnemyGroupManager.GotoPool();
            Target.EnemyManager.GotoPool();
            Target.BulletManager.GotoPool();
            Target.ItemManager.GotoPool();
            Target.EffectManager.GotoPool();
            Target.CollisionManager.DestroyDrawingColliderMeshes();

            Target.RealTimerManager.OnUpdate();
            Target.EventManager.OnUpdate();
            Target.EnemyGroupManager.OnUpdate();
            Target.EnemyManager.OnUpdate();
            Target.BulletManager.OnUpdate();
            Target.ItemManager.OnUpdate();
            Target.EffectManager.OnUpdate();
            Target.CameraManager.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            Target.RealTimerManager.OnLateUpdate();
            Target.EventManager.OnLateUpdate();
            Target.EnemyGroupManager.OnLateUpdate();
            Target.EnemyManager.OnLateUpdate();
            Target.BulletManager.OnLateUpdate();
            Target.ItemManager.OnLateUpdate();
            Target.EffectManager.OnLateUpdate();
            Target.CameraManager.OnLateUpdate();

            // 衝突フラグクリア
            Target.PlayerManager.ClearColliderFlag();
            Target.EnemyManager.ClearColliderFlag();
            Target.BulletManager.ClearColliderFlag();
            Target.ItemManager.ClearColliderFlag();

            // 衝突情報の更新
            Target.PlayerManager.UpdateCollider();
            Target.EnemyManager.UpdateCollider();
            Target.BulletManager.UpdateCollider();
            Target.ItemManager.UpdateCollider();

            // 衝突判定処理
            Target.CollisionManager.CheckCollision();
            Target.CollisionManager.DrawCollider();

            // 衝突処理
            Target.PlayerManager.ProcessCollision();
            Target.EnemyManager.ProcessCollision();
            Target.BulletManager.ProcessCollision();
            Target.ItemManager.ProcessCollision();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Target.RealTimerManager.OnFixedUpdate();
            Target.EventManager.OnFixedUpdate();
            Target.PlayerManager.OnFixedUpdate();
            Target.EnemyGroupManager.OnFixedUpdate();
            Target.EnemyManager.OnFixedUpdate();
            Target.BulletManager.OnFixedUpdate();
            Target.ItemManager.OnFixedUpdate();
            Target.EffectManager.OnFixedUpdate();
            Target.CameraManager.OnFixedUpdate();
        }
    }
}
