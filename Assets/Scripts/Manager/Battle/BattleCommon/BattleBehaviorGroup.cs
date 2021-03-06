﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// あるオブジェクトのサイクルを一纏めにしておきたい時に用いる。
/// </summary>
public class BattleBehaviorGroup : BattleControllableMonoBehavior
{
    [SerializeField]
    private List<BattleControllableMonoBehavior> m_BehaviorList;

    public override void OnInitialize()
    {
        base.OnInitialize();

        if (m_BehaviorList != null)
        {
            m_BehaviorList.ForEach((x) => x.OnInitialize());
        }
    }

    public override void OnFinalize()
    {
        base.OnFinalize();

        if (m_BehaviorList != null)
        {
            m_BehaviorList.ForEach((x) => x.OnFinalize());
        }
    }

    public override void OnStart()
    {
        base.OnStart();

        if (m_BehaviorList != null)
        {
            m_BehaviorList.ForEach((x) => x.OnStart());
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_BehaviorList != null)
        {
            m_BehaviorList.ForEach((x) => x.OnUpdate());
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (m_BehaviorList != null)
        {
            m_BehaviorList.ForEach((x) => x.OnLateUpdate());
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (m_BehaviorList != null)
        {
            m_BehaviorList.ForEach((x) => x.OnFixedUpdate());
        }
    }

    public override void OnEnableObject()
    {
        base.OnEnableObject();

        if (m_BehaviorList != null)
        {
            m_BehaviorList.ForEach((x) => x.OnEnableObject());
        }
    }

    public override void OnDisableObject()
    {
        base.OnDisableObject();

        if (m_BehaviorList != null)
        {
            m_BehaviorList.ForEach((x) => x.OnDisableObject());
        }
    }
}
