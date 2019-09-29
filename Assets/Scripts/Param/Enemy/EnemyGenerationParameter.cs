using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerationParameter : ScriptableObject
{
    [SerializeField]
    private int m_HP;

    // ドロップアイテム

    // 撃破時イベント

    [SerializeField]
    private EnemyBehaviorParameter m_EnemyBehaviorParameter;
}