#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵パラメータを難易度に応じてセットでまとめるための基底クラス。
/// </summary>
public abstract class BattleRealEnemyParamSetBase : ScriptableObject
{
    [Serializable]
    private enum E_ENEMY_CONTROLLER_TYPE
    {
        DEFAULT_ENEMY,
        DEFAULT_BOSS,
        OTHER,
    }

    [SerializeField, Tooltip("敵を制御するコントローラコンポーネントのタイプを指定する。")]
    private E_ENEMY_CONTROLLER_TYPE m_ControllerType;

    [SerializeField, Tooltip("コントローラタイプがOTHERの場合のコンポーネント名を指定する。")]
    private string m_ControllerClassName;

    /// <summary>
    /// 難易度に応じて取得できるパラメータを変えるもの。
    /// </summary>
    public abstract BattleRealEnemyParamBase GetEnemyParam(E_DIFFICULTY difficulty);

    public string GetControllerClassName()
    {
        switch(m_ControllerType)
        {
            case E_ENEMY_CONTROLLER_TYPE.DEFAULT_ENEMY:
                return typeof(BattleRealEnemyController).Name;
            case E_ENEMY_CONTROLLER_TYPE.DEFAULT_BOSS:
                return typeof(BattleRealBossController).Name;
            default:
                return m_ControllerClassName;
        }
    }
}
