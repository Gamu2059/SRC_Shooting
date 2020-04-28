using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleHackingManager
{
    private class GameOverState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            var waitTimer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.UNSCALED_TIMER, 0.6f);
            waitTimer.SetTimeoutCallBack(() =>
            {
                waitTimer = null;
                Target.RequestChangeStateBattleManagerAction?.Invoke(E_BATTLE_STATE.TO_REAL);
            });
            TimerManager.Instance.RegistTimer(waitTimer);
            HackingDataHolder.IsHackingSuccess = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            BattleHackingEffectManager.Instance.OnUpdate();
            BattleHackingCameraManager.Instance.OnUpdate();
            BattleHackingUiManager.Instance.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            BattleHackingEffectManager.Instance.OnLateUpdate();
            BattleHackingCameraManager.Instance.OnLateUpdate();
            BattleHackingUiManager.Instance.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            BattleHackingEffectManager.Instance.OnFixedUpdate();
            BattleHackingCameraManager.Instance.OnFixedUpdate();
            BattleHackingUiManager.Instance.OnFixedUpdate();
        }
    }
}
