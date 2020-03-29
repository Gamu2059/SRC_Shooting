#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// 座標指定弾ジェネレータのパラメータ。
    /// </summary>
    [Serializable]
    public class PointTweenBulletGeneratorParam : IBattleRealBulletGeneratorParamBase
    {
        [SerializeField, Tooltip("Nway弾として発射する弾のプレハブ配列 この並び順で発射する弾のサイクルをループさせる")]
        private BulletController[] m_Bullets;
        public BulletController[] Bullets => m_Bullets;

        [SerializeField, Tooltip("発射する弾に渡すパラメータ この並び順で発射する弾に渡すパラメータのサイクルをループさせる")]
        private BulletParam[] m_BulletParams;
        public BulletParam[] BulletParams => m_BulletParams;

        [Space()]

        [SerializeField, Tooltip("発射開始ワールド座標")]
        private Vector3 m_BeginPoint;
        public Vector3 BeginPoint => m_BeginPoint;

        [SerializeField, Tooltip("発射終了ワールド座標")]
        private Vector3 m_EndPoint;
        public Vector3 EndPoint => m_EndPoint;

        [SerializeField, Tooltip("発射する弾の数")]
        private int m_ShotNum;
        public int ShotNum => m_ShotNum;

        [Header("Shot Time")]

        [SerializeField, Tooltip("生成インスタンスが出来てから最初の弾を発射するまでの時間差")]
        private float m_ShotOffsetTime;
        public float ShotOffsetTime => m_ShotOffsetTime;

        [SerializeField, Tooltip("2回目以降の弾の発射をどれくらいの間隔で行うのか 0の場合、同時に発射される"), Min(0)]
        private float m_ShotInterval;
        public float ShotInterval => m_ShotInterval;

        [SerializeField, Tooltip("2回目以降の弾の発射の際にShotIntervalに加算される時間間隔 負数を指定すると早くなっていく")]
        private float m_ShotDeltaInterval;
        public float ShotDeltaInterval => m_ShotDeltaInterval;

        [SerializeField, Tooltip("2回目以降の弾の発射の際にShotDeltaIntevalに加算される時間間隔")]
        private float m_ShotDeltaDeltaInterval;
        public float ShotDeltaDeltaInterval => m_ShotDeltaDeltaInterval;

        [Header("Shot Angle")]

        [SerializeField, Tooltip("最初に発射する弾の角度。0度は上、90度は右を向く。(度数法)")]
        private float m_ShotAngle;
        public float ShotAngle => m_ShotAngle;

        [SerializeField, Tooltip("2回目以降の弾の発射の際にShotAngleに加算される角度間隔(度数法)")]
        private float m_ShotDeltaAngle;
        public float ShotDeltaAngle => m_ShotDeltaAngle;

        [SerializeField, Tooltip("2回目以降の弾の発射の際にShotDeltaAngleに加算される角度間隔(度数法)")]
        private float m_ShotDeltaDeltaAngle;
        public float ShotDeltaDeltaAngle => m_ShotDeltaDeltaAngle;

        [Header("Shot Effect")]

        [SerializeField, Tooltip("弾を発射する前に表示するエフェクト")]
        private EffectParamSet m_ShotBeforeEffect;
        public EffectParamSet ShotBeforeEffect => m_ShotBeforeEffect;

        [SerializeField, Tooltip("弾発射前エフェクトを弾の発射の何秒「前」に出すか。ただし、生成インスタンスの生成より前には当然表示できない。"), Min(0)]
        private float m_AppearBeforeTimeShotBeforeEffect;
        public float AppearBeforeTimeShotBeforeEffect => m_AppearBeforeTimeShotBeforeEffect;

        [SerializeField, Tooltip("発射した箇所に生成するエフェクト")]
        private EffectParamSet m_ShotEffect;
        public EffectParamSet ShotEffect => m_ShotEffect;
    }
}
