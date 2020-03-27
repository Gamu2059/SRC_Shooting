using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// BattleAnimationトラックで用いるデータ。
/// </summary>
[Serializable]
public class BattleAnimationParam
{
    [Serializable]
    public class FloatParam
    {
        public bool Use = false;

        public AnimationCurve AnimationValue;

        public float EndValue = 0;

        public float Scale = 1;
    }

    [Serializable]
    public class BattleAnimationVectorParam
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
