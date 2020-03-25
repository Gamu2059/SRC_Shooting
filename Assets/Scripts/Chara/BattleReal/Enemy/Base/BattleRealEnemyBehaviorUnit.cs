#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using BattleReal.BulletGenerator;

/// <summary>
/// リアルモードの敵を動かすための振る舞いの基底クラス。
/// </summary>
public class BattleRealEnemyBehaviorUnit : BattleRealEnemyBehaviorElement
{
    #region Define

    /// <summary>
    /// 弾生成に関するパラメータ
    /// </summary>
    [Serializable]
    public struct BulletGeneratorData
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
    /// 実際にインスタンスとして弾生成を管理する時に用いるデータ
    /// </summary>
    private struct BulletGeneratorFieldData
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

    [SerializeField, Tooltip("割り込み終了関数によって終了しなかった場合の、デフォルトの終了値")]
    private bool m_DefaultEndValue;

    [Header("Shot Parameter")]

    [SerializeField, Tooltip("弾生成に関するパラメータ")]
    private BulletGeneratorData[] m_BulletGeneratorDatas;

    #endregion

    protected BattleRealEnemyBase Enemy { get; private set; }
    protected BattleRealEnemyBehaviorController Controller { get; private set; }
    protected float CurrentTime { get; private set; }
    private List<BulletGeneratorFieldData> m_BulletGeneratorFieldDataList;

    public void OnStartUnit(BattleRealEnemyBase enemy, BattleRealEnemyBehaviorController controller)
    {
        Enemy = enemy;
        Controller = controller;
        CurrentTime = 0;

        m_BulletGeneratorFieldDataList = new List<BulletGeneratorFieldData>();
        for (var i = 0; i < m_BulletGeneratorDatas.Length; i++)
        {
            m_BulletGeneratorFieldDataList.Add(new BulletGeneratorFieldData()
            {
                Data = m_BulletGeneratorDatas[i],
                IsStart = false,
                IsStop = false,
                TimeCount = 0,
                TotalTimeCountFromFirstActivate = 0
            });
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

        m_BulletGeneratorFieldDataList.Clear();
        m_BulletGeneratorFieldDataList = null;
        Controller = null;
        Enemy = null;
    }

    public bool IsEndUnit()
    {
        return IsEnd();
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
                    BattleRealBulletGeneratorManager.Instance.CreateBulletGenerator(d.Data.ParamSet, Enemy);
                    AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot01Se);
                }

            d.TotalTimeCountFromFirstActivate += deltaTime;
            }
            else
            {
                if (d.TimeCount >= d.Data.StartTimeOnFirstActivate)
                {
                    d.IsStart = true;
                    d.TimeCount -= d.Data.StartTimeOnFirstActivate;
                    BattleRealBulletGeneratorManager.Instance.CreateBulletGenerator(d.Data.ParamSet, Enemy);
                    AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot01Se);
                }
            }

            d.TimeCount += deltaTime;
        }
    }

    #region Have to Override Method

    protected virtual void OnStart() { }

    protected virtual void OnUpdate(float deltaTime) { }

    protected virtual void OnLateUpdate(float deltaTime) { }

    protected virtual void OnEnd() { }

    protected virtual bool IsEnd() { return m_DefaultEndValue; }

    #endregion
}
