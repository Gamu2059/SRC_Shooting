#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのBulletManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Manager/BattleHackingBullet", fileName = "param.battle_hacking_bullet.asset")]
public class BattleHackingBulletManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("左下のオフセットフィールド")]
    private Vector2 m_MinOffsetFieldPosition;
    public Vector2 MinOffsetFieldPosition => m_MinOffsetFieldPosition;

    [SerializeField, Tooltip("右上のオフセットフィールド")]
    private Vector2 m_MaxOffsetFieldPosition;
    public Vector2 MaxOffsetFieldPosition => m_MaxOffsetFieldPosition;

    [SerializeField, Tooltip("弾削除エフェクト")]
    private EffectParamSet m_BulletDestroyEffectParam;
    public EffectParamSet BulletDestroyEffectParam => m_BulletDestroyEffectParam;
}
