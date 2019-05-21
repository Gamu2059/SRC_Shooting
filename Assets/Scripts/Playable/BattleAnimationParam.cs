using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleAnimationトラックで用いるデータ。
/// </summary>
[System.Serializable]
public struct BattleAnimationParam
{
    public const int WORLD_ANIMATION_INDEX = (int)E_ANIMATION_TYPE.ROTATION_Z;

    public enum E_ANIMATION_TYPE
    {
        POSITION_X,
        POSITION_Y,
        POSITION_Z,
        ROTATION_X,
        ROTATION_Y,
        ROTATION_Z,
        L_POSITION_X,
        L_POSITION_Y,
        L_POSITION_Z,
        L_ROTATION_X,
        L_ROTATION_Y,
        L_ROTATION_Z,
        L_SCALE_X,
        L_SCALE_Y,
        L_SCALE_Z,
    }

    [Tooltip("アニメーションのタイプ")]
    public E_ANIMATION_TYPE AnimationType;

    [Tooltip("アニメーションが絶対値か相対値か")]
    public E_RELATIVE RelativeType;

    [Tooltip("アニメーション")]
    public AnimationCurve Animation;

    [Tooltip("終了値を適用するかどうか")]
    public bool UseEndValue;

    [Tooltip("終了値")]
    public float EndValue;

    [Tooltip("")]
    public float EndValueLerp;
}
