﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングモードの画面のオブジェクトを保持するためのマネージャ。
/// </summary>
public class BattleHackingStageManager : SingletonMonoBehavior<BattleHackingStageManager>
{
    public enum E_HOLDER_TYPE
    {
        STAGE_OBJECT,
        PLAYER,
        ENEMY,
        BULLET,
        EFFECT,
    }

    public const string STAGE_OBJECT_HOLDER = "[StageObjectHolder]";
    public const string PLAYER_HOLDER = "[PlayerCharaHolder]";
    public const string ENEMY_HOLDER = "[EnemyCharaHolder]";
    public const string BULLET_HOLDER = "[BulletHolder]";
    public const string EFFECT_HOLDER = "[EffectHolder]";

    #region Inspector

    [Header("Holder")]

    /// <summary>
    /// ステージ上の背景オブジェクトを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private Transform m_StageObjectHolder = default;
    public Transform StageObjectHolder => m_StageObjectHolder;

    /// <summary>
    /// プレイヤーキャラを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private Transform m_PlayerCharaHolder = default;
    public Transform PlayerCharaHolder => m_PlayerCharaHolder;

    /// <summary>
    /// 敵キャラを保持するためのホルダー
    /// </summary>
    [SerializeField]
    private Transform m_EnemyCharaHolder = default;
    public Transform EnemyCharaHolder => m_EnemyCharaHolder;

    /// <summary>
    /// 弾を保持するためのホルダー
    /// </summary>
    [SerializeField]
    private Transform m_BulletHolder = default;
    public Transform BulletHolder => m_BulletHolder;

    [SerializeField]
    private Transform m_EffectHolder = default;
    public Transform EffectHolder => m_EffectHolder;

    [Header("Field")]

    [SerializeField]
    private Vector2 m_MinLocalFieldPosition = default;
    public Vector2 MinLocalFieldPosition => m_MinLocalFieldPosition;

    [SerializeField]
    private Vector2 m_MaxLocalFieldPosition = default;
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

    /// <summary>
    /// 指定した座標から、このマネージャが疑似的に表現するビューポート座標へと変換する。
    /// フロントオブジェクト専用。
    /// </summary>
    public Vector2 CalcViewportPosFromWorldPosition(float x, float z, bool isScaleWithLocalField)
    {
        var min = MinLocalFieldPosition;
        var max = MaxLocalFieldPosition;
        var vX = MathUtility.CalcRate(min.x, max.x, x) - 0.5f;
        var vZ = MathUtility.CalcRate(min.y, max.y, z) - 0.5f;

        if (isScaleWithLocalField)
        {
            vX *= (max.x - min.x) / 2;
            vZ *= (max.y - min.y) / 2;
        }

        return new Vector2(vX, vZ);
    }

    /// <summary>
    /// このマネージャが疑似的に表現するビューポート座標へと変換する。
    /// フロントオブジェクト専用。
    /// </summary>
    public Vector2 CalcViewportPosFromWorldPosition(Transform t, bool isScaleWithLocalField)
    {
        if (t == null)
        {
            return new Vector2(0.5f, 0.5f);
        }

        return CalcViewportPosFromWorldPosition(t.position.x, t.position.z, isScaleWithLocalField);
    }

    public Transform GetHolder(E_HOLDER_TYPE holderType)
    {
        Transform holder = null;
        string holderName = null;

        switch (holderType)
        {
            case E_HOLDER_TYPE.STAGE_OBJECT:
                holder = StageObjectHolder;
                holderName = STAGE_OBJECT_HOLDER;
                break;
            case E_HOLDER_TYPE.PLAYER:
                holder = PlayerCharaHolder;
                holderName = PLAYER_HOLDER;
                break;
            case E_HOLDER_TYPE.ENEMY:
                holder = EnemyCharaHolder;
                holderName = ENEMY_HOLDER;
                break;
            case E_HOLDER_TYPE.BULLET:
                holder = BulletHolder;
                holderName = BULLET_HOLDER;
                break;
            case E_HOLDER_TYPE.EFFECT:
                holder = EffectHolder;
                holderName = EFFECT_HOLDER;
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
