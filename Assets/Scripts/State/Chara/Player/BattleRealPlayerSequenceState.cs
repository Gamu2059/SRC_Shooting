partial class BattleRealPlayerController
{
    private class SequenceState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            var sequenceGroup = Target.m_SequenceGroup;
            var sequenceController = Target.m_SequenceController;
            if (sequenceGroup == null || sequenceController == null)
            {
                Target.RequestChangeState(E_STATE.GAME);
                return;
            }

            sequenceController.OnEndSequence += OnEndSequence;
            sequenceController.BuildSequence(sequenceGroup);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.m_SequenceController?.OnUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            if (Target.m_SequenceController != null)
            {
                Target.m_SequenceController.OnEndSequence -= OnEndSequence;
            }
        }

        private void OnEndSequence()
        {
            Target.RequestChangeState(E_STATE.GAME);
        }
    }
}
