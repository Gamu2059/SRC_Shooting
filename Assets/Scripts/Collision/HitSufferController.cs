using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 衝突状態を管理するコントローラ。
/// </summary>
public class HitSufferController<T> where T : BattleObjectBase
{
    public Dictionary<HitSet, HitSufferData<T>> m_HitSets;

    public Action<HitSufferData<T>> OnEnter;
    public Action<HitSufferData<T>> OnStay;
    public Action<HitSufferData<T>> OnExit;

    public HitSufferController()
    {
        m_HitSets = new Dictionary<HitSet, HitSufferData<T>>();
    }

    public void OnFinalize()
    {
        OnEnter = null;
        OnStay = null;
        OnExit = null;
        m_HitSets.Clear();
    }

    public bool IsContainHitSet(Transform hit, Transform suffer)
    {
        if (m_HitSets == null)
        {
            return false;
        }

        foreach (var key in m_HitSets.Keys)
        {
            if (hit == key.Hit && suffer == key.Suffer)
            {
                return true;
            }
        }

        return false;
    }

    public HitSufferData<T> Get(Transform hit, Transform suffer)
    {
        if (m_HitSets == null || hit == null || suffer == null)
        {
            return null;
        }

        foreach (var key in m_HitSets.Keys)
        {
            if (hit == key.Hit && suffer == key.Suffer)
            {
                return m_HitSets[key];
            }
        }

        return null;
    }

    public void Put(T opponentObject, ColliderData hitCollider, ColliderData sufferCollider, List<Vector2> hitPosList)
    {
        if (opponentObject == null || hitCollider == null || sufferCollider == null)
        {
            Debug.LogError("衝突情報を記録できません");
            return;
        }

        HitSufferData<T> data;
        var hit = hitCollider.Transform.Transform;
        var suffer = sufferCollider.Transform.Transform;
        if (!IsContainHitSet(hit, suffer))
        {
            data = new HitSufferData<T>(opponentObject, hitCollider, sufferCollider);
            m_HitSets.Add(new HitSet(hit, suffer), data);
        }
        else
        {
            data = Get(hit, suffer);
        }

        data.CountUp();
        data.Positions = hitPosList;
        data.IsUpdateFlag = true;
    }

    public void ClearUpdateFlag()
    {
        foreach (var v in m_HitSets.Values)
        {
            v.IsUpdateFlag = false;
        }
    }

    public void ProcessCollision()
    {
        CallAction();
        RemoveData();
    }

    private void CallAction()
    {
        foreach (var p in m_HitSets)
        {
            var v = p.Value;
            if (v.IsUpdateFlag)
            {
                if (v.Count < 2)
                {
                    OnEnter?.Invoke(v);
                }
                else
                {
                    OnStay?.Invoke(v);
                }
            }
            else
            {
                OnExit?.Invoke(v);
            }
        }
    }

    private void RemoveData()
    {
        List<HitSet> removeData = null;
        foreach (var p in m_HitSets)
        {
            if (!p.Value.IsUpdateFlag)
            {
                if (removeData == null)
                {
                    removeData = new List<HitSet>();
                }

                removeData.Add(p.Key);
            }
        }

        if (removeData == null)
        {
            return;
        }

        foreach (var data in removeData)
        {
            m_HitSets.Remove(data);
        }
    }
}
