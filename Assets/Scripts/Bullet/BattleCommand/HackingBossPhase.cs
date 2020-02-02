using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// あるハッキングのボスのある形態を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/HackingBossPhase", fileName = "HackingBossPhase", order = 0)]
[System.Serializable]
public class HackingBossPhase : ScriptableObject
{

    [SerializeField, Tooltip("状態")]
    private HackingBossPhaseState m_HackingBossPhaseState;

    [SerializeField, Tooltip("弾幕")]
    private Danmaku m_DanmakuArray;

    //[SerializeField, Tooltip("開始からの経過時間")]
    private float m_Time;


    public void OnStarts()
    {
        m_Time = 0;

        m_HackingBossPhaseState.OnStarts();
        m_DanmakuArray.OnStarts();
    }


    public TransformSimple OnUpdates(BattleHackingBossBehavior boss)
    {
        m_Time += Time.deltaTime;

        TransformSimple transform = m_HackingBossPhaseState.OnUpdates();
        m_DanmakuArray.OnUpdates(boss, m_HackingBossPhaseState);
        return transform;
    }
}
