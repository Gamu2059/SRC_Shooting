using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の振る舞いパラメータの規定クラス。
/// このクラスを継承してパラメータのセットを定義して下さい。
/// </summary>
[Obsolete]
public abstract class BattleRealEnemyBehaviorParamSet : ScriptableObject
{
    [SerializeField]
    private string m_BehaviorClass = default;
    public string BehaviorClass => m_BehaviorClass;

    [SerializeField]
    private BulletSetParam m_BulletSetParam = default;
    public BulletSetParam BulletSetParam => m_BulletSetParam;

    [SerializeField]
    private BattleRealEnemyLookParamSet m_EnemyLookParamSet = default;
    public BattleRealEnemyLookParamSet EnemyLookParamSet => m_EnemyLookParamSet;
}