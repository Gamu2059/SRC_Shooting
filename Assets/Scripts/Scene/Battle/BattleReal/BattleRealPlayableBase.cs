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

    public PlayableDirector PlayableDirector { get; private set; }

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

        PlayableDirector = GetComponent<PlayableDirector>();
        PlayableDirector.playOnAwake = false;
        PlayableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
        IsPaused = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        PlayableDirector.time += Time.deltaTime;
        PlayableDirector.Evaluate();

        if (PlayableDirector.playableAsset != null && PlayableDirector.playableGraph.IsDone())
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
        //if (BattleRealPlayableManager.Instance != null)
        //{
        //    BattleRealPlayableManager.Instance.RegistObject(this);
        //}
    }

    /// <summary>
    /// BattleRealPlayableManagerから外す。
    /// </summary>
    public void DestroyPlayable()
    {
        //if (BattleRealPlayableManager.Instance != null)
        //{
        //    BattleRealPlayableManager.Instance.DestroyObject(this);
        //}
    }

    /// <summary>
    /// Timelineによる制御を開始する。
    /// </summary>
    public void StartTimeline(TimelineParam timelineParam)
    {
        if (PlayableDirector == null)
        {
            return;
        }

        m_TimelineParam = timelineParam;
        if (m_TimelineParam == null)
        {
            Debug.LogError("TimelineParam is null");
            return;
        }

        PlayableDirector.Stop();
        PlayableDirector.playableAsset = m_TimelineParam.TimelineAsset;
        PlayableDirector.initialTime = 0;

        var outputs = PlayableDirector.playableAsset.outputs;

        // トラックをバインドする
        foreach (var trackBindParam in m_TimelineParam.TrackBindParams)
        {
            var target = transform.Find(trackBindParam.BindTargetName, false);
            if (target == null)
            {
                continue;
            }

            var binding = outputs.First(t => t.streamName == trackBindParam.TrackName);
            PlayableDirector.SetGenericBinding(binding.sourceObject, target);
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
                PlayableDirector.SetReferenceValue(referenceBindParam.ReferenceName, target.gameObject);
            }
            else if (referenceBindParam.BindTargetComponentType == "Transform")
            {
                PlayableDirector.SetReferenceValue(referenceBindParam.ReferenceName, target);
            }
            else
            {
                var type = Type.GetType(referenceBindParam.BindTargetComponentType);
                if (type != null)
                {
                    var component = target.GetComponent(type);
                    if (component != null)
                    {
                        PlayableDirector.SetReferenceValue(referenceBindParam.ReferenceName, component);
                    }
                }
            }

            //PlayableDirector.SetReferenceValue(BattleAnimationPlayableAsset.PLAYABLE_OBJECT, this);
        }

        PlayableDirector.Play();
    }

    /// <summary>
    /// 今設定されているTimelineを再生する。
    /// </summary>
    public void StartTimeline()
    {
        if (PlayableDirector == null)
        {
            return;
        }

        PlayableDirector.Stop();
        PlayableDirector.initialTime = 0;
        PlayableDirector.Play();
    }

    public void PauseTimeline()
    {
        IsPaused = true;
        PlayableDirector.initialTime = PlayableDirector.time;
    }

    public void ResumeTimeline()
    {
        IsPaused = false;
    }

    public void StopTimeline()
    {
        PlayableDirector.Stop();

        if (m_TimelineParam != null && m_TimelineParam.IsDestroyEndTimeline)
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
