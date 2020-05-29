using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// プレイヤー通常弾ジェネレータのパラメータ。
    /// </summary>
    [Serializable]
    public class PlayerNormalBulletGeneratorParam : IBattleRealBulletGeneratorParamBase
    {
        [Serializable]
        public class LevelIndividualParam
        {
            [SerializeField, Tooltip("サイドショットを使用するかどうか")]
            private bool m_UseSideShot;
            public bool UseSideShot => m_UseSideShot;

            [SerializeField, Tooltip("メインショットの発射間隔")]
            private float m_MainShotInterval;
            public float MainShotInterval => m_MainShotInterval;

            [SerializeField, Tooltip("サイドショットの発射間隔")]
            private float m_SideShotInterval;
            public float SideShotInterval => m_SideShotInterval;
        }

        [SerializeField, Tooltip("メインショットの発射点名")]
        private string m_MainShotTransformName;
        public string MainShotTransformName => m_MainShotTransformName;

        [SerializeField, Tooltip("左のサイドショットの発射点名")]
        private string m_LeftSideShotTransformName;
        public string LeftSideShotTransformName => m_LeftSideShotTransformName;

        [SerializeField, Tooltip("右のサイドショットの発射点名")]
        private string m_RightSideShotTransformName;
        public string RightSideShotTransformName => m_RightSideShotTransformName;

        [SerializeField, Tooltip("メインショットの弾プレハブ")]
        private BattleRealPlayerMainBullet m_MainShotBullet;
        public BattleRealPlayerMainBullet MainShotBullet => m_MainShotBullet;

        [SerializeField, Tooltip("サイドショットの弾プレハブ")]
        private BattleRealPlayerSideBullet m_SideShotBullet;
        public BattleRealPlayerSideBullet SideShotBullet => m_SideShotBullet;

        [SerializeField, Tooltip("メインショットの弾パラメータ")]
        private BulletParam m_MainShotParam;
        public BulletParam MainShotParam => m_MainShotParam;

        [SerializeField, Tooltip("サイドショットの弾パラメータ")]
        private BulletParam m_SideShotParam;
        public BulletParam SideShotParam => m_SideShotParam;

        [SerializeField, Tooltip("レベルごとの発射パラメータ")]
        private LevelIndividualParam[] m_LevelIndividualParams;
        public LevelIndividualParam[] LevelIndividualParams => m_LevelIndividualParams;
    }
}
