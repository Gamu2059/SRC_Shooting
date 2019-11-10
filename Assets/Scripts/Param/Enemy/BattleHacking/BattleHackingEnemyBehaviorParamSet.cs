#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードの敵の振る舞いパラメータの規定クラス。
/// このクラスを継承してパラメータのセットを定義して下さい。
/// </summary>
public abstract class BattleHackingEnemyBehaviorParamSet : ScriptableObject
{
    [SerializeField]
    private string m_BehaviorClass;
    public string BehaviorClass => m_BehaviorClass;

    [SerializeField]
    private CommandBulletSetParam m_BulletSetParam;
    public CommandBulletSetParam BulletSetParam => m_BulletSetParam;

    [SerializeField]
    private BattleHackingEnemyLookParamSet m_EnemyLookParamSet;
    public BattleHackingEnemyLookParamSet EnemyLookParamSet => m_EnemyLookParamSet;
}
