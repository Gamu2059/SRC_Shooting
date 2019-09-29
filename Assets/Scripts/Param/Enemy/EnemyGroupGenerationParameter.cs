using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupGenerationParameter : ScriptableObject
{
    [SerializeField]
    private string m_Condition;

    [SerializeField]
    private Vector2 m_ViewPortPosition;

    [SerializeField]
    private Vector3 m_OffsetPositionFromViewPort;

    [SerializeField]
    private float m_GenerationAngle;
    
    [SerializeField]
    private EnemyGroupBehaviorParameter m_EnemyGroupBehaviorParameter;

    [SerializeField]
    private E_RELATIVE m_OnlyViewPortPositionRerative;

    [SerializeField]
    private Vector2 m_OnlyViewPortPosition;

    [SerializeField]
    private E_RELATIVE m_OnlyOffsetPositionRelative;

    [SerializeField]
    private Vector3 m_OnlyOffsetPositionFromViewPort;

    [SerializeField]
    private E_RELATIVE m_OnlyGenerationAngleRelative;

    [SerializeField]
    private EnemyGenerationParameter m_EnemyGenerationParameter;
}