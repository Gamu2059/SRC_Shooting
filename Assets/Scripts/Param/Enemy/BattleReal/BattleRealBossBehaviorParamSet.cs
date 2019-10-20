using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスの攻撃やダウン時のパラメータの規定クラス。
/// このクラスを継承してパラメータのセットを定義して下さい。
/// </summary>
public class BattleRealBossBehaviorParamSet : ScriptableObject
{
    [SerializeField]
    private string m_BehaviorClass;
    public string BehaviorClass => m_BehaviorClass;
}
