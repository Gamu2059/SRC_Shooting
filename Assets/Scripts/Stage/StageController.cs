using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ全体を制御するためのコントローラ。
/// </summary>
public class StageController : BattleControllableMonoBehavior
{
    [Header("Objects Holder")]

    [SerializeField]
    private GameObject m_ObjectsHolder;

    /// <summary>
    /// BattleMainが有効になった時に呼び出される。
    /// </summary>
    public override void OnEnableObject()
    {
        base.OnEnableObject();
        m_ObjectsHolder.SetActive(true);
    }

    /// <summary>
    /// BattleMainが無効になった時に呼び出される。
    /// </summary>
    public override void OnDisableObject()
    {
        base.OnDisableObject();
        m_ObjectsHolder.SetActive(false);
    }
}
