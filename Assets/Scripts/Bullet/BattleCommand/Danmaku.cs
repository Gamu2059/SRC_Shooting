#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある1つの弾幕を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/Danmaku", fileName = "Danmaku", order = 0)]
[System.Serializable]
public class Danmaku : ScriptableObject
{
    [SerializeField, Tooltip("単位弾幕の配列")]
    private UnitDanmaku[] m_UnitDanmakuArray;


    public void OnStarts()
    {
        foreach (UnitDanmaku unitDanmaku in m_UnitDanmakuArray)
        {
            unitDanmaku.OnStarts();
        }
    }


    public void OnUpdates(
        BattleHackingBossBehavior boss,
        CommonOperationVariable commonOperationVariable
        )
    {
        foreach (UnitDanmaku unitDanmaku in m_UnitDanmakuArray)
        {
            unitDanmaku.OnUpdates(
                boss,
                commonOperationVariable
                );
        }
    }
}
