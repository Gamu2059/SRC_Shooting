using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleRealBossController
{
    private class DownBehaviorState : StateCycle
    {
        private BehaviorSet m_BehaviorSet;
        private BattleRealEnemyBehaviorUnit m_Behavior;
        private BattleRealEnemyBehaviorController m_BehaviorController;

        private float m_DownHealTime;
        private BattleCommonEffectController m_DownEffect;
        private BattleCommonEffectController m_DownPlayerTriangleEffect;

        public override void OnStart()
        {
            base.OnStart();

            // ハッキングから帰ってきた時のコールバックを登録する
            BattleRealEnemyManager.Instance.FromHackingAction += OnFromHacking;

            // ダウン中はダメージコライダーを無効にする
            Target.GetCollider().SetEnableCollider(Target.m_EnemyBodyCollider, false);

            BattleRealBulletManager.Instance.CheckPoolBullet(Target);
            AudioManager.Instance.Play(E_COMMON_SOUND.BOSS_DOWN);

            m_BehaviorSet = Target.m_CurrentBehaviorSet;
            if (m_BehaviorSet != null)
            {
                m_DownHealTime = m_BehaviorSet.DownHealTime;
                m_Behavior = m_BehaviorSet.DownBehavior;
                m_BehaviorController = Target.m_DownBehaviorController;
                switch (m_BehaviorSet.DownBehaviorType)
                {
                    case E_ENEMY_BEHAVIOR_TYPE.NONE:
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                        m_Behavior?.OnStartUnit(Target, null);
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                        m_BehaviorController?.BuildBehavior(m_BehaviorSet.DownBehaviorGroup);
                        break;
                }
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
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_BehaviorSet != null)
            {
                switch (m_BehaviorSet.BehaviorType)
                {
                    case E_ENEMY_BEHAVIOR_TYPE.NONE:
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                        m_Behavior?.OnUpdateUnit(Time.deltaTime);
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                        m_BehaviorController?.OnUpdate();
                        break;
                }
            }

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

            if (m_BehaviorSet != null)
            {
                switch (m_BehaviorSet.BehaviorType)
                {
                    case E_ENEMY_BEHAVIOR_TYPE.NONE:
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                        m_Behavior?.OnLateUpdateUnit(Time.deltaTime);
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                        m_BehaviorController?.OnLateUpdate();
                        break;
                }
            }
        }

        public override void OnEnd()
        {
            m_DownPlayerTriangleEffect?.DestroyEffect(true);
            m_DownEffect?.DestroyEffect(true);

            if (m_BehaviorSet != null)
            {
                switch (m_BehaviorSet.BehaviorType)
                {
                    case E_ENEMY_BEHAVIOR_TYPE.NONE:
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT:
                        m_Behavior?.OnStopUnit();
                        break;
                    case E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_CONTROLLER:
                        m_BehaviorController?.StopBehavior();
                        break;
                }
            }

            // ハッキングから帰ってきた時のコールバックを削除する
            BattleRealEnemyManager.Instance.FromHackingAction -= OnFromHacking;

            base.OnEnd();
        }

        private void HealDown()
        {
            Target.NowDownHp = Target.MaxDownHp;
            Target.RequestChangeState(E_STATE.BEHAVIOR);

            AudioManager.Instance.Play(E_COMMON_SOUND.BOSS_DOWN_RETURN);
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
