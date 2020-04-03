#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.EnemyGenerator
{
    /// <summary>
    /// リアルモードの敵の生成振る舞いの基底クラス。
    /// </summary>
    public class BattleRealEnemyGeneratorBase : ScriptableObject, IControllableGameCycle
    {
        #region Field Inspector

        [Header("Generator Parameter")]

        [SerializeField, Tooltip("生成終了条件を適用するかどうか")]
        private bool m_UseStopCondition;

        [SerializeField, Tooltip("生成終了条件")]
        private EventTriggerRootCondition m_StopCondition;

        #endregion

        protected BattleRealEnemyGroupController EnemyGroup { get; private set; }
        protected bool IsStopGenerate { get; private set; }

        public void SetEnemyGroup(BattleRealEnemyGroupController enemyGroup)
        {
            EnemyGroup = enemyGroup;
        }

        public virtual void OnInitialize() { }

        public virtual void OnFinalize() { }

        public void OnStart()
        {
            IsStopGenerate = false;
            OnStartGenerator();
        }

        public void OnUpdate()
        {
            if (m_UseStopCondition && !IsStopGenerate)
            {
                IsStopGenerate = BattleRealEventManager.Instance.IsMeetRootCondition(ref m_StopCondition);
            }

            if (!IsStopGenerate)
            {
                OnUpdateGenerator();
            }
        }

        public void OnLateUpdate()
        {
            if (!IsStopGenerate)
            {
                OnLateUpdateGenerator();
            }
        }

        public void OnFixedUpdate()
        {
            if (!IsStopGenerate)
            {
                OnFixedUpdateGenerator();
            }
        }

        public virtual void OnEnd() { }

        #region Have to Override

        protected virtual void OnStartGenerator() { }
        protected virtual void OnUpdateGenerator() { }
        protected virtual void OnLateUpdateGenerator() { }
        protected virtual void OnFixedUpdateGenerator() { }

        #endregion
    }
}
