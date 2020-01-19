using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある1つの弾幕を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/Danmaku", fileName = "Danmaku", order = 0)]
[System.Serializable]
public class Danmaku1 : ScriptableObject
{
    [SerializeField, Tooltip("単位弾幕の配列")]
    private UnitDanmaku1[] m_UnitDanmakuArray;


    public void OnStarts()
    {
        foreach (UnitDanmaku1 unitDanmaku in m_UnitDanmakuArray)
        {
            unitDanmaku.OnStarts();
        }
    }


    public void OnUpdates(BattleHackingBossBehavior boss, HackingBossPhaseState1 state)
    {
        foreach (UnitDanmaku1 unitDanmaku in m_UnitDanmakuArray)
        {
            unitDanmaku.OnUpdates(boss, state);
        }
    }
}
