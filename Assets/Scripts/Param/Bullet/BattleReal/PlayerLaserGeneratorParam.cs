using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// プレイヤーレーザージェネレータのパラメータ。
    /// </summary>
    [Serializable]
    public class PlayerLaserGeneratorParam : IBattleRealBulletGeneratorParamBase
    {
        [Serializable]
        public class LevelIndividualParam
        {
            [SerializeField, Tooltip("レーザーのプレハブ")]
            private BattleRealPlayerLaser m_Laser;
            public BattleRealPlayerLaser Laser => m_Laser;

            [SerializeField, Tooltip("レーザーの弾パラメータ")]
            private BulletParam m_LaserParam;
            public BulletParam LaserParam => m_LaserParam;
        }

        [SerializeField, Tooltip("レーザーの発射基準点名")]
        private string m_ShotTransformName;
        public string ShotTransformName => m_ShotTransformName;

        [SerializeField, Tooltip("レベルごとの発射パラメータ")]
        private LevelIndividualParam[] m_LevelIndividualParams;
        public LevelIndividualParam[] LevelIndividualParams => m_LevelIndividualParams;
    }
}
