using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// BattleAnimationトラックで用いるデータ。
/// </summary>
[Serializable]
public struct BattleAnimationParam
{
    [Serializable]
    public struct FloatParam
    {
        public bool Use;

        public bool IsNormalized;

        public E_RELATIVE RelativeType;

        public AnimationCurve AnimationValue;

        public float EndValue;
    }

    [Serializable]
    public struct BattleAnimationVectorParam
    {
        [Tooltip("ワールド空間かローカル空間か ただし、Scaleは常にローカル")]
        public Space SpaceType;

        public FloatParam XParam;

        public FloatParam YParam;

        public FloatParam ZParam;
    }

    public bool UsePosition;

    public BattleAnimationVectorParam Position;

    public bool UseRotation;

    public BattleAnimationVectorParam Rotation;

    public bool UseScale;

    public BattleAnimationVectorParam Scale;
}
