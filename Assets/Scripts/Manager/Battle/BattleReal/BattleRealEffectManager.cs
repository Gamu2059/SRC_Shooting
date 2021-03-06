﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// リアルモードのエフェクトを管理する。
/// </summary>
[Serializable]
public class BattleRealEffectManager : Singleton<BattleRealEffectManager>
{
    #region Define

    private class SequentialData
    {
        public List<SequentialEffectParamSet.SequentialEffectParam> Effects;
        public Transform Owner;
        public float NowTime;
        public bool IsCancelOnOwnerNull;
        public Action<BattleCommonEffectController> OnCreateEffect;
        public bool WillDestroy;

        public SequentialData(SequentialEffectParamSet effects, Transform owner, bool isCancelOnOwnerNull, Action<BattleCommonEffectController> onCreateEffect)
        {
            if (effects == null || effects.Params == null)
            {
                WillDestroy = true;
                return;
            }

            Effects = effects.Params.ToList();
            Owner = owner;
            IsCancelOnOwnerNull = isCancelOnOwnerNull;
            OnCreateEffect = onCreateEffect;
            NowTime = 0;
            WillDestroy = false;
        }
    }

    #endregion

    #region Field

    private BattleRealEffectManagerParamSet m_ParamSet;

    private Transform m_EffectHolder;

    /// <summary>
    /// STANDBY状態のエフェクトを保持するリスト。
    /// </summary>
    private List<BattleCommonEffectController> m_StandbyEffects;

    /// <summary>
    /// UPDATE状態のエフェクトを保持するリスト。
    /// </summary>
    private List<BattleCommonEffectController> m_UpdateEffects;

    /// <summary>
    /// POOL状態のエフェクトを保持するリスト。
    /// </summary>
    private List<BattleCommonEffectController> m_PoolEffects;

    /// <summary>
    /// POOL状態に遷移するエフェクトのリスト。
    /// </summary>
    private List<BattleCommonEffectController> m_GotoPoolEffects;

    /// <summary>
    /// 登録状態になっただけのシーケンシャルデータ。
    /// </summary>
    private List<SequentialData> m_StandbySequentialDatas;

    /// <summary>
    /// 処理中のシーケンシャルデータ。
    /// </summary>
    private List<SequentialData> m_ProcessingSequentialDatas;

    #endregion

    public static BattleRealEffectManager Builder(BattleRealManager realManager, BattleRealEffectManagerParamSet param)
    {
        var manager = Create();
        manager.SetParam(param);
        manager.SetCallback(realManager);
        manager.OnInitialize();
        return manager;
    }

    private void SetCallback(BattleRealManager manager)
    {
        manager.ChangeStateAction += OnChangeStateBattleRealManager;
    }

    private void SetParam(BattleRealEffectManagerParamSet param)
    {
        m_ParamSet = param;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_StandbyEffects = new List<BattleCommonEffectController>();
        m_UpdateEffects = new List<BattleCommonEffectController>();
        m_PoolEffects = new List<BattleCommonEffectController>();
        m_GotoPoolEffects = new List<BattleCommonEffectController>();
        m_StandbySequentialDatas = new List<SequentialData>();
        m_ProcessingSequentialDatas = new List<SequentialData>();
    }

    public override void OnFinalize()
    {
        m_ProcessingSequentialDatas.Clear();
        m_StandbySequentialDatas.Clear();
        m_StandbyEffects.Clear();
        m_UpdateEffects.Clear();
        m_PoolEffects.Clear();
        m_GotoPoolEffects.Clear();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_EffectHolder = BattleRealStageManager.Instance.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.EFFECT);
    }

    public override void OnUpdate()
    {
        // Start処理
        foreach (var effect in m_StandbyEffects)
        {
            if (effect == null)
            {
                continue;
            }

            if (effect.Cycle == E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                effect.OnStart();
            }
        }

        GotoUpdateFromStandby();

        // Update処理
        foreach (var effect in m_UpdateEffects)
        {
            if (effect == null)
            {
                continue;
            }

            if (effect.Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                effect.OnUpdate();
            }
        }

        // 処理中シーケンシャルデータの処理
        foreach (var data in m_ProcessingSequentialDatas)
        {
            if (data == null)
            {
                continue;
            }

            if (data.IsCancelOnOwnerNull && data.Owner == null)
            {
                data.WillDestroy = true;
                continue;
            }

            foreach (var effect in data.Effects)
            {
                if (effect.Delay <= data.NowTime)
                {
                    var e = CreateEffect(effect.Effect, data.Owner);
                    data.OnCreateEffect?.Invoke(e);
                }
            }

            data.Effects.RemoveAll(d => d.Delay <= data.NowTime);
            data.NowTime += Time.deltaTime;

            if (data.Effects.Count < 1)
            {
                data.WillDestroy = true;
            }
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var effect in m_UpdateEffects)
        {
            if (effect == null)
            {
                continue;
            }

            if (effect.Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
            {
                effect.OnLateUpdate();
            }
        }

        // 除外判定
        foreach (var effect in m_UpdateEffects)
        {
            if (effect == null)
            {
                continue;
            }

            if (effect.Cycle == E_POOLED_OBJECT_CYCLE.STANDBY_CHECK_POOL)
            {
                CheckPoolEffect(effect);
            }
        }

        // 削除予定シーケンシャルデータを削除
        m_ProcessingSequentialDatas.RemoveAll(d => d.WillDestroy);

        // 登録シーケンシャルデータを処理中シーケンシャルデータへ
        m_ProcessingSequentialDatas.AddRange(m_StandbySequentialDatas);
        m_StandbySequentialDatas.Clear();
    }

    #endregion

    /// <summary>
    /// 破棄フラグが立っているものをプールに戻す
    /// </summary>
    public void GotoPool()
    {
        GotoPoolFromUpdate();
    }

    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateFromStandby()
    {
        foreach (var effect in m_StandbyEffects)
        {
            if (effect == null)
            {
                continue;
            }
            else if (effect.Cycle != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckPoolEffect(effect);
                continue;
            }

            effect.Cycle = E_POOLED_OBJECT_CYCLE.UPDATE;
            m_UpdateEffects.Add(effect);
        }

        m_StandbyEffects.Clear();
    }

    /// <summary>
    /// POOL状態にする。
    /// </summary>
    private void GotoPoolFromUpdate()
    {
        int count = m_GotoPoolEffects.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var effect = m_GotoPoolEffects[idx];
            effect.OnFinalize();
            effect.Cycle = E_POOLED_OBJECT_CYCLE.POOLED;
            effect.gameObject.SetActive(false);
            m_GotoPoolEffects.RemoveAt(idx);
            m_UpdateEffects.Remove(effect);
            m_PoolEffects.Add(effect);
        }

        m_GotoPoolEffects.Clear();
    }

    /// <summary>
    /// エフェクトをSTANDBY状態にして制御下に入れる。
    /// </summary>
    private void CheckStandbyEffect(BattleCommonEffectController effect)
    {
        if (effect == null || !m_PoolEffects.Contains(effect))
        {
            Debug.LogError("指定されたエフェクトを追加できませんでした。");
            return;
        }

        m_PoolEffects.Remove(effect);
        m_StandbyEffects.Add(effect);
        effect.gameObject.SetActive(true);
        effect.Cycle = E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE;
        effect.OnInitialize();
    }

    /// <summary>
    /// 指定したエフェクトを制御から外すためにチェックする。
    /// </summary>
    private void CheckPoolEffect(BattleCommonEffectController effect)
    {
        if (effect == null || m_GotoPoolEffects.Contains(effect))
        {
            Debug.LogError("指定したエフェクトを削除できませんでした。");
            return;
        }

        effect.Cycle = E_POOLED_OBJECT_CYCLE.STANDBY_POOL;
        m_GotoPoolEffects.Add(effect);
    }

    /// <summary>
    /// プールからエフェクトを取得する。
    /// 足りなければ生成する。
    /// </summary>
    private BattleCommonEffectController GetPoolingEffect(BattleCommonEffectController effectPrefab)
    {
        if (effectPrefab == null)
        {
            return null;
        }

        string bulletId = effectPrefab.EffectGroupId;
        BattleCommonEffectController effect = null;

        foreach (var e in m_PoolEffects)
        {
            if (e != null && e.EffectGroupId == bulletId)
            {
                effect = e;
                break;
            }
        }

        if (effect == null)
        {
            effect = GameObject.Instantiate(effectPrefab);
            effect.transform.SetParent(m_EffectHolder);
            m_PoolEffects.Add(effect);
        }

        return effect;
    }

    /// <summary>
    /// エフェクトを作成する。
    /// </summary>
    public BattleCommonEffectController CreateEffect(EffectParamSet paramSet, Transform owner)
    {
        if (paramSet == null)
        {
            return null;
        }

        var poolingEffect = GetPoolingEffect(paramSet.Effect);
        if (poolingEffect == null)
        {
            return null;
        }

        poolingEffect.OnCreateEffect(paramSet, owner);
        CheckStandbyEffect(poolingEffect);

        return poolingEffect;
    }

    /// <summary>
    /// シーケンシャルエフェクトを登録する。
    /// </summary>
    public void RegisterSequentialEffect(SequentialEffectParamSet paramSet, Transform owner, bool isCancelOnOwnerNull = false, Action<BattleCommonEffectController> onCreateEffect = null)
    {
        if (paramSet == null)
        {
            return;
        }

        var data = new SequentialData(paramSet, owner, isCancelOnOwnerNull, onCreateEffect);
        if (data.WillDestroy)
        {
            return;
        }

        m_StandbySequentialDatas.Add(data);
    }

    /// <summary>
    /// 全てのエフェクトを一時停止する。
    /// </summary>
    public void PauseAllEffect()
    {
        m_StandbyEffects.ForEach(e => e.Pause());
        m_UpdateEffects.ForEach(e => e.Pause());
    }

    /// <summary>
    /// 全てのエフェクトを再開する。
    /// </summary>
    public void ResumeAllEffect()
    {
        m_StandbyEffects.ForEach(e => e.Resume());
        m_UpdateEffects.ForEach(e => e.Resume());
    }

    /// <summary>
    /// 全てのエフェクトを止める。
    /// 破棄ではない。
    /// </summary>
    public void StopAllEffect(bool isImmediate)
    {
        m_StandbyEffects.ForEach(e => e.Stop(isImmediate));
        m_UpdateEffects.ForEach(e => e.Stop(isImmediate));
    }

    /// <summary>
    /// 全てのエフェクトを破棄する。
    /// </summary>
    public void DestroyAllEffect(bool isImmediate)
    {
        m_StandbySequentialDatas.ForEach(s => s.WillDestroy = true);
        m_ProcessingSequentialDatas.ForEach(s => s.WillDestroy = true);
        m_StandbyEffects.ForEach(e => e.Cycle = E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        m_UpdateEffects.ForEach(e => e.DestroyEffect(isImmediate));
    }

    /// <summary>
    /// 指定したオーナーを持つエフェクトを破棄する。
    /// </summary>
    public void DestroyEffectByOwner(Transform owner, bool isImmediate)
    {
        foreach (var s in m_StandbySequentialDatas)
        {
            if (s.Owner == owner)
            {
                s.WillDestroy = true;
            }
        }

        foreach (var s in m_ProcessingSequentialDatas)
        {
            if (s.Owner == owner)
            {
                s.WillDestroy = true;
            }
        }

        // DestroyEffectを呼び出しても意味がないので、サイクルを上書きして破棄にもっていく
        foreach (var e in m_StandbyEffects)
        {
            if (e.Owner == owner)
            {
                e.Cycle = E_POOLED_OBJECT_CYCLE.STANDBY_POOL;
            }
        }

        foreach (var e in m_UpdateEffects)
        {
            if (e.Owner == owner)
            {
                e.DestroyEffect(isImmediate);
            }
        }
    }

    private void OnChangeStateBattleRealManager(E_BATTLE_REAL_STATE state)
    {
        switch (state)
        {
            case E_BATTLE_REAL_STATE.TO_HACKING:
                PauseAllEffect();
                break;
            case E_BATTLE_REAL_STATE.FROM_HACKING:
                ResumeAllEffect();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 弾消しエフェクトを表示する
    /// </summary>
    public void CreateBulletRemoveEffect(Transform owner, int value)
    {
        var effect = CreateEffect(m_ParamSet.BulletRemoveEffect, owner);
        if (effect != null && effect is BattleRealBulletRemoveEffect e)
        {
            e.SetPoint(value);
        }
    }
}
