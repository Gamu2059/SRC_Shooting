#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.EnemyGenerator
{
    /// <summary>
    /// 生成座標と生成間隔を指定して敵を生成していくジェネレータ。
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/EnemyGenerator/DefaultInterval", fileName = "param.enemy_generator.asset")]
    public class DefaultIntervalEnemyGenerator : BattleRealEnemyGeneratorBase
    {
        #region Define

        [Serializable]
        protected class IndividualParam
        {
            [SerializeField, Tooltip("生成座標系を敵グループを基準にするか。")]
            private E_RELATIVE m_Relative = E_RELATIVE.RELATIVE;
            public E_RELATIVE Relative => m_Relative;

            [SerializeField, Tooltip("生成するビューポート座標。RELATIVEにすると、敵グループの位置の相対座標になる。")]
            private Vector2 m_ViewPortPos;
            public Vector2 ViewPortPos => m_ViewPortPos;

            [SerializeField, Tooltip("ビューポート座標からのオフセット座標。RELATIVEにすると、敵グループの位置の相対座標になる。")]
            private Vector2 m_OffsetPosFromViewPort;
            public Vector2 OffsetPosFromViewPort => m_OffsetPosFromViewPort;

            [SerializeField, Tooltip("生成角度。RELATIVEにすると、敵グループの角度の相対角度になる。")]
            private float m_GenerateAngle;
            public float GenerateAngle => m_GenerateAngle;

            [SerializeField, Tooltip("生成する敵のパラメータ。nullにしておくとデフォルトの敵のパラメータで生成する。")]
            private BattleRealEnemyParamSetBase m_ParamSet;
            public BattleRealEnemyParamSetBase ParamSet => m_ParamSet;
        }

        #endregion

        #region Field Inspector

        [Header("Default Interval Enemy Parameter")]

        [SerializeField]
        private float m_GenerateOffsetTime;
        protected float GenerateOffsetTime => m_GenerateOffsetTime;

        [SerializeField]
        private float m_GenerateInterval;
        protected float GenerateInterval => m_GenerateInterval;

        [SerializeField]
        private BattleRealEnemyParamSetBase m_DefaultParamSet;
        protected BattleRealEnemyParamSetBase DefaultParamSet => m_DefaultParamSet;

        [SerializeField]
        private IndividualParam[] m_IndividualParams;
        protected IndividualParam[] IndividualParams => m_IndividualParams;

        #endregion

        #region Field

        private float m_GenerateEnemyTimeCount;
        private bool m_IsWaitOffsetTime;

        #endregion

        #region Game Cycle

        protected override void OnStartGenerator()
        {
            base.OnStartGenerator();

            m_GenerateEnemyTimeCount = 0;
            m_IsWaitOffsetTime = GenerateOffsetTime > 0;
        }

        protected override void OnFixedUpdateGenerator()
        {
            base.OnFixedUpdateGenerator();

            if (m_IsWaitOffsetTime)
            {
                if (m_GenerateEnemyTimeCount >= GenerateOffsetTime)
                {
                    m_GenerateEnemyTimeCount -= GenerateOffsetTime;
                    m_IsWaitOffsetTime = false;
                }
                else
                {
                    m_GenerateEnemyTimeCount += Time.fixedDeltaTime;
                }
            }
            else
            {
                if (m_GenerateEnemyTimeCount >= GenerateInterval)
                {
                    m_GenerateEnemyTimeCount -= GenerateInterval;
                    foreach (var param in IndividualParams)
                    {
                        Generate(param);
                    }
                }
                else
                {
                    m_GenerateEnemyTimeCount += Time.fixedDeltaTime;
                }
            }
        }

        #endregion

        private void Generate(IndividualParam param)
        {
            if (param == null)
            {
                return;
            }

            var enemyParam = param.ParamSet != null ? param.ParamSet : DefaultParamSet;
            var enemy = BattleRealEnemyManager.Instance.CreateEnemy(enemyParam);
            if (enemy == null)
            {
                return;
            }

            var enemyT = enemy.transform;
            enemyT.SetParent(EnemyGroup.transform);

            var viewPortPos = param.ViewPortPos;
            var offsetPos = param.OffsetPosFromViewPort;
            var pos = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewPortPos.x, viewPortPos.y);
            pos += offsetPos.ToVector3XZ();

            if (param.Relative == E_RELATIVE.RELATIVE)
            {
                enemyT.localPosition = pos;
                enemyT.localEulerAngles = new Vector3(0, param.GenerateAngle, 0);
            }
            else
            {
                enemyT.SetPositionAndRotation(pos, Quaternion.Euler(0, param.GenerateAngle, 0));
            }
        }
    }
}
