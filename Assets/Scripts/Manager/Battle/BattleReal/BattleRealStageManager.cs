using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードの画面のオブジェクトを保持するためのマネージャ。
/// </summary>
public class BattleRealStageManager : ControllableMonoBehavior
{
    public enum E_HOLDER_TYPE
    {
        STAGE_OBJECT,
        PLAYER,
        ENEMY_GROUP,
        ENEMY,
        BULLET,
        ITEM,
    }

    public const string STAGE_OBJECT_HOLDER = "[StageObjectHolder]";
    public const string PLAYER_HOLDER = "[PlayerCharaHolder]";
    public const string ENEMY_GROUP_HOLDER = "[EnemyGroupHolder]";
    public const string ENEMY_HOLDER = "[EnemyCharaHolder]";
    public const string BULLET_HOLDER = "[BulletHolder]";
    public const string ITEM_HOLDER = "[ItemHolder]";

    #region Inspector

    [Header("Holder")]

	/// <summary>
	/// ステージ上の背景オブジェクトを保持するためのホルダー
	/// </summary>
	[SerializeField]
	private Transform m_StageObjectHolder;

	/// <summary>
	/// プレイヤーキャラを保持するためのホルダー
	/// </summary>
	[SerializeField]
	private Transform m_PlayerCharaHolder;

    /// <summary>
    /// 敵グループを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private Transform m_EnemyGroupHolder;

	/// <summary>
	/// 敵キャラを退避するためのホルダー
	/// </summary>
	[SerializeField]
	private Transform m_EnemyCharaHolder;

	/// <summary>
	/// 弾を保持するためのホルダー
	/// </summary>
	[SerializeField]
	private Transform m_BulletHolder;

    /// <summary>
    /// アイテムを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private Transform m_ItemHolder;

    [Header("Filed")]

    [SerializeField]
    private Vector2 m_MinLocalFieldPosition;
    public Vector2 MinLocalFieldPosition => m_MinLocalFieldPosition;

    [SerializeField]
    private Vector2 m_MaxLocalFieldPosition;
    public Vector2 MaxLocalFieldPosition => m_MaxLocalFieldPosition;

    #endregion

    public static BattleRealStageManager Instance => BattleManager.Instance.BattleRealStageManager;

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

    public Transform GetHolder(E_HOLDER_TYPE holderType)
    {
        Transform holder = null;
        string holderName = null;

        switch(holderType)
        {
            case E_HOLDER_TYPE.STAGE_OBJECT:
                holder = m_StageObjectHolder;
                holderName = STAGE_OBJECT_HOLDER;
                break;
            case E_HOLDER_TYPE.PLAYER:
                holder = m_PlayerCharaHolder;
                holderName = PLAYER_HOLDER;
                break;
            case E_HOLDER_TYPE.ENEMY_GROUP:
                holder = m_EnemyGroupHolder;
                holderName = ENEMY_GROUP_HOLDER;
                break;
            case E_HOLDER_TYPE.ENEMY:
                holder = m_EnemyCharaHolder;
                holderName = ENEMY_HOLDER;
                break;
            case E_HOLDER_TYPE.BULLET:
                holder = m_BulletHolder;
                holderName = BULLET_HOLDER;
                break;
            case E_HOLDER_TYPE.ITEM:
                holder = m_ItemHolder;
                holderName = ITEM_HOLDER;
                break;
        }

        if (holder != null)
        {
            return holder;
        }

        holder = new GameObject(holderName).transform;
        holder.SetParent(transform);
        holder.position = Vector3.zero;
        return holder;
    }
}
