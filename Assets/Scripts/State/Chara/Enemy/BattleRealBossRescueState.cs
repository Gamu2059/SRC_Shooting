using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class RescueState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            Target.IsDead = true;
            Target.GetCollider().SetEnableAllCollider(false);

            var defeatTime = 0f;
            if (Target.m_BossParam != null)
            {
                // シーケンシャルエフェクトを登録する
                var effect = Target.m_BossParam.RescueSequentialEffect;
                if (effect != null)
                {
                    BattleRealEffectManager.Instance.RegisterSequentialEffect(effect, Target.transform);
                }

                defeatTime = Target.m_BossParam.RescueHideTime;
            }

            if (defeatTime <= 0)
            {
                Target.OnRescueDestroy();
            }
            else
            {
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, defeatTime);
                timer.SetTimeoutCallBack(() => Target.OnRescueDestroy());
                BattleRealTimerManager.Instance.RegistTimer(timer);
            }

            AudioManager.Instance.Stop(E_CUE_SHEET.ENEMY);

            // 全弾削除
            BattleRealBulletManager.Instance.CheckPoolBullet(Target);
            Target.ExecuteResuceEvent();
        }
    }
}
