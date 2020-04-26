using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class HackingFailureState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            if (Target.m_CurrentBehaviorSet != null)
            {
                // ハッキング失敗エフェクトは、ボスのステートに合わせて破棄しないので作って放置
                BattleRealEffectManager.Instance.CreateEffect(Target.m_BossParam.HackingFailureEffectParam, Target.transform);
            }

            var recoverDownHp = Target.MaxDownHp * Target.m_BossParam.HackingFailureRecoverDownHpRate;
            Target.RecoverDownHp(recoverDownHp);
            Target.RequestChangeState(E_STATE.BEHAVIOR);
        }
    }
}
