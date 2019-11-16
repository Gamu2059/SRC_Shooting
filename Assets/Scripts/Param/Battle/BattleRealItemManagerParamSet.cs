#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのItemManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleRealItem", fileName = "param.battle_real_item.asset")]
public class BattleRealItemManagerParamSet : ScriptableObject
{
    [Serializable]
    public struct ItemPrefabSet
    {
        [SerializeField]
        private E_ITEM_TYPE m_ItemType;
        public E_ITEM_TYPE ItemType => m_ItemType;

        [SerializeField]
        private BattleRealItemController m_ItemPrefab;
        public BattleRealItemController ItemPrefab => m_ItemPrefab;
    }

    [SerializeField, Tooltip("左下のオフセットフィールド")]
    private Vector2 m_MinOffsetFieldPosition;
    public Vector2 MinOffsetFieldPosition => m_MinOffsetFieldPosition;

    [SerializeField, Tooltip("右上のオフセットフィールド")]
    private Vector2 m_MaxOffsetFieldPosition;
    public Vector2 MaxOffsetFieldPosition => m_MaxOffsetFieldPosition;

    [SerializeField, Tooltip("アイテムの吸引速度")]
    private float m_AttractRate;
    public float AttractRate => m_AttractRate;

    [SerializeField, Tooltip("アイテムの軌道パラメータ")]
    private BulletOrbitalParam m_OrbitalParam;
    public BulletOrbitalParam OrbitalParam => m_OrbitalParam;

    [SerializeField, Tooltip("アイテムの種類")]
    private ItemPrefabSet[] m_ItemPrefabSets;
    public ItemPrefabSet[] ItemPrefabSets => m_ItemPrefabSets;
}
