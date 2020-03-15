using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealEnemyController
{
    private class BehaviorState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            var param = Target.m_EnemyParam;
            if (param != null && param.OnStartBehaviorEvents != null)
            {
                BattleRealEventManager.Instance.AddEvent(param.OnStartBehaviorEvents);
            }

            Target.m_EnemyBehavior?.OnStart();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.m_EnemyBehavior?.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            Target.m_EnemyBehavior?.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Target.m_EnemyBehavior?.OnFixedUpdate();
        }
    }
}
