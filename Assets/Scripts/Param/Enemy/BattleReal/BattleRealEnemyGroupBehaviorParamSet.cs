#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードの敵グループの振る舞いパラメータの規定クラス。
/// このクラスを継承してパラメータのセットを定義して下さい。
/// </summary>
public abstract class BattleRealEnemyGroupBehaviorParamSet : ScriptableObject
{
    [SerializeField]
    private string m_BehaviorClass;
    public string BehaviorClass => m_BehaviorClass;
}