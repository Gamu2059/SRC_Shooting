﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードのEnemyManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleHackingEnemy", fileName = "param.battle_hacking_enemy.asset")]
public class BattleHackingEnemyManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("左下のオフセットフィールド")]
    private Vector2 m_MinOffsetFieldPosition = default;
    public Vector2 MinOffsetFieldPosition => m_MinOffsetFieldPosition;

    [SerializeField, Tooltip("右上のオフセットフィールド")]
    private Vector2 m_MaxOffsetFieldPosition = default;
    public Vector2 MaxOffsetFieldPosition => m_MaxOffsetFieldPosition;

    [Header("SE"), Tooltip("あとでそれぞれの敵パラメータで指定するようにする")]

    [SerializeField]
    private PlaySoundParam m_SmallShot01Se;
    public PlaySoundParam SmallShot01Se => m_SmallShot01Se;

    [SerializeField]
    private PlaySoundParam m_MediumShot01Se;
    public PlaySoundParam MediumShot01Se => m_MediumShot01Se;

    [SerializeField]
    private PlaySoundParam m_MediumShot02Se;
    public PlaySoundParam MediumShot02Se => m_MediumShot02Se;

    [SerializeField]
    private PlaySoundParam m_DamageSe;
    public PlaySoundParam DamageSe => m_DamageSe;

    [SerializeField]
    private PlaySoundParam m_BreakSe;
    public PlaySoundParam BreakSe => m_BreakSe;
}
