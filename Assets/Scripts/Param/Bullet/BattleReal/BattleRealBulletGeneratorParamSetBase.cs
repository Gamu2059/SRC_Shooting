#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleReal.BulletGenerator
{
    /// <summary>
    /// リアルモードの弾生成パラメータを難易度に応じてセットでまとめるための基底クラス。
    /// </summary>
    public abstract class BattleRealBulletGeneratorParamSetBase : ScriptableObject
    {
        [SerializeField, Tooltip("パラメータを渡す弾生成クラスの名前")]
        private string m_GeneratorClassName;
        public string GeneratorClassName => m_GeneratorClassName;

        /// <summary>
        /// 難易度に応じて取得できるパラメータを変えるもの。
        /// </summary>
        public abstract IBattleRealBulletGeneratorParamBase GetGeneratorParam(E_DIFFICULTY difficulty);
    }
}
