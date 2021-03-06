﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using BattleReal.BulletGenerator;

/// <summary>
/// リアルモードの敵を動かすための振る舞いの基底クラス。
/// </summary>
public class BattleRealEnemyBehaviorUnitBase : BattleRealEnemyBehaviorElement
{
    #region Define

    /// <summary>
    /// 弾生成に関するパラメータ
    /// </summary>
    [Serializable]
    private class BulletGeneratorData
    {
        [Tooltip("弾生成パラメータセット")]
        public BattleRealBulletGeneratorParamSetBase ParamSet;

        [Tooltip("この振る舞いに入ってから最初の弾生成インスタンスを発動するまでの時間")]
        public float StartTimeOnFirstActivate;

        [Tooltip("最初の弾生成インスタンスを発動して以降、発動を取りやめるまでの時間")]
        public float StopTimeFromFirstActivate;

        [Tooltip("最初の弾生成インスタンスを発動して以降の発動間隔")]
        public float ActivateInterval;
    }

    /// <summary>
    /// 難易度別の弾生成に関するパラメータ
    /// </summary>
    [Serializable]
    private class BulletGeneratorDataSet
    {
        [SerializeField]
        private E_DIFFICULTY m_Difficulty;
        public E_DIFFICULTY Difficulty => m_Difficulty;

        [SerializeField, Tooltip("弾生成に関するパラメータ")]
        private BulletGeneratorData[] m_BulletGeneratorDatas;
        public BulletGeneratorData[] BulletGeneratorDatas => m_BulletGeneratorDatas;
    }

    /// <summary>
    /// 実際にインスタンスとして弾生成を管理する時に用いるデータ
    /// </summary>
    private class BulletGeneratorFieldData
    {
        public BulletGeneratorData Data;
        public bool IsStart;
        public bool IsStop;
        public float TimeCount;
        public float TotalTimeCountFromFirstActivate;
    }

    #endregion

    #region Field Inspector

    [Header("End Parameter")]

    [SerializeField, Tooltip("終了条件に先立って、割り込み終了関数を適用するかどうか")]
    private bool m_ApplyInterruptEnd;

    [SerializeField, Tooltip("割り込み終了関数")]
    private BattleRealEnemyBehaviorInterruptFuncBase m_InterruptEndFunc;

    [SerializeField, Tooltip("割り込み終了関数によって終了しなかった場合の、デフォルトの終了値")]
    private bool m_DefaultEndValue;

    [Header("Option Parameter")]

    [SerializeField]
    private BattleRealEnemyBehaviorOptionFuncBase[] m_OnStartOptions;

    [SerializeField]
    private BattleRealEnemyBehaviorOptionFuncBase[] m_OnEndOptions;

    [SerializeField]
    private BattleRealEnemyBehaviorOptionFuncBase[] m_OnStopOptions;

    [Header("Shot Parameter")]

    [SerializeField, Tooltip("難易度を参照して、難易度別の弾生成パラメータを取得するかどうか")]
    private bool m_ReferenceDifficultyForBulletGenerator;

    [SerializeField, Tooltip("弾生成に関するパラメータ")]
    private BulletGeneratorData[] m_BulletGeneratorDatas;

    [SerializeField, Tooltip("難易度別の弾生成に関するパラメータ")]
    private List<BulletGeneratorDataSet> m_DifficultyBulletGeneratorDatas;

    [Space()]
    [SerializeField, Tooltip("振る舞いが終了した時に弾生成を破棄する")]
    private bool m_DestroyBulletGeneratorOnBehaviorEnd;

    #endregion

    protected BattleRealEnemyBase Enemy { get; private set; }
    protected BattleRealEnemyBehaviorController Controller { get; private set; }
    public float CurrentTime { get; private set; }
    private List<BulletGeneratorFieldData> m_BulletGeneratorFieldDataList;
    private List<BattleRealBulletGeneratorBase> m_BulletGeneratorList;

    public void OnStartUnit(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorController controller)
    {
        Enemy = enemy;
        Controller = controller;
        CurrentTime = 0;

        m_BulletGeneratorList = new List<BattleRealBulletGeneratorBase>();
        m_BulletGeneratorFieldDataList = new List<BulletGeneratorFieldData>();

        var bulletGeneratordatas = GetBulletGeneratorDatas();
        if (bulletGeneratordatas != null)
        {
            for (var i = 0; i < bulletGeneratordatas.Length; i++)
            {
                m_BulletGeneratorFieldDataList.Add(new BulletGeneratorFieldData()
                {
                    Data = bulletGeneratordatas[i],
                    IsStart = false,
                    IsStop = false,
                    TimeCount = 0,
                    TotalTimeCountFromFirstActivate = 0
                });
            }

        }

        if (m_OnStartOptions != null)
        {
            foreach (var option in m_OnStartOptions)
            {
                option?.Call(Enemy);
            }
        }

        OnStart();
    }

    public void OnUpdateUnit(float deltaTime)
    {
        CurrentTime += deltaTime;
        UpdateBulletGenerator(deltaTime);
        OnUpdate(deltaTime);
    }

    public void OnLateUpdateUnit(float deltaTime)
    {
        OnLateUpdate(deltaTime);
    }

    public void OnEndUnit()
    {
        OnEnd();

        if (m_OnEndOptions != null)
        {
            foreach (var option in m_OnEndOptions)
            {
                option?.Call(Enemy);
            }
        }

        if (m_BulletGeneratorList != null)
        {
            if (m_DestroyBulletGeneratorOnBehaviorEnd)
            {
                foreach (var g in m_BulletGeneratorList)
                {
                    g.Destroy();
                }
            }
            m_BulletGeneratorList.Clear();
            m_BulletGeneratorList = null;
        }

        m_BulletGeneratorFieldDataList?.Clear();
        m_BulletGeneratorFieldDataList = null;
        Controller = null;
        Enemy = null;
    }

    public bool IsEndUnit()
    {
        if (m_ApplyInterruptEnd && m_InterruptEndFunc != null)
        {
            if (m_InterruptEndFunc.IsInterruptEnd(Enemy, this))
            {
                return true;
            }
        }

        return IsEnd();
    }

    public void OnStopUnit()
    {
        OnStop();

        if (m_OnStopOptions != null)
        {
            foreach (var option in m_OnStopOptions)
            {
                option?.Call(Enemy);
            }
        }

        if (m_BulletGeneratorList != null)
        {
            if (m_DestroyBulletGeneratorOnBehaviorEnd)
            {
                foreach (var g in m_BulletGeneratorList)
                {
                    g.Destroy();
                }
            }
            m_BulletGeneratorList.Clear();
            m_BulletGeneratorList = null;
        }

        m_BulletGeneratorFieldDataList?.Clear();
        m_BulletGeneratorFieldDataList = null;
        Controller = null;
        Enemy = null;
    }

    private void UpdateBulletGenerator(float deltaTime)
    {
        for (var i = 0; i < m_BulletGeneratorFieldDataList.Count; i++)
        {
            var d = m_BulletGeneratorFieldDataList[i];

            if (d.IsStop)
            {
                continue;
            }

            if (d.IsStart)
            {
                if (d.TotalTimeCountFromFirstActivate >= d.Data.StopTimeFromFirstActivate)
                {
                    d.IsStop = true;
                    continue;
                }

                if (d.TimeCount >= d.Data.ActivateInterval)
                {
                    d.TimeCount -= d.Data.ActivateInterval;
                    var g = BattleRealBulletGeneratorManager.Instance.CreateBulletGenerator(d.Data.ParamSet, Enemy);
                    if (g != null)
                    {
                        m_BulletGeneratorList.Add(g);
                    }
                }

                d.TotalTimeCountFromFirstActivate += deltaTime;
            }
            else
            {
                if (d.TimeCount >= d.Data.StartTimeOnFirstActivate)
                {
                    d.IsStart = true;
                    d.TimeCount -= d.Data.StartTimeOnFirstActivate;
                    var g = BattleRealBulletGeneratorManager.Instance.CreateBulletGenerator(d.Data.ParamSet, Enemy);
                    if (g != null)
                    {
                        m_BulletGeneratorList.Add(g);
                    }
                }
            }

            d.TimeCount += deltaTime;
        }
    }

    /// <summary>
    /// 弾生成に関するパラメータを取得する。
    /// </summary>
    private BulletGeneratorData[] GetBulletGeneratorDatas()
    {
        if (!m_ReferenceDifficultyForBulletGenerator)
        {
            return m_BulletGeneratorDatas;
        }

        if (m_DifficultyBulletGeneratorDatas == null || DataManager.Instance == null)
        {
            return m_BulletGeneratorDatas;
        }

        var difficulty = DataManager.Instance.Difficulty;
        var foundData = m_DifficultyBulletGeneratorDatas.Find(d => d.Difficulty == difficulty);
        return foundData != null ? foundData.BulletGeneratorDatas : m_BulletGeneratorDatas;
    }

    #region Have to Override Method

    protected virtual void OnStart() { }

    protected virtual void OnUpdate(float deltaTime) { }

    protected virtual void OnLateUpdate(float deltaTime) { }

    protected virtual void OnEnd() { }

    protected virtual bool IsEnd() { return m_DefaultEndValue; }

    protected virtual void OnStop() { }

    #endregion
}
