using System;
partial class BattleRealManager
{
    private class GameClearState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            // ステージクリアした時しか記録しない
            var resultData = DataManager.Instance.BattleResultData;
            var battleData = DataManager.Instance.BattleData;
            resultData.ClacScore(battleData);

            GameManager.Instance.PlayerRecordManager.AddStoryModeRecord(new PlayerRecord("Nanashi", resultData.TotalScore, E_STAGE.NORMAL_1, DateTime.Now));

            AudioManager.Instance.StopAllBgm();
            AudioManager.Instance.StopAllSe();

            BattleRealUiManager.Instance.SetEnableBossUI(false);
            //AudioManager.Instance.Play(m_ParamSet.GameClearSe);

            //var battleManager = Target.m_BattleManager;
            //battleManager.BattleRealUiManager.SetEnableBossUI(false);
            //Target.PlayerManager.StopChargeShot();

            //battleManager.BattleRealUiManager.PlayGameClearAnimation();
            //var hideViewWaitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1f);
            //hideViewWaitTimer.SetTimeoutCallBack(() =>
            //{
            //    battleManager.BattleRealUiManager.PlayMainViewHideAnimation();
            //    var resultWaitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1f);
            //    resultWaitTimer.SetTimeoutCallBack(() =>
            //    {
            //        battleManager.BattleRealUiManager.DisplayResult();
            //    });
            //    TimerManager.Instance.RegistTimer(resultWaitTimer);
            //});
            //TimerManager.Instance.RegistTimer(hideViewWaitTimer);
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
            BattleRealCollisionManager.Instance.DestroyDrawingColliderMeshes();

            BattleRealTimerManager.Instance.OnUpdate();
            BattleRealEventManager.Instance.OnUpdate();
            //BattleRealPlayerManager.Instance.OnUpdate();
            BattleRealEnemyGroupManager.Instance.OnUpdate();
            BattleRealEnemyManager.Instance.OnUpdate();
            BattleRealBulletGeneratorManager.Instance.OnUpdate();
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
            BattleRealBulletGeneratorManager.Instance.OnLateUpdate();
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

            // 衝突判定処理
            BattleRealCollisionManager.Instance.CheckCollision();
            BattleRealCollisionManager.Instance.DrawCollider();

            // 衝突処理
            BattleRealPlayerManager.Instance.ProcessCollision();
            BattleRealEnemyManager.Instance.ProcessCollision();
            BattleRealBulletManager.Instance.ProcessCollision();
            BattleRealItemManager.Instance.ProcessCollision();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            BattleRealTimerManager.Instance.OnFixedUpdate();
            BattleRealEventManager.Instance.OnFixedUpdate();
            //BattleRealPlayerManager.Instance.OnFixedUpdate();
            BattleRealEnemyGroupManager.Instance.OnFixedUpdate();
            BattleRealEnemyManager.Instance.OnFixedUpdate();
            BattleRealBulletGeneratorManager.Instance.OnFixedUpdate();
            BattleRealBulletManager.Instance.OnFixedUpdate();
            BattleRealItemManager.Instance.OnFixedUpdate();
            BattleRealEffectManager.Instance.OnFixedUpdate();
            BattleRealCameraManager.Instance.OnFixedUpdate();
            BattleRealUiManager.Instance.OnFixedUpdate();
        }
    }
}
