using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealEnemyController
{
    private class StartState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            var param = Target.m_EnemyParam;
            if (param != null && param.OnInitializeEvents != null)
            {
                BattleRealEventManager.Instance.AddEvent(param.OnInitializeEvents);
            }

            if (param != null && param.OnInitializeSequence)
            {
                Target.RequestChangeState(E_STATE.SEQUENCE);
            }
            else
            {
                Target.RequestChangeState(E_STATE.BEHAVIOR);
            }
        }
    }
}
