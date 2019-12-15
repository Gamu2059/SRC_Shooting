#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 時間軸を持つエフェクト指定パラメータ。
/// </summary>
[Serializable]
public class SequentialEffectParamSet
{
    [Serializable]
    public struct SequentialEffectParam
    {
        public float Delay;
        public EffectParamSet Effect;
    }

    [SerializeField]
    private SequentialEffectParam[] m_Params;
    public SequentialEffectParam[] Params => m_Params;
}
