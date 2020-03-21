#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.EnemyGenerator
{
    /// <summary>
    /// 生成座標とタイミングを指定して敵を生成していくジェネレータ。
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGenerator/Default", fileName = "default.battle_real_enemy_generator.asset", order = 0)]
    public class DefaultEnemyGenerator : BattleRealEnemyGenerator
    {
        #region Define

        [Serializable]
        private class IndividualParam
        {
            [SerializeField, Tooltip("敵グループが生成されてからの相対的な生成時間。"), Min(0)]
            private float m_GenerateTime;
            public float GenerateTime => m_GenerateTime;

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

        [SerializeField]
        private BattleRealEnemyParamSetBase m_DefaultParamSet;
        public BattleRealEnemyParamSetBase DefaultParamSet => m_DefaultParamSet;

        [SerializeField]
        private IndividualParam[] m_IndividualParams;
        private IndividualParam[] IndividualParams => m_IndividualParams;

        #endregion

        #region Field

        private List<IndividualParam> m_GenerateParams;
        private float m_GenerateEnemyTimeCount;

        #endregion

        #region Game Cycle

        public override void OnStart()
        {
            base.OnStart();

            m_GenerateEnemyTimeCount = 0;
            m_GenerateParams = new List<IndividualParam>();
            if (IndividualParams != null)
            {
                m_GenerateParams.AddRange(IndividualParams);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_GenerateParams.Count < 1)
            {
                return;
            }

            foreach(var param in m_GenerateParams)
            {
                if (m_GenerateEnemyTimeCount >= param.GenerateTime)
                {
                    Generate(param);
                }
            }

            m_GenerateEnemyTimeCount += Time.deltaTime;
            m_GenerateParams.RemoveAll( p => m_GenerateEnemyTimeCount >= p.GenerateTime);
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
                var angles = enemyT.localEulerAngles;
                angles.y = param.GenerateAngle;
                enemyT.localEulerAngles = angles;
            }
            else
            {
                enemyT.position = pos;
                var angles = enemyT.eulerAngles;
                angles.y = param.GenerateAngle;
                enemyT.eulerAngles = angles;
            }
        }
    }
}
