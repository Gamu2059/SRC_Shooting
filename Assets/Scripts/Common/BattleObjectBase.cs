using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルにおけるオブジェクトの基底クラス。
/// </summary>
[RequireComponent(typeof(BattleObjectCollider))]
public class BattleObjectBase : ControllableMonoBehaviour, IColliderBase
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

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (m_Collider == null) {
            m_Collider = GetComponent<BattleObjectCollider>();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    /// <summary>
	/// 衝突情報を取得する。
	/// </summary>
	public ColliderData[] GetColliderData()
    {
        return m_ColliderDatas;
    }

    public void UpdateColliderData()
    {
        m_ColliderDatas = m_Collider.CreateColliderData();
    }
}

