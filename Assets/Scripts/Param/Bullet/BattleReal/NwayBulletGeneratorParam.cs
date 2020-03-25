#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// Nway弾ジェネレータのパラメータ。
    /// </summary>
    [Serializable]
    public class NwayBulletGeneratorParam : IBattleRealBulletGeneratorParamBase
    {
        public enum E_AXIS_TYPE
        {
            /// <summary>
            /// トランスフォームからプレイヤーの方向を軸とする
            /// </summary>
            PLAYER_LOOK,

            /// <summary>
            /// トランスフォームのForwardを軸とする
            /// </summary>
            TRANSFORM_FORWARD,
        }

        [SerializeField, Tooltip("Nway弾として発射する弾のプレハブ配列 この並び順で発射する弾のサイクルをループさせる")]
        private BulletController[] m_Bullets;
        public BulletController[] Bullets => m_Bullets;

        [SerializeField, Tooltip("発射する弾に渡すパラメータ この並び順で発射する弾に渡すパラメータのサイクルをループさせる")]
        private BulletParam[] m_BulletParams;
        public BulletParam[] BulletParams => m_BulletParams;

        [SerializeField, Tooltip("このNway弾を発射するトランスフォームの名前")]
        private string m_ShotTransformName;
        public string ShotTransformName => m_ShotTransformName;

        [SerializeField, Tooltip("発射する数"), Min(0)]
        private int m_ShotNum;
        public int ShotNum => m_ShotNum;

        [SerializeField, Tooltip("Nwayの軸(中心の向き)のタイプ")]
        private E_AXIS_TYPE m_AxisType = E_AXIS_TYPE.PLAYER_LOOK;
        public E_AXIS_TYPE AxisType => m_AxisType;

        [SerializeField, Tooltip("Nwayの軸のオフセット角度(度数法)")]
        private float m_AxisOffsetAngle;
        public float AxisOffsetAngle => m_AxisOffsetAngle;

        [Header("Shot Radius")]

        [SerializeField, Tooltip("このNway弾を発射する時の半径"), Min(0)]
        private float m_ShotRadius;
        public float ShotRadius => m_ShotRadius;

        [SerializeField, Tooltip("2回目以降の弾の発射の際にShotRadiusに加算される半径 負数を指定すると狭まっていく")]
        private float m_ShotDeltaRadius;
        public float ShotDeltaRadius => m_ShotDeltaRadius;

        [SerializeField, Tooltip("2回目以降の弾の発射の際にShotDeltaRadiusに加算される半径")]
        private float m_ShotDeltaDeltaRadius;
        public float ShotDeltaDeltaRadius => m_ShotDeltaDeltaRadius;

        [Header("Shot Interval")]

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

        [SerializeField, Tooltip("弾と弾の間の角度間隔(度数法)"), Min(0)]
        private float m_ShotAngle;
        public float ShotAngle => m_ShotAngle;

        [SerializeField, Tooltip("2回目以降の弾の発射の際にShotAngleに加算される角度間隔(度数法)")]
        private float m_ShotDeltaAngle;
        public float ShotDeltaAngle => m_ShotDeltaAngle;

        [SerializeField, Tooltip("2回目以降の弾の発射の際にShotDeltaAngleに加算される角度間隔(度数法)")]
        private float m_ShotDeltaDeltaAngle;
        public float ShotDeltaDeltaAngle => m_ShotDeltaDeltaAngle;
    }
}
