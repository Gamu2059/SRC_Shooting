using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealEnemyController
{
    private class RetireState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            Target.GetCollider().SetEnableAllCollider(false);

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

            Target.ExecuteRetireEvent();
        }
    }
}
