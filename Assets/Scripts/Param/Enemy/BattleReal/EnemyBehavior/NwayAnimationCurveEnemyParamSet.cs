using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, Obsolete]
public class NwayAnimationCurveEnemyParamSet : AnimationCurveEnemyParamSet
{
    [Header("N way Param")]

    [SerializeField]
    private int m_Num = default;
    public int Num => m_Num;

    [SerializeField]
    private float m_Radius = default;
    public float Radius => m_Radius;

    [SerializeField]
    private float m_AngleOffset = default;
    public float AngleOffset => m_AngleOffset;
}
