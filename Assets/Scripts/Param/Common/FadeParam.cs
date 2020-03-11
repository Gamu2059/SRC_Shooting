#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// FadeManagerに渡すパラメータ。
/// </summary>
[Serializable]
public struct FadeParam
{
    public bool IsFadeOut;
    public Color FadeOutColor;
    public bool UseAnimation;
    public float Duration;
    public AnimationCurve RateCurve;
}
