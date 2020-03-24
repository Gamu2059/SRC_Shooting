using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealEnemyController
{
    private class BehaviorState : StateCycle
    {
        private bool m_UseBehavior;
        private BattleRealEnemyBehaviorUnit m_Behavior;
        private BattleRealEnemyBehaviorController m_BehaviorController;

        public override void OnStart()
        {
            base.OnStart();

            var param = Target.m_EnemyParam;
            if (param != null && param.OnStartBehaviorEvents != null)
            {
                BattleRealEventManager.Instance.AddEvent(param.OnStartBehaviorEvents);
            }

            m_UseBehavior = Target.m_BehaviorType == E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT;
            m_Behavior = Target.m_Behavior;
            m_BehaviorController = Target.m_BehaviorController;
            if (m_UseBehavior)
            {
                m_Behavior?.OnStartUnit(Target, null);
            }
            else
            {
                m_BehaviorController?.BuildBehavior(Target.m_BehaviorGroup);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_UseBehavior)
            {
                if (m_Behavior == null)
                {
                    Target.Destroy();
                    return;
                }

                m_Behavior.OnUpdateUnit(Time.deltaTime);
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
                if (m_Behavior == null)
                {
                    Target.Destroy();
                    return;
                }

                m_Behavior.OnLateUpdateUnit(Time.deltaTime);
                if (m_Behavior.IsEndUnit())
                {
                    Target.Destroy();
                    return;
                }
            }
            else
            {
                m_BehaviorController?.OnLateUpdate();
            }
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
    }
}
