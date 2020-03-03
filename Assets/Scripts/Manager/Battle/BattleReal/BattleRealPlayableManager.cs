#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// タイムライン制御するオブジェクトのマネージャ。
/// </summary>
public class BattleRealPlayableManager : ControllableMonoBehavior
{
    public static BattleRealPlayableManager Instance {
        get {
            if (BattleManager.Instance == null)
            {
                return null;
            }

            return null;
        }
    }

    [Serializable]
    public class Playable
    {
        public string Name;
        public BattleRealPlayableBase PlayableObject;
    }

    #region Field Inspector

    [SerializeField]
    private Playable[] m_Playables;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        foreach (var p in m_Playables)
        {
            if (p == null)
            {
                continue;
            }

            var dir = p.PlayableObject.PlayableDirector;
            p.PlayableObject.StopTimeline();
        }
    }

    public override void OnUpdate()
    {
        // Update処理
        foreach (var p in m_Playables)
        {
            if (p == null)
            {
                continue;
            }

            if (!p.PlayableObject.IsPaused)
            {
                p.PlayableObject.OnUpdate();
            }
        }
    }

    #endregion

    public void StartPlayable(string name)
    {
        foreach (var p in m_Playables)
        {
            if (name == p.Name)
            {
                p.PlayableObject.StartTimeline();
            }
        }
    }

    public void PausePlayable(string name)
    {
        foreach (var p in m_Playables)
        {
            if (name == p.Name)
            {
                p.PlayableObject.PauseTimeline();
            }
        }
    }

    public void ResumePlayable(string name)
    {
        foreach (var p in m_Playables)
        {
            if (name == p.Name)
            {
                p.PlayableObject.ResumeTimeline();
            }
        }
    }

    public void StopPlayable(string name)
    {
        foreach (var p in m_Playables)
        {
            if (name == p.Name)
            {
                p.PlayableObject.StopTimeline();
            }
        }
    }
}
