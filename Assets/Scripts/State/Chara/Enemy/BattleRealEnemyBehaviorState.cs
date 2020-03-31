using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealEnemyController
{
    private class BehaviorState : StateCycle
    {
        private BattleRealEnemyBehaviorUnit m_Behavior;
        private BattleRealEnemyBehaviorController m_BehaviorController;

        public override void OnStart()
        {
            base.OnStart();

            Target.GetCollider().SetEnableAllCollider(true);
            Target.WillDestroyOnOutOfEnemyField = true;

            var param = Target.m_EnemyParam;
            if (param != null && param.OnStartBehaviorEvents != null)
            {
                BattleRealEventManager.Instance.AddEvent(param.OnStartBehaviorEvents);
            }

            m_Behavior = Target.m_Behavior;
            m_BehaviorController = Target.m_BehaviorController;
            switch (Target.m_BehaviorType)
            {
                case E_ENEMY_BEHAVIOR_TYPE.NONE:
                    break;
                case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                    m_Behavior?.OnStartUnit(Target, null);
                    break;
                case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                    m_BehaviorController?.BuildBehavior(Target.m_BehaviorGroup);
                    break;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            switch (Target.m_BehaviorType)
            {
                case E_ENEMY_BEHAVIOR_TYPE.NONE:
                    break;
                case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                    if (m_Behavior == null)
                    {
                        Target.OnRetireDestroy();
                        return;
                    }

                    m_Behavior.OnUpdateUnit(Time.deltaTime);
                    break;
                case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                    m_BehaviorController?.OnUpdate();
                    break;
            }
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();

            switch (Target.m_BehaviorType)
            {
                case E_ENEMY_BEHAVIOR_TYPE.NONE:
                    break;
                case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                    if (m_Behavior == null)
                    {
                        Target.OnRetireDestroy();
                        return;
                    }

                    m_Behavior.OnLateUpdateUnit(Time.deltaTime);
                    if (m_Behavior.IsEndUnit())
                    {
                        Target.OnRetireDestroy();
                        return;
                    }
                    break;
                case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                    m_BehaviorController?.OnLateUpdate();
                    break;
            }
        }

        public override void OnEnd()
        {
            switch (Target.m_BehaviorType)
            {
                case E_ENEMY_BEHAVIOR_TYPE.NONE:
                    break;
                case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                    m_Behavior?.OnEndUnit();
                    m_Behavior?.OnStopUnit();
                    break;
                case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                    m_BehaviorController?.StopBehavior();
                    break;
            }

            base.OnEnd();
        }
    }
}
