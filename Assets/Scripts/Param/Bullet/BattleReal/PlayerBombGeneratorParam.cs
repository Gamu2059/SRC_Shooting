using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// プレイヤーボムジェネレータのパラメータ。
    /// </summary>
    [Serializable]
    public class PlayerBombGeneratorParam : IBattleRealBulletGeneratorParamBase
    {
        [Serializable]
        public class LevelIndividualParam
        {
            [SerializeField, Tooltip("ボムの発射回数")]
            private int m_ShotNum;
            public int ShotNum => m_ShotNum;

            [SerializeField, Tooltip("ボムの発射間隔")]
            private float m_ShotInterval;
            public float ShotInterval => m_ShotInterval;
        }

        [SerializeField]
        private string m_ShotTransformName;
        public string ShotTransformName => m_ShotTransformName;

        [SerializeField, Tooltip("ボムのプレハブ")]
        private BattleRealPlayerBomb m_Bomb;
        public BattleRealPlayerBomb Bomb => m_Bomb;

        [SerializeField, Tooltip("ボムの弾パラメータ")]
        private BulletParam m_BombParam;
        public BulletParam BombParam => m_BombParam;

        [SerializeField]
        private LevelIndividualParam[] m_LevelIndividualParams;
        public LevelIndividualParam[] LevelIndividualParams => m_LevelIndividualParams;
    }
}
