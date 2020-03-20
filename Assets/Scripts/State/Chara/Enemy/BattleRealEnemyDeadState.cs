using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealEnemyController
{
    private class DeadState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            var defeatTime = 0f;
            if (Target.Param != null)
            {
                // シーケンシャルエフェクトを登録する
                var effect = Target.Param.DefeatSequentialEffect;
                if (effect != null)
                {
                    BattleRealEffectManager.Instance.RegisterSequentialEffect(effect, Target.transform);
                }

                defeatTime = Target.Param.DefeatHideTime;
            }

            if (defeatTime <= 0)
            {
                Target.OnDefeatDestroy();
            }
            else
            {
                var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, defeatTime);
                timer.SetTimeoutCallBack(() => Target.OnDefeatDestroy());
                BattleRealTimerManager.Instance.RegistTimer(timer);
            }

            Target.ExecuteDefeatEvent();
        }
    }
}
