partial class BattleRealManager
{
    private class GameOverState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            AudioManager.Instance.StopAllBgm();
            //AudioManager.Instance.Play(m_ParamSet.GameOverSe);
            //m_GameOverController.PlayGameOver();

            BattleRealPlayerManager.Instance.StopChargeShot();
            BattleRealUiManager.Instance.PlayGameOver();
            BattleRealUiManager.Instance.DisableAllBossUI();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //// 消滅の更新
            //BattleRealEnemyGroupManager.Instance.GotoPool();
            //BattleRealEnemyManager.Instance.GotoPool();
            //BattleRealBulletGeneratorManager.Instance.GotoPool();
            //BattleRealBulletManager.Instance.GotoPool();
            //BattleRealItemManager.Instance.GotoPool();
            //BattleRealEffectManager.Instance.GotoPool();
            //BattleRealCollisionManager.Instance.DestroyDrawingColliderMeshes();

            //BattleRealInputManager.Instance.OnUpdate();
            //BattleRealTimerManager.Instance.OnUpdate();
            //BattleRealEventManager.Instance.OnUpdate();
            //BattleRealPlayerManager.Instance.OnUpdate();
            //BattleRealEnemyGroupManager.Instance.OnUpdate();
            //BattleRealEnemyManager.Instance.OnUpdate();
            //BattleRealBulletGeneratorManager.Instance.OnUpdate();
            //BattleRealBulletManager.Instance.OnUpdate();
            //BattleRealItemManager.Instance.OnUpdate();
            //BattleRealEffectManager.Instance.OnUpdate();
            //BattleRealCameraManager.Instance.OnUpdate();
            BattleRealUiManager.Instance.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            //BattleRealTimerManager.Instance.OnLateUpdate();
            //BattleRealEventManager.Instance.OnLateUpdate();
            //BattleRealPlayerManager.Instance.OnLateUpdate();
            //BattleRealEnemyGroupManager.Instance.OnLateUpdate();
            //BattleRealEnemyManager.Instance.OnLateUpdate();
            //BattleRealBulletGeneratorManager.Instance.OnLateUpdate();
            //BattleRealBulletManager.Instance.OnLateUpdate();
            //BattleRealItemManager.Instance.OnLateUpdate();
            //BattleRealEffectManager.Instance.OnLateUpdate();
            //BattleRealCameraManager.Instance.OnLateUpdate();
            BattleRealUiManager.Instance.OnLateUpdate();

            // 衝突フラグクリア
            //BattleRealPlayerManager.Instance.ClearColliderFlag();
            //BattleRealEnemyManager.Instance.ClearColliderFlag();
            //BattleRealBulletManager.Instance.ClearColliderFlag();
            //BattleRealItemManager.Instance.ClearColliderFlag();

            //// 衝突情報の更新
            //BattleRealPlayerManager.Instance.UpdateCollider();
            //BattleRealEnemyManager.Instance.UpdateCollider();
            //BattleRealBulletManager.Instance.UpdateCollider();
            //BattleRealItemManager.Instance.UpdateCollider();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            //BattleRealTimerManager.Instance.OnFixedUpdate();
            //BattleRealEventManager.Instance.OnFixedUpdate();
            //BattleRealPlayerManager.Instance.OnFixedUpdate();
            //BattleRealEnemyGroupManager.Instance.OnFixedUpdate();
            //BattleRealEnemyManager.Instance.OnFixedUpdate();
            //BattleRealBulletGeneratorManager.Instance.OnFixedUpdate();
            //BattleRealBulletManager.Instance.OnFixedUpdate();
            //BattleRealItemManager.Instance.OnFixedUpdate();
            //BattleRealEffectManager.Instance.OnFixedUpdate();
            //BattleRealCameraManager.Instance.OnFixedUpdate();
            BattleRealUiManager.Instance.OnFixedUpdate();
        }
    }
}
