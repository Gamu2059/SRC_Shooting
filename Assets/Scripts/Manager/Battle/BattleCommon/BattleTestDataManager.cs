#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エディタ専用のバトルシーンにテストデータを埋め込むためのマネージャ。
/// </summary>
public class BattleTestDataManager : SingletonMonoBehavior<BattleTestDataManager>
{
    #region Field Inspector

    [Header("Player")]

    [SerializeField]
    private bool m_IsNotPlayerDead;
    public bool IsNotPlayerDead => m_IsNotPlayerDead;

    [SerializeField]
    private bool m_IsNotGameOver;
    public bool IsNotGameOver => m_IsNotGameOver;

    [Header("Collider")]

    [SerializeField]
    private Material m_ColliderMaterial;
    public Material ColliderMaterial => m_ColliderMaterial;

    [SerializeField]
    private bool m_IsDrawColliderArea;
    public bool IsDrawColliderArea => m_IsDrawColliderArea;

    [SerializeField]
    private bool m_IsDrawOutSideColliderArea;
    public bool IsDrawOutSideColliderArea => m_IsDrawOutSideColliderArea;

    #endregion

    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();

#if !UNITY_EDITOR
        Destroy(this);
#endif
    }

    #endregion
}
