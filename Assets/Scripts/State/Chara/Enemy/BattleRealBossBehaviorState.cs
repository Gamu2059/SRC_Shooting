using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class BehaviorState : StateCycle
    {
        private bool m_UseBehavior;
        private BattleRealEnemyBehaviorUnit m_Behavior;
        private BattleRealEnemyBehaviorController m_BehaviorController;

        public override void OnStart()
        {
            base.OnStart();

            // 通常ではダメージコライダーを有効にする
            Target.GetCollider().SetEnableCollider(Target.m_DamageCollider, true);

            m_UseBehavior = false;
            if (Target.m_CurrentBehaviorSet != null)
            {
                m_UseBehavior = Target.m_CurrentBehaviorSet.BehaviorType == E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT;
                m_Behavior = Target.m_CurrentBehaviorSet.Behavior;
                m_BehaviorController = Target.m_BehaviorController;
                if (m_UseBehavior)
                {
                    m_Behavior.OnStartUnit(Target, null);
                }
                else
                {
                    m_BehaviorController.BuildBehavior(Target.m_CurrentBehaviorSet.BehaviorGroup);
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_UseBehavior)
            {
                m_Behavior?.OnUpdateUnit(Time.deltaTime);
            }
            else
            {
                m_BehaviorController?.OnUpdate();
            }
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();

            if (m_UseBehavior)
            {
                m_Behavior?.OnLateUpdateUnit(Time.deltaTime);
            }
            else
            {
                m_BehaviorController?.OnLateUpdate();
            }
            CheckChangeBehavior();
        }

        public override void OnEnd()
        {
            if (m_UseBehavior)
            {
                m_Behavior?.OnEndUnit();
            }
            else
            {
                m_BehaviorController?.OnEndUnit();
            }

            base.OnEnd();
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
