using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyBehavior/NwayAnimationCurve", fileName = "behavior.nway_animation_curve.asset")]

public class NwayAnimationCurveEnemyParamSet : AnimationCurveEnemyParamSet
{
    [Header("N way Param")]

    [SerializeField]
    private int m_Num;
    public int Num => m_Num;

    [SerializeField]
    private float m_Radius;
    public float Radius => m_Radius;

    [SerializeField]
    private float m_AngleOffset;
    public float AngleOffset => m_AngleOffset;
}
