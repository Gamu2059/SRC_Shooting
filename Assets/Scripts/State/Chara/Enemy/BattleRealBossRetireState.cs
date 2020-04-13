using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class RetireState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            var retireTime = 0f;
            if (Target.Param != null)
            {
                // シーケンシャルエフェクトを登録する
                var effect = Target.Param.RetireSequentialEffect;
                if (effect != null)
                {
                    BattleRealEffectManager.Instance.RegisterSequentialEffect(effect, Target.transform);
                }

                retireTime = Target.Param.RetireHideTime;
            }

            if (retireTime <= 0)
            {
                Target.OnRetireDestroy();
            }
            else
            {
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, retireTime);
                timer.SetTimeoutCallBack(() => Target.OnRetireDestroy());
                BattleRealTimerManager.Instance.RegistTimer(timer);
            }

            AudioManager.Instance.Stop(E_CUE_SHEET.ENEMY);

            // 全弾削除
            BattleRealBulletManager.Instance.CheckPoolBullet(Target);
            Target.ExecuteRetireEvent();
        }
    }
}
