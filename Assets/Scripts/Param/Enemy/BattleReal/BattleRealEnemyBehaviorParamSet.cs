using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の振る舞いパラメータの規定クラス。
/// このクラスを継承してパラメータのセットを定義して下さい。
/// </summary>
public abstract class BattleRealEnemyBehaviorParamSet : ScriptableObject
{
    [Serializable]
    public class BulletSet
    {
        public string Name;
        public BulletController Bullet;
    }

    [SerializeField]
    private string m_BehaviorClass;
    public string BehaviorClass => m_BehaviorClass;

    [SerializeField]
    private List<BulletSet> m_BulletSets;
    public List<BulletSet> BulletSets => m_BulletSets;

    [SerializeField]
    private BattleRealEnemyLookParamSet m_EnemyLookParamSet;
    public BattleRealEnemyLookParamSet EnemyLookParamSet => m_EnemyLookParamSet;
}