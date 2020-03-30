#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 指定回数だけ繰り返すための処理機構
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Group/CountLoop", fileName = "count_loop.behavior_group.asset", order = 20)]
public class BattleRealCountLoopGroup : BattleRealEnemyBehaviorGroup
{
    #region Field Inspector

    [Header("Count Loop Parameter")]

    [SerializeField, Tooltip("「ループする」回数。 つまり、1を指定すると2回このグループの処理が実行されるということ。"), Min(0)]
    private int m_LoopCount = 0;

    #endregion

    #region Field

    private int m_CurrentCount;

    #endregion

    protected override void OnStart()
    {
        base.OnStart();
        m_CurrentCount = 0;
    }

    protected override void OnLooped()
    {
        base.OnLooped();
        m_CurrentCount++;
    }

    public override bool IsEndGroup()
    {
        return m_CurrentCount >= m_LoopCount;
    }
}
