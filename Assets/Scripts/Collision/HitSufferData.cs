using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 衝突情報をまとめるクラス。
/// </summary>
public class HitSufferData<T> where T : BattleObjectBase
{
    /// <summary>
    /// 相手方のオブジェクト
    /// </summary>
    public T OpponentObject { get; private set; }

    /// <summary>
    /// 当てた側の衝突情報
    /// </summary>
    public ColliderData HitCollider { get; private set; }

    /// <summary>
    /// 当てられた側の衝突情報
    /// </summary>
    public ColliderData SufferCollider { get; private set; }

    /// <summary>
    /// 連続で当てられた回数
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// 衝突判定の更新フラグ
    /// </summary>
    public bool IsUpdateFlag;

    /// <summary>
    /// 衝突座標リスト
    /// </summary>
    public List<Vector2> Positions;

    public HitSufferData(T opponentObj, ColliderData hitCollider, ColliderData sufferCollider)
    {
        OpponentObject = opponentObj;
        HitCollider = hitCollider;
        SufferCollider = sufferCollider;
    }

    public void CountUp()
    {
        Count++;
    }
}
