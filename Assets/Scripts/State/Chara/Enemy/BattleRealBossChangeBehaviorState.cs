using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

partial class BattleRealBossController
{
    private class ChangeBehaviorState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            // このステートに遷移したら必ず次の振る舞いへとデータが切り替わる
            var nextBehaviorIndex = Math.Min(Target.m_CurrentBehaviorSetIndex + 1, Target.m_BehaviorSets.Count - 1);
            Target.ChangeBehaviorSet(nextBehaviorIndex);

            // 振る舞いが変わると、持ち越したハッキングモードのボスのダメージはリセットされる
            Target.m_CarryOverHackingBossDamage = 0;

            var behaviorSet = Target.m_CurrentBehaviorSet;
            if (behaviorSet == null)
            {
                Debug.LogWarningFormat("[{0}] : BehaviorSetがないため、シーケンスを実行できません。", GetType().Name);
                Target.RequestChangeState(E_STATE.BEHAVIOR);
                return;
            }

            var isStartSequence = Target.StartSequence(behaviorSet.SequenceGroupOnChangeBehavior);
            if (!isStartSequence)
            {
                Target.RequestChangeState(E_STATE.BEHAVIOR);
            }
        }
    }
}
