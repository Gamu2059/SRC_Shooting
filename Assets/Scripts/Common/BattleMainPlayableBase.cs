using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;
using System;

/// <summary>
/// Timeline制御が可能なオブジェクトの基底クラス。
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class BattleMainPlayableBase : BattleMainObjectBase
{
    protected PlayableDirector m_PlayableDirector;

    protected TimelineParam m_TimelineParam;

    /// <summary>
    /// サイクル
    /// </summary>
    protected E_OBJECT_CYCLE m_Cycle;

    #region Get & Set
    public E_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    public void SetCycle(E_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    #endregion


    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PlayableDirector = GetComponent<PlayableDirector>();
        m_PlayableDirector.playOnAwake = false;
        m_PlayableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_PlayableDirector.time += Time.deltaTime;
        m_PlayableDirector.Evaluate();

        if (m_PlayableDirector.playableAsset != null && m_PlayableDirector.playableGraph.IsDone())
        {
            StopTimeline();
        }
    }

    /// <summary>
    /// PlayableManagerに登録する。
    /// </summary>
    protected void RegistPlayable()
    {
        if (PlayableManager.Instance != null)
        {
            PlayableManager.Instance.RegistObject(this);
        }
    }

    public void DestroyPlayable()
    {
        if (PlayableManager.Instance != null)
        {
            PlayableManager.Instance.DestroyObject(this);
        }
    }

    /// <summary>
    /// Timelineによる制御を開始する。
    /// </summary>
    /// <param name="timelineParam"></param>
    public void StartTimeline(TimelineParam timelineParam)
    {
        if (m_PlayableDirector == null)
        {
            return;
        }

        m_TimelineParam = timelineParam;

        m_PlayableDirector.Stop();
        m_PlayableDirector.playableAsset = m_TimelineParam.TimelineAsset;

        var outputs = m_PlayableDirector.playableAsset.outputs;

        // トラックをバインドする
        foreach (var trackBindParam in m_TimelineParam.TrackBindParams)
        {
            var target = transform.Find(trackBindParam.BindTargetName, false);
            if (target == null)
            {
                continue;
            }

            var binding = outputs.First(t => t.streamName == trackBindParam.TrackName);
            m_PlayableDirector.SetGenericBinding(binding.sourceObject, target);
        }

        // 参照をバインドする
        foreach(var referenceBindParam in m_TimelineParam.ReferenceBindParams)
        {
            var target = transform.Find(referenceBindParam.BindTargetName, false);
            if (target == null)
            {
                continue;
            }

            if (referenceBindParam.BindTargetComponentType == "GameObject")
            {
                m_PlayableDirector.SetReferenceValue(referenceBindParam.ReferenceName, target.gameObject);
            } else if (referenceBindParam.BindTargetComponentType == "Transform")
            {
                m_PlayableDirector.SetReferenceValue(referenceBindParam.ReferenceName, target);
            } else
            {
                var type = Type.GetType(referenceBindParam.BindTargetComponentType);
                if (type != null)
                {
                    var component = target.GetComponent(type);
                    if (component != null)
                    {
                        m_PlayableDirector.SetReferenceValue(referenceBindParam.ReferenceName, component);
                    }
                }
            }
        }

        m_PlayableDirector.Play();
    }

    public void PauseTimeline()
    {
        m_PlayableDirector.Pause();
    }

    public void ResumeTimeline()
    {
        m_PlayableDirector.Resume();
    }

    public void StopTimeline()
    {
        m_PlayableDirector.Stop();

        if (m_TimelineParam.IsDestroyEndTimeline)
        {
            DestroyPlayable();
        }
    }
}
