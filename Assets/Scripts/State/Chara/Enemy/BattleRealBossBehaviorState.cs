using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class BehaviorState : StateCycle
    {
        private BattleRealEnemyBehaviorUnit m_Behavior;

        public override void OnStart()
        {
            base.OnStart();

            // 通常ではダメージコライダーを有効にする
            Target.GetCollider().SetEnableCollider(Target.m_DamageCollider, true);

            m_Behavior = null;
            if (Target.m_CurrentBehaviorSet != null)
            {
                m_Behavior = Target.m_CurrentBehaviorSet.Behavior;
            }

            m_Behavior?.OnStart();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            m_Behavior?.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            m_Behavior?.OnLateUpdate();
            CheckChangeBehavior();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            m_Behavior?.OnFixedUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            m_Behavior?.OnEnd();
        }

        private void CheckChangeBehavior()
        {
            // 現在の振る舞いのインデックスが一番最後のものならば、これ以上振る舞いを変更しない
            if (Target.m_CurrentBehaviorSetIndex >= Target.m_BehaviorSets.Count - 1)
            {
                return;
            }

            var behaviorSet = Target.m_CurrentBehaviorSet;
            if (behaviorSet == null)
            {
                return;
            }

            if (Target.MaxHp <= 0)
            {
                return;
            }

            var rate = Target.NowHp / Target.MaxHp;
            if (rate <= behaviorSet.HpRateThresholdNextBehavior)
            {
                Target.RequestChangeState(E_STATE.CHANGE_BEHAVIOR);
            }
        }
    }
}
