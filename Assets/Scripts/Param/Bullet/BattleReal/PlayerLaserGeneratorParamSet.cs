using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// プレイヤーレーザージェネレータのパラメータセット。
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Bullet/BulletGenerator/PlayerLaser", fileName = "param.bullet_generator.asset", order = 0)]
    public class PlayerLaserGeneratorParamSet : BattleRealBulletGeneratorParamSetBase
    {
        [Serializable]
        private class Set
        {
            public E_DIFFICULTY Difficulty;
            public PlayerLaserGeneratorParam Param;
        }

        [SerializeField]
        private Set[] m_Params;

        public override IBattleRealBulletGeneratorParamBase GetGeneratorParam(E_DIFFICULTY difficulty)
        {
            var set = m_Params.First(s => s.Difficulty == difficulty);
            return set?.Param;
        }
    }
}
