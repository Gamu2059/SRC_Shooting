﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルにおけるオブジェクトの基底クラス。
/// </summary>
[RequireComponent(typeof(BattleObjectCollider))]
public abstract class BattleObjectBase : ControllableMonoBehavior, IColliderBase, IColliderProcess
{
    /// <summary>
    /// 衝突情報コンポーネント。
    /// </summary>
    private BattleObjectCollider m_Collider;

    /// <summary>
    /// キャッシュしている衝突情報。
    /// </summary>
    private ColliderData[] m_ColliderDatas;

    /// <summary>
    /// 衝突情報コンポーネントを取得する。
    /// </summary>
    public BattleObjectCollider GetCollider()
    {
        return m_Collider;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (m_Collider == null)
        {
            m_Collider = GetComponent<BattleObjectCollider>();
        }
    }

    public override void OnUpdate()
    {
        //base.OnUpdate();
    }

    #endregion

    /// <summary>
    /// 衝突フラグをクリアする。
    /// </summary>
    public abstract void ClearColliderFlag();

    /// <summary>
    /// 衝突情報を更新する。
    /// </summary>
    public void UpdateCollider()
    {
        m_ColliderDatas = m_Collider.CreateColliderData();
    }

    /// <summary>
    /// 実際の衝突処理を行う。
    /// </summary>
    public abstract void ProcessCollision();

    /// <summary>
    /// 衝突情報を取得する。
    /// </summary>
    public ColliderData[] GetColliderData()
    {
        return m_ColliderDatas;
    }
}

