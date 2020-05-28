using System;
using UniRx;

partial class BattleRealManager
{
    private class GameClearState : StateCycle
    {
        private IDisposable m_Disposable;

        public override void OnStart()
        {
            base.OnStart();

            // チャプターをして終了
            DataManager.Instance.OnChapterEnd(true);

            //GameManager.Instance.PlayerRecordManager.AddStoryModeRecord(new PlayerRecord("Nanashi", resultData.TotalScore, E_CHAPTER.NORMAL_1, DateTime.Now));

            AudioManager.Instance.StopAllBgm();
            AudioManager.Instance.StopAllSe();

            AudioManager.Instance.Play(E_COMMON_SOUND.GAME_CLEAR);
            BattleRealPlayerManager.Instance.StopChargeShot();
            BattleRealUiManager.Instance.DisableAllBossUI();
            BattleRealUiManager.Instance.PlayClearTelop(() =>
            {
                if (DataManager.Instance.Chapter == E_CHAPTER.CHAPTER_0)
                {
                    // チャプター0の時はリザルト表示をスキップする
                    m_Disposable = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ => Target.End());
                    return;
                }

                BattleRealUiManager.Instance.ShowResult(Target.End);
            });
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

        public override void OnEnd()
        {
            m_Disposable?.Dispose();
            base.OnEnd();
        }
    }
}
