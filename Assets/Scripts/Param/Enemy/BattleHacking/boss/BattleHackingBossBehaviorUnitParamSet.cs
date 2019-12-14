#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのボスの攻撃やダウン時のパラメータの規定クラス。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Boss/BaseBehaviorUnit", fileName = "param.hacking_boss_base_behavior_unit.asset")]
public class BattleHackingBossBehaviorUnitParamSet : ScriptableObject
{
    [SerializeField]
    private string m_BehaviorClass;
    public string BehaviorClass => m_BehaviorClass;


    // ボスや攻撃に関わらず共通に使いそうなので、以下に書いておく。

    [SerializeField, Tooltip("ベジェ曲線の配列")]
    private Bezier1Point[] m_Bezier3Points;
    public Bezier1Point[] Bezier3Points => m_Bezier3Points;

    [SerializeField, Tooltip("ベジェのループの開始が第何形態目からか")]
    private int m_LoopBeginPhase;
    public int LoopBeginPhase => m_LoopBeginPhase;

    [Header("Shot Param")]

    [SerializeField, Tooltip("アセット用単位弾幕パラメータの配列")]
    private AllUDFieldArray m_AllUDFieldArray;
    public AllUDFieldArray AllUDFieldArray => m_AllUDFieldArray;

    [SerializeField, Tooltip("弾幕の抽象クラスの配列")]
    private DanmakuCountAbstract2[] m_DanmakuCountAbstractArray;
    public DanmakuCountAbstract2[] DanmakuCountAbstractArray => m_DanmakuCountAbstractArray;
}
