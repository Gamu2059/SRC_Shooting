using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class DownBehaviorState : StateCycle
    {
        private BattleRealEnemyBehaviorUnit m_Behavior;
        private float m_DownHealTime;
        private BattleCommonEffectController m_DownEffect;
        private BattleCommonEffectController m_DownPlayerTriangleEffect;

        public override void OnStart()
        {
            base.OnStart();

            // ハッキングから帰ってきた時のコールバックを登録する
            BattleRealEnemyManager.Instance.FromHackingAction += OnFromHacking;

            // ダウン中はダメージコライダーを無効にする
            Target.GetCollider().SetEnableCollider(Target.m_DamageCollider, false);

            BattleRealBulletManager.Instance.CheckPoolBullet(Target);
            AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.DownSe);

            m_Behavior = null;
            m_DownHealTime = 0;
            if (Target.m_CurrentBehaviorSet != null)
            {
                m_Behavior = Target.m_CurrentBehaviorSet.DownBehavior;
                m_DownHealTime = Target.m_CurrentBehaviorSet.DownHealTime;
            }

            var effectManager = BattleRealEffectManager.Instance;
            m_DownEffect = effectManager.CreateEffect(Target.m_BossParam.DownEffectParam, Target.transform);
            m_DownPlayerTriangleEffect = effectManager.CreateEffect(Target.m_BossParam.PlayerTriangleEffectParam, Target.transform);

            // ダウンが意味をなさないパラメータの場合は即座に通常の振る舞いに戻す
            if (Target.MaxDownHp <= 0 || m_DownHealTime <= 0)
            {
                HealDown();
                return;
            }

            m_Behavior?.OnStart();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            m_Behavior?.OnUpdate();

            if (Target.MaxDownHp > 0 && m_DownHealTime > 0)
            {
                Target.NowDownHp += Target.MaxDownHp * Time.deltaTime / m_DownHealTime;                
                if (Target.NowDownHp >= Target.MaxDownHp)
                {
                    HealDown();
                }
            }
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            m_Behavior?.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            m_Behavior?.OnFixedUpdate();
        }

        public override void OnEnd()
        {
            base.OnEnd();
            m_DownPlayerTriangleEffect?.DestroyEffect(true);
            m_DownEffect?.DestroyEffect(true);
            m_Behavior?.OnEnd();

            // ハッキングから帰ってきた時のコールバックを削除する
            BattleRealEnemyManager.Instance.FromHackingAction -= OnFromHacking;
        }

        private void HealDown()
        {
            Target.NowDownHp = Target.MaxDownHp;
            Target.RequestChangeState(E_STATE.BEHAVIOR);
        }

        private void OnFromHacking()
        {
            if (HackingDataHolder.IsHackingSuccess)
            {
                Target.RequestChangeState(E_STATE.HACKING_SUCCESS);
            }
            else
            {
                Target.RequestChangeState(E_STATE.HACKING_FAILURE);
            }
        }
    }
}
