#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 振る舞いの継続時間による割り込み終了オプション。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Interrupt/BehaviorTime", fileName = "time.behavior_interrupt.asset")]
public class BattleRealEnemyBehaviorTimeEndFunc : BattleRealEnemyBehaviorInterruptEndFunc
{
    [SerializeField, Tooltip("割り込みで終了する、この振る舞いの経過時間")]
    private float m_EndElapsedTime;

    public override bool IsInterruptEnd(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorUnit unit)
    {
        return unit.CurrentTime >= m_EndElapsedTime;
    }
}
