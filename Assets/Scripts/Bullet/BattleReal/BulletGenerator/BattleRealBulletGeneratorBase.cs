#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// パラメータによって弾の生成方法を制御するスクリプト。
    /// </summary>
    public abstract class BattleRealBulletGeneratorBase : ControllableObject
    {
        public E_POOLED_OBJECT_CYCLE Cycle;
        protected IBattleRealBulletGeneratorParamBase Param { get; private set; }
        protected BattleRealCharaController Owner { get; private set; }
        protected BattleRealEnemyBase EnemyOwner { get; private set; }

        public void SetParam(IBattleRealBulletGeneratorParamBase param, BattleRealCharaController owner)
        {
            Param = param;
            Owner = owner;
            EnemyOwner = Owner as BattleRealEnemyBase;
            OnSetParam();
        }

        protected abstract void OnSetParam();

        public sealed override void OnUpdate()
        {
            base.OnUpdate();

            if (Owner == null)
            {
                Destroy();
            }

            if (EnemyOwner != null && EnemyOwner.GetCycle() == E_POOLED_OBJECT_CYCLE.STANDBY_POOL)
            {
                Destroy();
            }

            OnGeneratorUpdate(Time.deltaTime);
        }

        protected abstract void OnGeneratorUpdate(float deltaTime);

        public void Destroy()
        {
            if (Cycle != E_POOLED_OBJECT_CYCLE.STANDBY_CHECK_POOL && Cycle != E_POOLED_OBJECT_CYCLE.POOLED)
            {
                BattleRealBulletGeneratorManager.Instance.CheckStandbyPool(this);
            }
        }
    }
}
