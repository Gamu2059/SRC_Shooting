using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealEnemyController
{
    private class SequenceState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            var param = Target.m_EnemyParam;
            var sequenceController = Target.SequenceController;
            if (param == null || param.OnInitializeSequence == null || sequenceController == null)
            {
                Target.RequestChangeState(E_STATE.BEHAVIOR);
                return;
            }

            sequenceController.OnEndSequence += OnEndSequence;
            sequenceController.BuildSequence(param.OnInitializeSequence);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.SequenceController?.OnUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            if (Target.SequenceController != null)
            {
                Target.SequenceController.OnEndSequence -= OnEndSequence;
            }
        }

        private void OnEndSequence()
        {
            Target.RequestChangeState(E_STATE.BEHAVIOR);
        }
    }
}
