#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

/// <summary>
/// AdvEngineを管理する。
/// </summary>
public class AdvEngineManager : GlobalSingletonMonoBehavior<AdvEngineManager>
{
    [SerializeField]
    private GameObject m_AdvManagersPrefab;

    public AdvEngine AdvEngine { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        var advManagers = Instantiate(m_AdvManagersPrefab);
        advManagers.transform.SetParent(transform);
        advManagers.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        AdvEngine = GetComponentInChildren<AdvEngine>();
    }
}
