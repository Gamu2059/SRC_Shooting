using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum E_COLLISION_TYPE
{
    RECT,
    CIRCLE
}

public class EnemyLookParameter : ScriptableObject
{
    class CollisionData
    {
        string CollisionName;
        E_COLLISION_TYPE CollisionType;
        Transform CollisionParameter;
    }

    [SerializeField]
    private GameObject m_EnemyPrefab;

    [SerializeField]
    private string m_EnemyID;

    [SerializeField]
    private List<CollisionData> m_CollisionParameter;
}