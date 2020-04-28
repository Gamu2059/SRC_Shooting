using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleHackingManager
{
    private class GameClearState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            var destroyBulletTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 1f);
            destroyBulletTimer.SetTimeoutCallBack(() =>
            {
                destroyBulletTimer = null;

                var destroyBossTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 1f);
                destroyBossTimer.SetTimeoutCallBack(() =>
                {
                    destroyBossTimer = null;
                    Target.RequestChangeStateBattleManagerAction?.Invoke(E_BATTLE_STATE.TO_REAL);
                });
                TimerManager.Instance.RegistTimer(destroyBossTimer);

                BattleHackingCameraManager.Instance.Shake(Target.m_ParamSet.DestroyBossShakeParam);
            });
            TimerManager.Instance.RegistTimer(destroyBulletTimer);

            HackingDataHolder.IsHackingSuccess = true;
            BattleHackingBulletManager.Instance.DestroyAllEnemyBullet();
            BattleHackingCameraManager.Instance.Shake(Target.m_ParamSet.DestroyBulletShakeParam);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            BattleHackingBulletManager.Instance.GotoPool();

            BattleHackingEnemyManager.Instance.OnUpdate();
            BattleHackingEffectManager.Instance.OnUpdate();
            BattleHackingCameraManager.Instance.OnUpdate();
            BattleHackingUiManager.Instance.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            BattleHackingEnemyManager.Instance.OnLateUpdate();
            BattleHackingEffectManager.Instance.OnLateUpdate();
            BattleHackingCameraManager.Instance.OnLateUpdate();
            BattleHackingUiManager.Instance.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            BattleHackingEnemyManager.Instance.OnFixedUpdate();
            BattleHackingEffectManager.Instance.OnFixedUpdate();
            BattleHackingCameraManager.Instance.OnFixedUpdate();
            BattleHackingUiManager.Instance.OnFixedUpdate();
        }
    }
}
