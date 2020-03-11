partial class BattleRealManager
{
    private class WaitTalkState : StateCycle
    {
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
            Target.PlayerManager.OnUpdate();
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
            Target.PlayerManager.OnLateUpdate();
            Target.EnemyGroupManager.OnLateUpdate();
            Target.EnemyManager.OnLateUpdate();
            Target.BulletManager.OnLateUpdate();
            Target.ItemManager.OnLateUpdate();
            Target.EffectManager.OnLateUpdate();
            Target.CameraManager.OnLateUpdate();
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
