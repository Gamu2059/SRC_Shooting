using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのBulletManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleHackingBullet", fileName = "param.battle_hacking_bullet.asset")]
public class BattleHackingBulletManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("左下のオフセットフィールド")]
    private Vector2 m_MinOffsetFieldPosition = default;
    public Vector2 MinOffsetFieldPosition => m_MinOffsetFieldPosition;

    [SerializeField, Tooltip("右上のオフセットフィールド")]
    private Vector2 m_MaxOffsetFieldPosition = default;
    public Vector2 MaxOffsetFieldPosition => m_MaxOffsetFieldPosition;

    [SerializeField, Tooltip("弾削除エフェクト")]
    private EffectParamSet m_BulletDestroyEffectParam;
    public EffectParamSet BulletDestroyEffectParam => m_BulletDestroyEffectParam;
}
