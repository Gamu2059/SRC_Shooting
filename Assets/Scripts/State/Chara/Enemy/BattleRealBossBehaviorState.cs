using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class BehaviorState : StateCycle
    {
        private BehaviorSet m_BehaviorSet;
        private BattleRealEnemyBehaviorUnit m_Behavior;
        private BattleRealEnemyBehaviorController m_BehaviorController;

        /// <summary>
        /// このステートに遷移したのが1回目かどうか
        /// </summary>
        private bool m_IsFirstTime;

        public BehaviorState() : base()
        {
            m_IsFirstTime = true;
        }

        public override void OnStart()
        {
            base.OnStart();

            // 通常ではダメージコライダーを有効にする
            Target.GetCollider().SetEnableCollider(Target.m_EnemyBodyCollider, true);

            if (m_IsFirstTime)
            {
                var param = Target.m_BossParam;
                if (param != null && param.OnStartBehaviorEvents != null)
                {
                    BattleRealEventManager.Instance.AddEvent(param.OnStartBehaviorEvents);
                }
            }

            m_BehaviorSet = Target.m_CurrentBehaviorSet;
            if (m_BehaviorSet != null)
            {
                m_Behavior = m_BehaviorSet.Behavior;
                m_BehaviorController = Target.m_BehaviorController;
                switch (m_BehaviorSet.BehaviorType)
                {
                    case E_ENEMY_BEHAVIOR_TYPE.NONE:
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                        m_Behavior?.OnStartUnit(Target, null);
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                        m_BehaviorController?.BuildBehavior(m_BehaviorSet.BehaviorGroup);
                        break;
                }
            }
            m_IsFirstTime = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_BehaviorSet != null)
            {
                switch (m_BehaviorSet.BehaviorType)
                {
                    case E_ENEMY_BEHAVIOR_TYPE.NONE:
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                        m_Behavior?.OnUpdateUnit(Time.deltaTime);
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                        m_BehaviorController?.OnUpdate();
                        break;
                }
            }
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();

            if (m_BehaviorSet != null)
            {
                switch (m_BehaviorSet.BehaviorType)
                {
                    case E_ENEMY_BEHAVIOR_TYPE.NONE:
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                        m_Behavior?.OnLateUpdateUnit(Time.deltaTime);
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                        m_BehaviorController?.OnLateUpdate();
                        break;
                }
            }

            CheckChangeBehavior();
        }

        public override void OnEnd()
        {
            if (m_BehaviorSet != null)
            {
                switch (m_BehaviorSet.BehaviorType)
                {
                    case E_ENEMY_BEHAVIOR_TYPE.NONE:
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                        m_Behavior?.OnEndUnit();
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                        m_BehaviorController?.OnEndUnit();
                        break;
                }
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
