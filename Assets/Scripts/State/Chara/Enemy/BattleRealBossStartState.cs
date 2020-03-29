using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    partial class StartState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            Target.ChangeBehaviorSet(0);
            // 0で設定されているので、最大値まで回復させておく
            Target.RecoverDownHp(Target.MaxDownHp);
            Target.HackingCompleteNum = Target.m_BehaviorSets.Count;
            Target.HackingSuccessCount = 0;

            var param = Target.m_BossParam;
            if (param != null && param.OnInitializeEvents != null)
            {
                BattleRealEventManager.Instance.AddEvent(param.OnInitializeEvents);
            }

            if (param != null && param.OnInitializeSequence)
            {
                Target.StartSequence(param.OnInitializeSequence);
            }
            else
            {
                Target.RequestChangeState(E_STATE.BEHAVIOR);
            }
        }
    }
}
