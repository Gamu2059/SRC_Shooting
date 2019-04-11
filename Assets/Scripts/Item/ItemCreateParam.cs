using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// アイテムの生成パラメータ
/// </summary>
[Serializable]
public struct ItemCreateParam
{
    /// <summary>
    /// アイテムの拡散パラメータ
    /// </summary>
    [Serializable]
    public struct ItemSpreadParam
    {
        /// <summary>
        /// この生成情報で生成するアイテムの種類
        /// </summary>
        public E_ITEM_TYPE ItemType;

        /// <summary>
        /// アイテムの生成個数
        /// </summary>
        public int ItemNum;

        /// <summary>
        /// アイテムの生成半径
        /// </summary>
        public float SpreadRadius;
    }

    /// <summary>
    /// アイテムの生成情報群
    /// </summary>
    public ItemSpreadParam[] ItemSpreadParams;

    /// <summary>
    /// 指定した座標から生成されるアイテムの種類
    /// </summary>
    public E_ITEM_TYPE CenterCreateItemType;
}
