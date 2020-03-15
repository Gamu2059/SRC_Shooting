partial class BattleManager
{
    /// <summary>
    /// リアルモードを動かすためのステート
    /// </summary>
    private partial class RealModeState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_RealManager.RequestChangeState(E_BATTLE_REAL_STATE.GAME);
            Target.m_HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.STAY_REAL);
            AudioManager.Instance.OperateAisac(E_AISAC_TYPE.AISAC_HACK, E_CUE_SHEET.BGM, 0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Target.m_IsStartHackingMode)
            {
                Target.m_IsStartHackingMode = false;
                Target.RequestChangeState(E_BATTLE_STATE.TO_HACKING);
            }

            Target.m_RealManager.OnUpdate();
            Target.m_HackingManager.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            Target.m_RealManager.OnLateUpdate();
            Target.m_HackingManager.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Target.m_RealManager.OnFixedUpdate();
            Target.m_HackingManager.OnFixedUpdate();
        }
    }
}
