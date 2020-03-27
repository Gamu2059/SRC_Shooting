using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleReal.BulletGenerator;

/// <summary>
/// リアルモードの弾生成インスタンスを管理する。
/// </summary>
[Serializable]
public class BattleRealBulletGeneratorManager : Singleton<BattleRealBulletGeneratorManager>
{
    #region Field

    private LinkedList<BattleRealBulletGeneratorBase> m_StandbyUpdateGenerators;
    private LinkedList<BattleRealBulletGeneratorBase> m_UpdateGenerators;
    private LinkedList<BattleRealBulletGeneratorBase> m_StandbyPoolGenerators;
    private Dictionary<string, LinkedList<BattleRealBulletGeneratorBase>> m_PoolGeneratorDict;

    #endregion

    public static BattleRealBulletGeneratorManager Builder()
    {
        var manager = Create();
        manager.OnInitialize();
        return manager;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_StandbyUpdateGenerators = new LinkedList<BattleRealBulletGeneratorBase>();
        m_UpdateGenerators = new LinkedList<BattleRealBulletGeneratorBase>();
        m_StandbyPoolGenerators = new LinkedList<BattleRealBulletGeneratorBase>();
        m_PoolGeneratorDict = new Dictionary<string, LinkedList<BattleRealBulletGeneratorBase>>();
    }

    public override void OnFinalize()
    {
        foreach (var g in m_PoolGeneratorDict)
        {
            g.Value?.Clear();
        }
        m_PoolGeneratorDict.Clear();

        m_StandbyPoolGenerators.Clear();
        m_UpdateGenerators.Clear();
        m_StandbyUpdateGenerators.Clear();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        // BulletGeneratorはOnFixedUpdateしか拡張できないという仕様でいく

        // Start処理
        foreach (var g in m_StandbyUpdateGenerators)
        {
            if (g == null)
            {
                continue;
            }
            else if (g.Cycle != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckStandbyPool(g);
            }

            g.OnStart();
        }

        GotoUpdateFromStandbyUpdate();

        // Update処理
        foreach (var g in m_UpdateGenerators)
        {
            if (g == null)
            {
                continue;
            }
            else if (g.Cycle != E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                CheckStandbyPool(g);
            }

            g.OnFixedUpdate();
        }
    }

    #endregion

    /// <summary>
    /// 破棄フラグが立っているものをプールに戻す
    /// </summary>
    public void GotoPool()
    {
        GotoPoolFromStandbyPool();
    }

    /// <summary>
    /// STANDBY_UPDATEからUPDATEに遷移させる
    /// </summary>
    private void GotoUpdateFromStandbyUpdate()
    {
        foreach (var g in m_StandbyUpdateGenerators)
        {
            if (g == null)
            {
                continue;
            }
            else if (g.Cycle != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckStandbyPool(g);
                continue;
            }

            g.Cycle = E_POOLED_OBJECT_CYCLE.UPDATE;
            m_UpdateGenerators.AddLast(g);
        }

        m_StandbyUpdateGenerators.Clear();
    }

    /// <summary>
    /// STANDBY_POOLからPOOLに遷移させる
    /// </summary>
    private void GotoPoolFromStandbyPool()
    {
        foreach (var g in m_StandbyPoolGenerators)
        {
            if (g == null)
            {
                continue;
            }

            g.OnFinalize();
            g.Cycle = E_POOLED_OBJECT_CYCLE.POOLED;

            var className = g.GetType().Name;
            LinkedList<BattleRealBulletGeneratorBase> poolList = null;
            if (m_PoolGeneratorDict.TryGetValue(className, out poolList))
            {
                if (poolList == null)
                {
                    poolList = new LinkedList<BattleRealBulletGeneratorBase>();
                    m_PoolGeneratorDict[className] = poolList;
                }
            }
            else
            {
                poolList = new LinkedList<BattleRealBulletGeneratorBase>();
                m_PoolGeneratorDict.Add(className, poolList);
            }

            poolList.AddLast(g);

            // どこから入ってきたか分からないため両方のリストで削除を行う
            m_StandbyUpdateGenerators.Remove(g);
            m_UpdateGenerators.Remove(g);
        }

        m_StandbyPoolGenerators.Clear();
    }

    /// <summary>
    /// 弾生成インスタンスをSTANDBY_UPDATE状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyUpdate(BattleRealBulletGeneratorBase g)
    {
        if (g == null)
        {
            return;
        }

        m_StandbyUpdateGenerators.AddLast(g);
        g.Cycle = E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE;
        g.OnInitialize();
    }

    /// <summary>
    /// 弾生成インスタンスをSTANDBY_POOL状態にして制御下から外す。
    /// </summary>
    public void CheckStandbyPool(BattleRealBulletGeneratorBase g)
    {
        if (g == null)
        {
            return;
        }

        g.Cycle = E_POOLED_OBJECT_CYCLE.STANDBY_POOL;
        m_StandbyPoolGenerators.AddLast(g);
    }

    /// <summary>
    /// プールから弾生成インスタンスを取得する。
    /// 足りなければ生成する。
    /// </summary>
    private BattleRealBulletGeneratorBase GetPoolingGenerator(string className)
    {
        Type classType = null;

        try
        {
            classType = Type.GetType(className);
        }
        catch (Exception)
        {
            Debug.LogErrorFormat("弾生成クラスの名前を認識できませんでした。 name : {0}", className);
            return null;
        }

        BattleRealBulletGeneratorBase g = null;
        if (m_PoolGeneratorDict.TryGetValue(className, out LinkedList<BattleRealBulletGeneratorBase> list))
        {
            if (list.Count > 0)
            {
                // リストから一つ取り出す
                g = list.First.Value;
                list.RemoveFirst();
            }
        }
        else
        {
            // リストをDictに作成する
            m_PoolGeneratorDict.Add(className, new LinkedList<BattleRealBulletGeneratorBase>());
        }

        if (g == null)
        {
            var cstr = classType.GetConstructor(new Type[0]);
            if (cstr == null)
            {
                Debug.LogErrorFormat("{0}クラスにはデフォルトコンストラクタが定義されていないため、生成できませんでした。", className);
                return null;
            }

            g = cstr.Invoke(new object[0]) as BattleRealBulletGeneratorBase;
        }

        return g;
    }

    /// <summary>
    /// 弾生成インスタンスを作成する。
    /// </summary>
    /// <param name="paramSet">弾生成パラメータセット</param>
    /// <param name="owner">弾を発射するキャラ</param>
    public BattleRealBulletGeneratorBase CreateBulletGenerator(BattleRealBulletGeneratorParamSetBase paramSet, BattleRealCharaController owner)
    {
        if (paramSet == null)
        {
            return null;
        }

        var difficulty = DataManager.Instance.BattleData.Difficulty;
        var param = paramSet.GetGeneratorParam(difficulty);
        if (param == null)
        {
            Debug.LogErrorFormat("難易度に応じたパラメータがありません。 paramSet : {0}, difficulty : {1}", paramSet.name, difficulty);
            return null;
        }

        var g = GetPoolingGenerator(paramSet.GeneratorClassName);
        if (g == null)
        {
            return null;
        }
     
        g.SetParam(param, owner);
        CheckStandbyUpdate(g);

        return g;
    }
}
