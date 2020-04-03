#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// 座標指定弾ジェネレータのパラメータセット。
    /// </summary>
    [Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Bullet/BulletGenerator/PointTween", fileName = "param.bullet_generator.asset", order = 20)]
    public class PointTweenBulletGeneratorParamSet : BattleRealBulletGeneratorParamSetBase
    {
        [Serializable]
        private class Set
        {
            public E_DIFFICULTY Difficulty;
            public PointTweenBulletGeneratorParam Param;
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
