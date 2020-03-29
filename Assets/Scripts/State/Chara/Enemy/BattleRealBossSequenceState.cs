using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class SequenceState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            Target.GetCollider().SetEnableAllCollider(false);

            var sequenceGroup = Target.m_ReservedSequenceGroup;
            var sequenceController = Target.SequenceController;
            if (sequenceGroup == null || sequenceController == null)
            {
                Target.RequestChangeState(E_STATE.BEHAVIOR);
                return;
            }

            sequenceController.OnEndSequence += OnEndSequence;
            sequenceController.BuildSequence(sequenceGroup);
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
            Target.GetCollider().SetEnableAllCollider(true);
        }
    }
}
