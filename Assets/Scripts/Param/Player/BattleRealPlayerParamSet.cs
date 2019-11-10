#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのプレイヤーのパラメータのセット。
/// </summary>
[Serializable]
//[Serializable, CreateAssetMenu(menuName = "Param/BattleRealPlayerParamSet", fileName = "param.battle_real_player_param_set")]
public class BattleRealPlayerParamSet
{
    [Serializable]
    public class LevelParam
    {
        [SerializeField]
        private int m_NormalShotPower;
        public int NormalShotPower => m_NormalShotPower;

        [SerializeField]
        private float m_NormalShotInterval;
        public float NormalShotInterval;
    }

    [SerializeField]
    private LevelParam[] m_LevelParams;
    public LevelParam[] LevelParams => m_LevelParams;

}
