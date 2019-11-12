#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761のダウン行動パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/INF-C-761/Down", fileName = "param.inf_c_761_down.asset")]
public class InfC761DownParamSet : BattleRealBossBehaviorParamSet
{
    [SerializeField]
    private float m_DownTime;
    public float DownTime => m_DownTime;

    [SerializeField]
    private float m_Amplitude;
    public float Amplitude => m_Amplitude;
}
