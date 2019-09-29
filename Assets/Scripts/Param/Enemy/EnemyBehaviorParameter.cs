using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorParameter : ScriptableObject
{
    [SerializeField]
    private Dictionary<string, GameObject> m_UseBulletPrefab;

    [SerializeField]
    private EnemyLookParameter m_EnemyLookParameter;
}