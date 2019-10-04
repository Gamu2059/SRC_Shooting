using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードの画面のオブジェクトを保持するためのマネージャ。
/// </summary>
public class BattleRealStageManager : ControllableMonoBehavior
{
    #region Inspector

    [Header("Holder")]

	/// <summary>
	/// ステージ上の背景オブジェクトを保持するためのホルダー
	/// </summary>
	[SerializeField]
	private Transform m_StageObjectHolder;
    public Transform StageObjectHolder => m_StageObjectHolder;

	/// <summary>
	/// プレイヤーキャラを保持するためのホルダー
	/// </summary>
	[SerializeField]
	private Transform m_PlayerCharaHolder;
    public Transform PlayerCharaHolder => m_PlayerCharaHolder; 

	/// <summary>
	/// 敵キャラを保持するためのホルダー
	/// </summary>
	[SerializeField]
	private Transform m_EnemyCharaHolder;
    public Transform EnemyCharaHolder => m_EnemyCharaHolder;

	/// <summary>
	/// 弾を保持するためのホルダー
	/// </summary>
	[SerializeField]
	private Transform m_BulletHolder;
    public Transform BulletHolder => m_BulletHolder;

    /// <summary>
    /// アイテムを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private Transform m_ItemHolder;
    public Transform ItemHolder => m_ItemHolder;

    [Header("Filed")]

    [SerializeField]
    private Vector2 m_MinLocalFieldPosition;
    public Vector2 MinLocalFieldPosition => m_MinLocalFieldPosition;

    [SerializeField]
    private Vector2 m_MaxLocalFieldPosition;
    public Vector2 MaxLocalFieldPosition => m_MaxLocalFieldPosition;

    #endregion

    /// <summary>
    /// 指定したオブジェクトの座標が、フィールド領域の外にあるかどうかを判定する。
    /// </summary>
    public bool IsOutOfField(Transform obj)
    {
        var localPos = obj.localPosition;
        return
            localPos.x < m_MinLocalFieldPosition.x ||
            localPos.x > m_MaxLocalFieldPosition.x ||
            localPos.z < m_MinLocalFieldPosition.y ||
            localPos.z > m_MaxLocalFieldPosition.y;
    }

    /// <summary>
    /// 指定したオブジェクトの座標を動体オブジェクトホルダーのフィールド領域に入れ込む。
    /// </summary>
    public void ClampMovingObjectPosition(Transform obj)
    {
        if (!IsOutOfField(obj))
        {
            return;
        }

        var pos = obj.localPosition;
        pos.x = Mathf.Clamp(pos.x, m_MinLocalFieldPosition.x, m_MaxLocalFieldPosition.x);
        pos.z = Mathf.Clamp(pos.z, m_MinLocalFieldPosition.y, m_MaxLocalFieldPosition.y);
        obj.localPosition = pos;
    }
}
