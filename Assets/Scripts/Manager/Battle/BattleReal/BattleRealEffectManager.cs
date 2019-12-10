using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのエフェクトを管理する。
/// </summary>
[Serializable]
public class BattleRealEffectManager : ControllableObject
{
    public static BattleRealEffectManager Instance {
        get {
            if (BattleRealManager.Instance == null)
            {
                return null;
            }

            return BattleRealManager.Instance.EffectManager;
        }
    }

    #region Field

    private Transform m_EffectHolder;

    /// <summary>
    /// STANDBY状態のエフェクトを保持するリスト。
    /// </summary>
    private List<BattleRealEffectController> m_StandbyEffects;

    /// <summary>
    /// UPDATE状態のエフェクトを保持するリスト。
    /// </summary>
    private List<BattleRealEffectController> m_UpdateEffects;

    /// <summary>
    /// POOL状態のエフェクトを保持するリスト。
    /// </summary>
    private List<BattleRealEffectController> m_PoolEffects;

    /// <summary>
    /// POOL状態に遷移するエフェクトのリスト。
    /// </summary>
    private List<BattleRealEffectController> m_GotoPoolEffects;

    #endregion

    public BattleRealEffectManager()
    {

    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_StandbyEffects = new List<BattleRealEffectController>();
        m_UpdateEffects = new List<BattleRealEffectController>();
        m_PoolEffects = new List<BattleRealEffectController>();
        m_GotoPoolEffects = new List<BattleRealEffectController>();

        BattleRealManager.Instance.OnTransitionToHacking += PauseAllEffect;
        BattleRealManager.Instance.OnTransitionToReal += ResumeAllEffect;
    }

    public override void OnFinalize()
    {
        BattleRealManager.Instance.OnTransitionToReal -= ResumeAllEffect;
        BattleRealManager.Instance.OnTransitionToHacking -= PauseAllEffect;

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

            effect.OnStart();
        }

        GotoUpdateFromStandby();

        // Update処理
        foreach (var effect in m_UpdateEffects)
        {
            if (effect == null)
            {
                continue;
            }

            effect.OnUpdate();
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

            effect.OnLateUpdate();
        }
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
            var bullet = m_GotoPoolEffects[idx];
            bullet.OnFinalize();
            bullet.Cycle = E_POOLED_OBJECT_CYCLE.POOLED;
            bullet.gameObject.SetActive(false);
            m_GotoPoolEffects.RemoveAt(idx);
            m_UpdateEffects.Remove(bullet);
            m_PoolEffects.Add(bullet);
        }

        m_GotoPoolEffects.Clear();
    }

    /// <summary>
    /// エフェクトをSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyEffect(BattleRealEffectController effect)
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
    public void CheckPoolEffect(BattleRealEffectController effect)
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
    /// <param name="effectPrefab">取得や生成の情報源となるエフェクトのプレハブ</param>
    /// <param name="owner">エフェクトを作成したもの</param>
    public BattleRealEffectController GetPoolingBullet(BattleRealEffectController effectPrefab, Transform owner)
    {
        if (effectPrefab == null)
        {
            return null;
        }

        string bulletId = effectPrefab.EffectGroupId;
        BattleRealEffectController effect = null;

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

        effect.Owner = owner;

        CheckStandbyEffect(effect);

        return effect;
    }

    /// <summary>
    /// 全てのエフェクトを一時停止する。
    /// </summary>
    public void PauseAllEffect()
    {
        foreach (var e in m_StandbyEffects)
        {
            e.Pause();
        }

        foreach (var e in m_UpdateEffects)
        {
            e.Pause();
        }
    }

    /// <summary>
    /// 全てのエフェクトを再開する。
    /// </summary>
    public void ResumeAllEffect()
    {
        foreach (var e in m_StandbyEffects)
        {
            e.Resume();
        }

        foreach (var e in m_UpdateEffects)
        {
            e.Resume();
        }
    }

    /// <summary>
    /// 全てのエフェクトを止める。
    /// 破棄ではない。
    /// </summary>
    public void StopAllEffect(bool isImmediate)
    {
        foreach (var e in m_StandbyEffects)
        {
            e.Stop(isImmediate);
        }

        foreach (var e in m_UpdateEffects)
        {
            e.Stop(isImmediate);
        }
    }
}
