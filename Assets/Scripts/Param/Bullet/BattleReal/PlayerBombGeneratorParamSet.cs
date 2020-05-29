using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// プレイヤーボムジェネレータのパラメータセット。
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Bullet/BulletGenerator/PlayerBomb", fileName = "param.bullet_generator.asset", order = 0)]
    public class PlayerBombGeneratorParamSet : BattleRealBulletGeneratorParamSetBase
    {
        [Serializable]
        private class Set
        {
            public E_DIFFICULTY Difficulty;
            public PlayerBombGeneratorParam Param;
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
