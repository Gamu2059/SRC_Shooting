using UnityEngine;

partial class BattleRealManager
{
    private class DeadState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.PlayerManager.SetPlayerActive(false);

            // シェイクが邪魔になるので止める
            Target.CameraManager.StopShake();

            var battleData = DataManager.Instance.BattleData;
            if (battleData.PlayerLife < 1)
            {
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 1);
                timer.SetTimeoutCallBack(() =>
                {
                    timer = null;
                    Target.m_BattleManager.GameOver();
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
            Target.EnemyGroupManager.GotoPool();
            Target.EnemyManager.GotoPool();
            Target.BulletManager.GotoPool();
            Target.ItemManager.GotoPool();
            Target.EffectManager.GotoPool();
            Target.CollisionManager.DestroyDrawingColliderMeshes();

            //InputManager.OnUpdate();
            Target.RealTimerManager.OnUpdate();
            Target.EventManager.OnUpdate();
            //PlayerManager.OnUpdate();
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
            //PlayerManager.OnLateUpdate();
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
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Target.RealTimerManager.OnFixedUpdate();
            Target.EventManager.OnFixedUpdate();
            //PlayerManager.OnFixedUpdate();
            Target.EnemyGroupManager.OnFixedUpdate();
            Target.EnemyManager.OnFixedUpdate();
            Target.BulletManager.OnFixedUpdate();
            Target.ItemManager.OnFixedUpdate();
            Target.EffectManager.OnFixedUpdate();
            Target.CameraManager.OnFixedUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Target.PlayerManager.InitPlayerPosition();
            Target.PlayerManager.SetPlayerActive(true);
            Target.PlayerManager.SetPlayerInvinsible();
            Time.timeScale = 1;
        }
    }
}
