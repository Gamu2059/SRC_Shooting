using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_COLLIDER_TYPE
{
    DEFAULT,

    /// <summary>
    /// 被弾するコライダー
    /// </summary>
    CRITICAL,

    /// <summary>
    /// アイテムを実際に回収することができるコライダー
    /// </summary>
    ITEM_GAIN,

    /// <summary>
    /// アイテムを吸引することができるコライダー
    /// </summary>
    ITEM_ATTRACT,

    /// <summary>
    /// プレイヤーのレーザー
    /// </summary>
    PLAYER_LASER,

    /// <summary>
    /// プレイヤーのハッキング可能コライダー。ENEMY_HACKINGと接触することでハッキング開始となる。
    /// </summary>
    PLAYER_HACKING,

    /// <summary>
    /// プレイヤーの通常弾
    /// </summary>
    PLAYER_BULLET,

    /// <summary>
    /// プレイヤーのボム
    /// </summary>
    PLAYER_BOMB,

    /// <summary>
    /// 敵の通常弾
    /// </summary>
    ENEMY_BULLET,

    /// <summary>
    /// 敵のレーザー
    /// </summary>
    ENEMY_LASER,

    /// <summary>
    /// 敵のボム
    /// </summary>
    ENEMY_BOMB,

    /// <summary>
    /// 敵のハッキング可能コライダー。PLAYER_HACKINGと接触することでハッキング開始となる。
    /// </summary>
    ENEMY_HACKING,

    /// <summary>
    /// 敵本体。プレイヤーがこれに当たるとプレイヤーが死亡する。
    /// </summary>
    ENEMY_MAIN_BODY,

    /// <summary>
    /// 敵の本体以外のオブジェクトのコライダー。プレイヤーがこれに当たるとプレイヤーが死亡する。
    /// </summary>
    ENEMY_SUB_BODY,
}
