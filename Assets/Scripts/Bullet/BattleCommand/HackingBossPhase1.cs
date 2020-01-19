using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// あるハッキングのボスのある形態を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/HackingBossPhase", fileName = "HackingBossPhase", order = 0)]
[System.Serializable]
public class HackingBossPhase1 : ScriptableObject
{

    [SerializeField, Tooltip("状態")]
    private HackingBossPhaseState1 m_HackingBossPhaseState;

    [SerializeField, Tooltip("弾幕")]
    private Danmaku1 m_DanmakuArray;


    public void OnStarts()
    {
        m_HackingBossPhaseState.OnStarts();
        m_DanmakuArray.OnStarts();
    }


    public TransformSimple OnUpdates(BattleHackingBossBehavior boss)
    {
        TransformSimple transform = m_HackingBossPhaseState.OnUpdates();
        m_DanmakuArray.OnUpdates(boss, m_HackingBossPhaseState);
        return transform;
    }
}
