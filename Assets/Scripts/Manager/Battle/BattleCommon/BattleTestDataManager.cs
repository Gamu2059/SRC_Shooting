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
