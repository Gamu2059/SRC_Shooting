using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class HackingSuccessState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();

            Target.HackingSuccessCount++;

            // ハッキングコンプリートの場合は救出される
            if (Target.HackingSuccessCount >= Target.HackingCompleteNum)
            {
                Target.RequestChangeState(E_STATE.RESCUE);
                return;
            }

            if (Target.m_CurrentBehaviorSet != null)
            {
                var transform = Target.transform;
                var behaviorSet = Target.m_CurrentBehaviorSet;
                BattleRealItemManager.Instance.CreateItem(transform.position, behaviorSet.HackingSuccessItemParam);

                // ハッキング成功エフェクトは、ボスのステートに合わせて破棄しないので作って放置
                BattleRealEffectManager.Instance.CreateEffect(Target.m_BossParam.HackingSuccessEffectParam, transform);
            }

            // HPの割合によらず、強制的に次の振る舞いへと遷移する
            Target.RequestChangeState(E_STATE.CHANGE_BEHAVIOR);
        }
    }
}
