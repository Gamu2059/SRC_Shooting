using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;
using System;

/// <summary>
/// リアルモードのTimeline制御が可能なオブジェクトの基底クラス。
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class BattleRealPlayableBase : ControllableMonoBehavior
{
    #region Field

    protected PlayableDirector m_PlayableDirector;

    protected TimelineParam m_TimelineParam;

    protected E_OBJECT_CYCLE m_Cycle;

    public bool IsPaused { get; private set; }

    public Vector3 RecordedPosition { get; private set; }
    public Vector3 RecordedRotation { get; private set; }
    public Vector3 RecordedLocalPosition { get; private set; }
    public Vector3 RecordedLocalRotation { get; private set; }
    public Vector3 RecordedLocalScale { get; private set; }

    #endregion

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

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PlayableDirector = GetComponent<PlayableDirector>();
        m_PlayableDirector.playOnAwake = false;
        m_PlayableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
        IsPaused = false;
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

    #endregion

    /// <summary>
    /// BattleRealPlayableManagerに登録する。
    /// </summary>
    protected void RegistPlayable()
    {
        if (PlayableManager.Instance != null)
        {
            PlayableManager.Instance.RegistObject(this);
        }
    }

    /// <summary>
    /// BattleRealPlayableManagerから外す。
    /// </summary>
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
    public void StartTimeline(TimelineParam timelineParam)
    {
        if (m_PlayableDirector == null)
        {
            return;
        }

        m_TimelineParam = timelineParam;

        m_PlayableDirector.Stop();
        m_PlayableDirector.playableAsset = m_TimelineParam.TimelineAsset;
        m_PlayableDirector.initialTime = 0;

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
        foreach (var referenceBindParam in m_TimelineParam.ReferenceBindParams)
        {
            var target = transform.Find(referenceBindParam.BindTargetName, false);
            if (target == null)
            {
                continue;
            }

            if (referenceBindParam.BindTargetComponentType == "GameObject")
            {
                m_PlayableDirector.SetReferenceValue(referenceBindParam.ReferenceName, target.gameObject);
            }
            else if (referenceBindParam.BindTargetComponentType == "Transform")
            {
                m_PlayableDirector.SetReferenceValue(referenceBindParam.ReferenceName, target);
            }
            else
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

            m_PlayableDirector.SetReferenceValue(BattleAnimationPlayableAsset.PLAYABLE_OBJECT, this);
        }

        m_PlayableDirector.Play();
    }

    public void PauseTimeline()
    {
        IsPaused = true;
        m_PlayableDirector.initialTime = m_PlayableDirector.time;
    }

    public void ResumeTimeline()
    {
        IsPaused = false;
    }

    public void StopTimeline()
    {
        m_PlayableDirector.Stop();

        if (m_TimelineParam.IsDestroyEndTimeline)
        {
            DestroyPlayable();
        }
    }

    public void RecordTransform()
    {
        RecordedPosition = transform.position;
        RecordedRotation = transform.eulerAngles;
        RecordedLocalPosition = transform.localPosition;
        RecordedLocalRotation = transform.localEulerAngles;
        RecordedLocalScale = transform.localScale;
    }
}
