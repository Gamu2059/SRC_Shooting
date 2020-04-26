#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Bulletに渡すパラメータ。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleCommon/BulletParam", fileName = "param.bullet.asset", order = 0)]
public class BulletParam : ScriptableObject
{
    #region Define

    [Serializable]
    private class DifficultySet
    {
        public E_DIFFICULTY Difficulty;
        public BulletOrbitalParam Param;
    }

    #endregion

    [Tooltip("弾の継続時間 これを超えると自動消滅する")]
    public float LifeTime;

    [Tooltip("弾発射時のSE 指定しない場合は何も鳴らない")]
    public PlaySoundParam ShotSE;

    [SerializeField, Tooltip("難易度を参照して、難易度別のパラメータを取得するかどうか")]
    private bool m_ReferenceDifficulty;

    [Tooltip("発射時のパラメータ")]
    public BulletOrbitalParam OrbitalParam;

    [SerializeField, Tooltip("難易度別の発射時のパラメータ")]
    private List<DifficultySet> m_DifficultyReferecedOrbitalParams;

    [HideInInspector, Tooltip("特殊な条件を満たす時のパラメータ")]
    public BulletOrbitalParam[] ConditionalOrbitalParams;

    /// <summary>
    /// 指定したインデックスのConditionalOrbitalParamを取得する。
    /// もし範囲外ならデフォルトのOrbitalParamを取得する。
    /// </summary>
    public BulletOrbitalParam GetOrbitalParam(int orbitalIndex = -1)
    {
        if (!m_ReferenceDifficulty)
        {
            return OrbitalParam;
        }

        if (m_DifficultyReferecedOrbitalParams == null || DataManager.Instance == null)
        {
            return OrbitalParam;
        }

        var difficulty = DataManager.Instance.Difficulty;
        var foundParam = m_DifficultyReferecedOrbitalParams.Find(p => p.Difficulty == difficulty);
        return foundParam != null ? foundParam.Param : OrbitalParam;

        //if (orbitalIndex < 0 || orbitalIndex >= ConditionalOrbitalParams.Length)
        //{
        //    return OrbitalParam;
        //}

        //return ConditionalOrbitalParams[orbitalIndex];
    }
}
