using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルのリアルモードの処理を管理する。
/// </summary>
public class BattleRealManager : BattleControllableMonoBehavior
{
    private BattleMainTimerManager m_TimerManager;
    private EventManager m_EventManager;
    private PlayerCharaManager m_PlayerManager;
    private EnemyCharaManager m_EnemyManager;
    private BulletManager m_BulletManager;
    private ItemManager m_ItemManager;

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_TimerManager.OnInitialize();
        m_EventManager.OnInitialize();
        m_PlayerManager.OnInitialize();
        m_EnemyManager.OnInitialize();
        m_BulletManager.OnInitialize();
        m_ItemManager.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_ItemManager.OnFinalize();
        m_BulletManager.OnFinalize();
        m_EnemyManager.OnFinalize();
        m_PlayerManager.OnFinalize();
        m_EventManager.OnFinalize();
        m_TimerManager.OnFinalize();

        base.OnFinalize();
    }

    public override void OnEnableObject()
    {
        base.OnEnableObject();

        m_TimerManager.OnEnableObject();
        m_EventManager.OnEnableObject();
        m_PlayerManager.OnEnableObject();
        m_EnemyManager.OnEnableObject();
        m_BulletManager.OnEnableObject();
        m_ItemManager.OnEnableObject();
    }

    public override void OnDisableObject()
    {
        m_ItemManager.OnDisableObject();
        m_BulletManager.OnDisableObject();
        m_EnemyManager.OnDisableObject();
        m_PlayerManager.OnDisableObject();
        m_EventManager.OnDisableObject();
        m_TimerManager.OnDisableObject();

        base.OnDisableObject();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_TimerManager.OnEnableObject();
        m_EventManager.OnEnableObject();
        m_PlayerManager.OnEnableObject();
        m_EnemyManager.OnEnableObject();
        m_BulletManager.OnEnableObject();
        m_ItemManager.OnEnableObject();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_TimerManager.OnUpdate();
        m_EventManager.OnUpdate();
        m_PlayerManager.OnUpdate();
        m_EnemyManager.OnUpdate();
        m_BulletManager.OnUpdate();
        m_ItemManager.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        m_TimerManager.OnLateUpdate();
        m_EventManager.OnLateUpdate();
        m_PlayerManager.OnLateUpdate();
        m_EnemyManager.OnLateUpdate();
        m_BulletManager.OnLateUpdate();
        m_ItemManager.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        m_TimerManager.OnFixedUpdate();
        m_EventManager.OnFixedUpdate();
        m_PlayerManager.OnFixedUpdate();
        m_EnemyManager.OnFixedUpdate();
        m_BulletManager.OnFixedUpdate();
        m_ItemManager.OnFixedUpdate();
    }

}
