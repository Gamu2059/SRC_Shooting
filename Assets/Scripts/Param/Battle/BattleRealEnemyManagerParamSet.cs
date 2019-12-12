#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのEnemyManagerのパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/ParamSet/Manager/BattleRealEnemy", fileName = "param.battle_real_enemy.asset")]
public class BattleRealEnemyManagerParamSet : ScriptableObject
{
    [SerializeField, Tooltip("左下のオフセットフィールド")]
    private Vector2 m_MinOffsetFieldPosition;
    public Vector2 MinOffsetFieldPosition => m_MinOffsetFieldPosition;

    [SerializeField, Tooltip("右上のオフセットフィールド")]
    private Vector2 m_MaxOffsetFieldPosition;
    public Vector2 MaxOffsetFieldPosition => m_MaxOffsetFieldPosition;

    [Header("SE"), Tooltip("あとでそれぞれの敵パラメータで指定するように変更する")]

    [SerializeField]
    private PlaySoundParam m_Shot01Se;
    public PlaySoundParam Shot01Se => m_Shot01Se;

    [SerializeField]
    private PlaySoundParam m_Shot02Se;
    public PlaySoundParam Shot02Se => m_Shot02Se;

    [SerializeField]
    private PlaySoundParam m_DamageSe;
    public PlaySoundParam DamageSe => m_DamageSe;

    [SerializeField]
    private PlaySoundParam m_BreakSe;
    public PlaySoundParam BreakSe => m_BreakSe;

    [SerializeField]
    private PlaySoundParam m_DownSe;
    public PlaySoundParam DownSe => m_DownSe;
}