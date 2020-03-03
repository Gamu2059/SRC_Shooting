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

        public AnimationCurve AnimationValue;

        public float EndValue;
    }

    [Serializable]
    public struct BattleAnimationVectorParam
    {
        public FloatParam XParam;

        public FloatParam YParam;

        public FloatParam ZParam;
    }

    public bool UsePosition;

    public BattleAnimationVectorParam Position;

    [Space()]

    public bool UseRotation;

    public BattleAnimationVectorParam Rotation;
}
