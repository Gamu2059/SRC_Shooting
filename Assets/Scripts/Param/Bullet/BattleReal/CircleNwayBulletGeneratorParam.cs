#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// 円周上からNway弾を生成するジェネレータのパラメータ。<br/>
    /// 主に4wayパラスで使用する。
    /// </summary>
    [Serializable]
    public class CircleNwayBulletGeneratorParam : IBattleRealBulletGeneratorParamBase
    {
        [SerializeField, Tooltip("Nway弾として発射する弾のプレハブ配列 この並び順で発射する弾のサイクルをループさせる")]
        private BulletController[] m_Bullets;
        public BulletController[] Bullets => m_Bullets;

        [SerializeField, Tooltip("発射する弾に渡すパラメータ この並び順で発射する弾に渡すパラメータのサイクルをループさせる")]
        private BulletParam[] m_BulletParams;
        public BulletParam[] BulletParams => m_BulletParams;

        [Header("Shot General")]

        [SerializeField, Tooltip("発射の位置、向きの基準となるトランスフォームの名前")]
        private string m_ShotTransformName;
        public string ShotTransformName => m_ShotTransformName;

        [SerializeField, Tooltip("発射点の数"), Min(0)]
        private int m_ShotPointNum;
        public int ShotPointNum => m_ShotPointNum;

        [SerializeField, Tooltip("発射点の発射基準のトランスフォームに対する半径")]
        private float m_ShotPointRadius;
        public float ShotPointRadius => m_ShotPointRadius;

        [SerializeField, Tooltip("発射点の向きの基準とする発射基準のトランスフォームの正面方向に対するオフセット角度(度数法)")]
        private float m_ShotPointOffsetAngleFromTransform;
        public float ShotPointOffsetAngleFromTransform => m_ShotPointOffsetAngleFromTransform;

        [SerializeField, Tooltip("発射点の回転速度(度数法)")]
        private float m_ShotPointAngleSpeedFromTransform;
        public float ShotPointAngleSpeedFromTransform => m_ShotPointAngleSpeedFromTransform;

        [SerializeField, Tooltip("全体で最初の一度だけ生成するエフェクト")]
        private SequentialEffectParamSet m_ShotOverallStartEffect;
        public SequentialEffectParamSet ShotOverallStartEffect => m_ShotOverallStartEffect;

        [SerializeField, Tooltip("発射点に生成するエフェクト")]
        private EffectParamSet m_ShotEffect;
        public EffectParamSet ShotEffect => m_ShotEffect;

        [Header("Shot Time")]

        [SerializeField, Tooltip("発射点から何回発射するか"), Min(0)]
        private int m_ShotNum;
        public int ShotNum => m_ShotNum;

        [SerializeField, Tooltip("生成インスタンスが出来てから最初の弾を発射するまでの時間差"), Min(0)]
        private float m_ShotOffsetTime;
        public float ShotOffsetTime => m_ShotOffsetTime;

        [SerializeField, Tooltip("2回目以降の弾の発射をどれくらいの間隔で行うのか 0の場合、同時に発射される 分解能は0.02"), Min(0)]
        private float m_ShotInterval;
        public float ShotInterval => m_ShotInterval;
    }
}
