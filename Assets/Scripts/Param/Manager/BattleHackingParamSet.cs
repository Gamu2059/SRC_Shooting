#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードで使用するパラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleHacking/Manager/BattleHacking", fileName = "param.battle_hacking.asset")]
public class BattleHackingParamSet : ScriptableObject
{
    [SerializeField]
    private BattleHackingPlayerManagerParamSet m_PlayerManagerParamSet;
    public BattleHackingPlayerManagerParamSet PlayerManagerParamSet => m_PlayerManagerParamSet;

    [SerializeField]
    private BattleHackingEnemyManagerParamSet m_EnemyManagerParamSet;
    public BattleHackingEnemyManagerParamSet EnemyManagerParamSet => m_EnemyManagerParamSet;

    [SerializeField]
    private BattleHackingBulletManagerParamSet m_BulletManagerParamSet;
    public BattleHackingBulletManagerParamSet BulletManagerParamSet => m_BulletManagerParamSet;

    [Header("カメラ シェイク")]

    [SerializeField]
    private CameraShakeParam m_DestroyBulletShakeParam;
    public CameraShakeParam DestroyBulletShakeParam => m_DestroyBulletShakeParam;

    [SerializeField]
    private CameraShakeParam m_DestroyBossShakeParam;
    public CameraShakeParam DestroyBossShakeParam => m_DestroyBossShakeParam;
}
